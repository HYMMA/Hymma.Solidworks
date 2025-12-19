// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Microsoft.Win32;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// Manages starting multiple SolidWorks instances without journal file conflicts.
    /// </summary>
    /// <remarks>
    /// When starting multiple SolidWorks instances, each instance tries to write to the same journal file,
    /// causing a warning dialog. This class temporarily modifies the registry to give each instance
    /// a unique journal folder path, avoiding the conflict.
    /// <para>
    /// Supports multiple installed SolidWorks versions (e.g., 2023, 2024, 2025) and can modify
    /// registry settings for all versions or a specific version.
    /// </para>
    /// <para>
    /// <b>Note:</b> Starting new SolidWorks instances is inherently slow (60+ seconds per instance).
    /// For most use cases, consider using the existing SolidWorks instance instead.
    /// </para>
    /// </remarks>
    [Obsolete("This class is not practical for production use due to SolidWorks startup time (~60+ seconds per instance). Consider using the existing SolidWorks instance or COM automation with CreateObject instead.")]
    public class MultiInstanceSolidworksManager : IDisposable
    {
        private const string SolidWorksRegistryPath = @"Software\SolidWorks";
        private const string JournalFoldersValueName = "SolidWorks Journal Folders";
        private const string ExtReferencesSubKey = "ExtReferences";

        private readonly List<SolidWorksInstanceInfo> _instances = new List<SolidWorksInstanceInfo>();
        private readonly object _lockObject = new object();
        private bool _disposed;

        /// <summary>
        /// Gets the list of active SolidWorks instances managed by this manager.
        /// </summary>
        public IReadOnlyList<SolidWorksInstanceInfo> Instances => _instances.AsReadOnly();

        /// <summary>
        /// Starts a new SolidWorks instance with a unique journal folder to avoid conflicts.
        /// Modifies the registry for ALL installed SolidWorks versions to prevent journal file conflicts
        /// regardless of which version starts.
        /// </summary>
        /// <param name="executablePath">Path to SLDWORKS.exe. If null, uses default installation path.</param>
        /// <param name="timeoutSeconds">Timeout in seconds to wait for SolidWorks to start.</param>
        /// <param name="windowStyle">Window style for the new process.</param>
        /// <returns>Information about the started SolidWorks instance.</returns>
        /// <exception cref="TimeoutException">Thrown when SolidWorks doesn't start within the timeout period.</exception>
        /// <exception cref="InvalidOperationException">Thrown when unable to find or modify registry settings.</exception>
        public SolidWorksInstanceInfo StartInstance(
            string executablePath = null,
            int timeoutSeconds = 60,
            ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal)
        {
            return StartInstance(executablePath, timeoutSeconds, windowStyle, modifyAllVersions: true, targetVersion: null);
        }

        /// <summary>
        /// Starts a new SolidWorks instance with a unique journal folder to avoid conflicts.
        /// </summary>
        /// <param name="executablePath">Path to SLDWORKS.exe. If null, uses default installation path.</param>
        /// <param name="timeoutSeconds">Timeout in seconds to wait for SolidWorks to start.</param>
        /// <param name="windowStyle">Window style for the new process.</param>
        /// <param name="modifyAllVersions">If true, modifies registry for all installed SolidWorks versions.
        /// If false, only modifies the target version.</param>
        /// <param name="targetVersion">Specific version to target (e.g., "SOLIDWORKS 2024").
        /// If null and modifyAllVersions is false, attempts to detect from executable path.</param>
        /// <returns>Information about the started SolidWorks instance.</returns>
        /// <exception cref="TimeoutException">Thrown when SolidWorks doesn't start within the timeout period.</exception>
        /// <exception cref="InvalidOperationException">Thrown when unable to find or modify registry settings.</exception>
        public SolidWorksInstanceInfo StartInstance(
            string executablePath,
            int timeoutSeconds,
            ProcessWindowStyle windowStyle,
            bool modifyAllVersions,
            string targetVersion)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(MultiInstanceSolidworksManager));

            executablePath = executablePath ?? GetDefaultSolidWorksPath();

            if (string.IsNullOrEmpty(executablePath) || !File.Exists(executablePath))
                throw new FileNotFoundException("SolidWorks executable not found.", executablePath ?? "null");

            // Get all installed versions
            var installedVersions = GetInstalledVersions();
            if (installedVersions.Count == 0)
                throw new InvalidOperationException("No SolidWorks installations found in registry.");

            // Determine which versions to modify
            List<string> versionsToModify;
            if (modifyAllVersions)
            {
                versionsToModify = installedVersions;
            }
            else
            {
                // Try to determine version from path or use provided target
                string detectedVersion = targetVersion ?? GetSolidWorksVersionFromPath(executablePath);
                if (string.IsNullOrEmpty(detectedVersion))
                    detectedVersion = GetLatestInstalledVersion();

                if (string.IsNullOrEmpty(detectedVersion))
                    throw new InvalidOperationException("Could not determine SolidWorks version.");

                versionsToModify = new List<string> { detectedVersion };
            }

            lock (_lockObject)
            {
                // Generate unique journal folder for this instance
                string uniqueJournalFolder = CreateUniqueJournalFolder();

                // Save original journal folder values for all versions we're modifying
                var originalValues = new Dictionary<string, string>();
                foreach (var version in versionsToModify)
                {
                    string keyPath = GetExtReferencesKeyPath(version);
                    originalValues[keyPath] = GetRegistryValue(keyPath, JournalFoldersValueName);
                }

                // Set unique journal folder in registry for all versions
                foreach (var version in versionsToModify)
                {
                    string keyPath = GetExtReferencesKeyPath(version);
                    SetRegistryValue(keyPath, JournalFoldersValueName, uniqueJournalFolder);
                }

                // Add a small delay to ensure registry is flushed
                Thread.Sleep(500);

                try
                {
                    // Start SolidWorks process with /b argument for background/silent mode
                    var startInfo = new ProcessStartInfo(executablePath)
                    {
                        Arguments = "/b",
                        WindowStyle = windowStyle,
                        UseShellExecute = true
                    };

                    var process = Process.Start(startInfo);

                    if (process == null)
                        throw new InvalidOperationException("Failed to start SolidWorks process");

                    // Wait for SolidWorks to initialize and register in ROT
                    ISldWorks app = WaitForSolidWorksInstance(process.Id, timeoutSeconds);

                    // Detect actual version that started (from the running instance)
                    string actualVersion = GetVersionFromRunningInstance(app) ?? versionsToModify.First();

                    // Freeze graphics to prevent UI updates and improve performance
                    try
                    {
                        app.UserControl = false;
                        app.UserControlBackground = false;
                        app.Visible = false;
                        var frame = (Frame)app.Frame();
                        if (frame != null)
                        {
                            frame.KeepInvisible = true;
                        }
                    }
                    catch
                    {
                        // Ignore errors freezing graphics
                    }

                    var instanceInfo = new SolidWorksInstanceInfo(
                        app,
                        process,
                        uniqueJournalFolder,
                        originalValues,
                        actualVersion);

                    _instances.Add(instanceInfo);

                    return instanceInfo;
                }
                catch
                {
                    // Restore original journal folder values on failure
                    foreach (var kvp in originalValues)
                    {
                        RestoreRegistryValue(kvp.Key, JournalFoldersValueName, kvp.Value);
                    }

                    // Clean up the temporary journal folder
                    TryDeleteDirectory(uniqueJournalFolder);

                    throw;
                }
            }
        }

        /// <summary>
        /// Starts a new SolidWorks instance for a specific version year (e.g., 2023, 2024, 2025).
        /// </summary>
        /// <param name="year">The SolidWorks version year (e.g., 2023, 2024, 2025).</param>
        /// <param name="timeoutSeconds">Timeout in seconds to wait for SolidWorks to start.</param>
        /// <param name="windowStyle">Window style for the new process.</param>
        /// <param name="modifyAllVersions">If true, modifies registry for all installed versions to prevent conflicts.</param>
        /// <returns>Information about the started SolidWorks instance.</returns>
        public SolidWorksInstanceInfo StartInstanceByYear(
            int year,
            int timeoutSeconds = 60,
            ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal,
            bool modifyAllVersions = true)
        {
            string targetVersion = $"SOLIDWORKS {year}";
            string executablePath = GetExecutablePathForVersion(targetVersion);

            if (string.IsNullOrEmpty(executablePath))
                throw new InvalidOperationException($"SolidWorks {year} installation not found.");

            return StartInstance(executablePath, timeoutSeconds, windowStyle, modifyAllVersions, targetVersion);
        }

        /// <summary>
        /// Starts multiple SolidWorks instances sequentially.
        /// </summary>
        /// <param name="count">Number of instances to start.</param>
        /// <param name="executablePath">Path to SLDWORKS.exe. If null, uses default installation path.</param>
        /// <param name="timeoutSecondsPerInstance">Timeout in seconds for each instance to start.</param>
        /// <param name="delayBetweenStartsMs">Delay in milliseconds between starting each instance.</param>
        /// <param name="windowStyle">Window style for the new processes.</param>
        /// <returns>List of information about the started SolidWorks instances.</returns>
        public List<SolidWorksInstanceInfo> StartInstances(
            int count,
            string executablePath = null,
            int timeoutSecondsPerInstance = 60,
            int delayBetweenStartsMs = 2000,
            ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than 0.");

            var results = new List<SolidWorksInstanceInfo>();

            for (int i = 0; i < count; i++)
            {
                var instance = StartInstance(executablePath, timeoutSecondsPerInstance, windowStyle);
                results.Add(instance);

                // Wait between starts to let SolidWorks fully initialize
                if (i < count - 1 && delayBetweenStartsMs > 0)
                {
                    Thread.Sleep(delayBetweenStartsMs);
                }
            }

            return results;
        }

        /// <summary>
        /// Closes a specific SolidWorks instance and restores its registry settings.
        /// </summary>
        /// <param name="instance">The instance to close.</param>
        /// <param name="forceKill">If true, forcefully kills the process if it doesn't close gracefully.</param>
        public void CloseInstance(SolidWorksInstanceInfo instance, bool forceKill = false)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            lock (_lockObject)
            {
                try
                {
                    // Try to close SolidWorks gracefully
                    if (instance.Application != null)
                    {
                        try
                        {
                            instance.Application.ExitApp();
                        }
                        catch (COMException)
                        {
                            // SolidWorks may have already closed
                        }
                    }

                    // Wait for process to exit or force kill
                    if (instance.Process != null && !instance.Process.HasExited)
                    {
                        if (!instance.Process.WaitForExit(10000) && forceKill)
                        {
                            instance.Process.Kill();
                        }
                    }
                }
                finally
                {
                    // Restore original registry values for all modified versions
                    foreach (var kvp in instance.OriginalRegistryValues)
                    {
                        RestoreRegistryValue(kvp.Key, JournalFoldersValueName, kvp.Value);
                    }

                    // Clean up temporary journal folder
                    TryDeleteDirectory(instance.JournalFolder);

                    _instances.Remove(instance);
                }
            }
        }

        /// <summary>
        /// Closes all managed SolidWorks instances and restores registry settings.
        /// </summary>
        /// <param name="forceKill">If true, forcefully kills processes that don't close gracefully.</param>
        public void CloseAllInstances(bool forceKill = false)
        {
            var instancesToClose = _instances.ToList();
            foreach (var instance in instancesToClose)
            {
                CloseInstance(instance, forceKill);
            }
        }

        /// <summary>
        /// Gets the default SolidWorks installation path (latest version).
        /// </summary>
        /// <returns>Path to SLDWORKS.exe or null if not found.</returns>
        public static string GetDefaultSolidWorksPath()
        {
            // Try to find from registry (latest version)
            string latestVersion = GetLatestInstalledVersion();
            if (!string.IsNullOrEmpty(latestVersion))
            {
                string path = GetExecutablePathForVersion(latestVersion);
                if (!string.IsNullOrEmpty(path))
                    return path;
            }

            // Try common installation paths as fallback
            string[] commonPaths = new[]
            {
                @"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\SLDWORKS.exe",
                @"C:\Program Files\SolidWorks Corp\SolidWorks\SLDWORKS.exe",
                @"C:\Program Files (x86)\SOLIDWORKS Corp\SOLIDWORKS\SLDWORKS.exe"
            };

            foreach (var path in commonPaths)
            {
                if (File.Exists(path))
                    return path;
            }

            return null;
        }

        /// <summary>
        /// Gets the executable path for a specific SolidWorks version.
        /// </summary>
        /// <param name="version">Version string (e.g., "SOLIDWORKS 2024").</param>
        /// <returns>Path to SLDWORKS.exe or null if not found.</returns>
        public static string GetExecutablePathForVersion(string version)
        {
            using (var key = Registry.CurrentUser.OpenSubKey($@"{SolidWorksRegistryPath}\{version}\Setup"))
            {
                if (key != null)
                {
                    string installPath = key.GetValue("SolidWorks Installation Directory") as string;
                    if (!string.IsNullOrEmpty(installPath))
                    {
                        string exePath = Path.Combine(installPath, "SLDWORKS.exe");
                        if (File.Exists(exePath))
                            return exePath;
                    }
                }
            }

            // Try LocalMachine as fallback
            using (var key = Registry.LocalMachine.OpenSubKey($@"SOFTWARE\SolidWorks\{version}\Setup"))
            {
                if (key != null)
                {
                    string installPath = key.GetValue("SolidWorks Installation Directory") as string;
                    if (!string.IsNullOrEmpty(installPath))
                    {
                        string exePath = Path.Combine(installPath, "SLDWORKS.exe");
                        if (File.Exists(exePath))
                            return exePath;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the executable path for a specific SolidWorks version year.
        /// </summary>
        /// <param name="year">Version year (e.g., 2023, 2024, 2025).</param>
        /// <returns>Path to SLDWORKS.exe or null if not found.</returns>
        public static string GetExecutablePathForYear(int year)
        {
            return GetExecutablePathForVersion($"SOLIDWORKS {year}");
        }

        /// <summary>
        /// Gets the latest installed SolidWorks version from the registry.
        /// </summary>
        /// <returns>Version string (e.g., "SOLIDWORKS 2024") or null if not found.</returns>
        public static string GetLatestInstalledVersion()
        {
            var versions = GetInstalledVersions();
            return versions.FirstOrDefault();
        }

        /// <summary>
        /// Gets all installed SolidWorks versions from the registry, ordered by version descending.
        /// </summary>
        /// <returns>List of version strings (e.g., ["SOLIDWORKS 2025", "SOLIDWORKS 2024", "SOLIDWORKS 2023"]).</returns>
        public static List<string> GetInstalledVersions()
        {
            var versions = new List<string>();

            using (var swKey = Registry.CurrentUser.OpenSubKey(SolidWorksRegistryPath))
            {
                if (swKey == null)
                    return versions;

                versions = swKey.GetSubKeyNames()
                    .Where(name => name.StartsWith("SOLIDWORKS", StringComparison.OrdinalIgnoreCase)
                                   && char.IsDigit(name.Last())) // Filter out non-version keys
                    .OrderByDescending(name => ExtractYearFromVersion(name))
                    .ToList();
            }

            return versions;
        }

        /// <summary>
        /// Gets the installation information for all installed SolidWorks versions.
        /// </summary>
        /// <returns>Dictionary mapping version strings to executable paths.</returns>
        public static Dictionary<string, string> GetAllInstallations()
        {
            var installations = new Dictionary<string, string>();

            foreach (var version in GetInstalledVersions())
            {
                string exePath = GetExecutablePathForVersion(version);
                if (!string.IsNullOrEmpty(exePath))
                {
                    installations[version] = exePath;
                }
            }

            return installations;
        }

        private static int ExtractYearFromVersion(string version)
        {
            // Extract year from version string like "SOLIDWORKS 2024"
            var parts = version.Split(' ');
            if (parts.Length >= 2 && int.TryParse(parts.Last(), out int year))
                return year;
            return 0;
        }

        private static string GetExtReferencesKeyPath(string version)
        {
            return $@"{SolidWorksRegistryPath}\{version}\{ExtReferencesSubKey}";
        }

        private static string GetSolidWorksVersionFromPath(string executablePath)
        {
            if (string.IsNullOrEmpty(executablePath))
                return null;

            var directoryName = Path.GetDirectoryName(executablePath);
            if (string.IsNullOrEmpty(directoryName))
                return null;

            // Try to find year in path (e.g., "2024" in the path)
            var installedVersions = GetInstalledVersions();
            foreach (var version in installedVersions)
            {
                int year = ExtractYearFromVersion(version);
                if (year > 0 && directoryName.Contains(year.ToString()))
                    return version;
            }

            // Check each version's install path against the provided path
            foreach (var version in installedVersions)
            {
                string versionPath = GetExecutablePathForVersion(version);
                if (!string.IsNullOrEmpty(versionPath) &&
                    string.Equals(Path.GetFullPath(versionPath), Path.GetFullPath(executablePath), StringComparison.OrdinalIgnoreCase))
                {
                    return version;
                }
            }

            return null;
        }

        private static string GetVersionFromRunningInstance(ISldWorks app)
        {
            try
            {
                // ISldWorks.RevisionNumber returns something like "33.0" for SW 2025
                // SW 2023 = 31.x, SW 2024 = 32.x, SW 2025 = 33.x
                string revision = app.RevisionNumber();
                if (!string.IsNullOrEmpty(revision))
                {
                    var parts = revision.Split('.');
                    if (parts.Length > 0 && int.TryParse(parts[0], out int majorVersion))
                    {
                        // Major version 28 = SW 2020, each year increments by 1
                        int year = 1992 + majorVersion; // 28 + 1992 = 2020
                        return $"SOLIDWORKS {year}";
                    }
                }
            }
            catch
            {
                // Ignore errors reading version
            }
            return null;
        }

        private static string CreateUniqueJournalFolder()
        {
            string tempPath = Path.Combine(
                Path.GetTempPath(),
                "SolidWorksJournals",
                $"Instance_{Guid.NewGuid():N}");

            Directory.CreateDirectory(tempPath);
            return tempPath;
        }

        private static string GetRegistryValue(string keyPath, string valueName)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(keyPath))
            {
                return key?.GetValue(valueName) as string;
            }
        }

        private static void SetRegistryValue(string keyPath, string valueName, string value)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(keyPath, writable: true))
            {
                if (key == null)
                {
                    // Create the key if it doesn't exist
                    using (var newKey = Registry.CurrentUser.CreateSubKey(keyPath))
                    {
                        newKey?.SetValue(valueName, value ?? string.Empty, RegistryValueKind.String);
                    }
                }
                else
                {
                    key.SetValue(valueName, value ?? string.Empty, RegistryValueKind.String);
                }
            }
        }

        private static void RestoreRegistryValue(string keyPath, string valueName, string originalValue)
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(keyPath, writable: true))
                {
                    if (key != null)
                    {
                        if (originalValue == null)
                        {
                            key.DeleteValue(valueName, throwOnMissingValue: false);
                        }
                        else
                        {
                            key.SetValue(valueName, originalValue, RegistryValueKind.String);
                        }
                    }
                }
            }
            catch
            {
                // Best effort restoration - don't throw
            }
        }

        private static void TryDeleteDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, recursive: true);
                }
            }
            catch
            {
                // Best effort cleanup
            }
        }

        [DllImport("ole32.dll")]
        private static extern int CreateBindCtx(uint reserved, out IBindCtx ppbc);

        private static ISldWorks WaitForSolidWorksInstance(int processId, int timeoutSeconds)
        {
            var monikerName = "SolidWorks_PID_" + processId.ToString();
            var timeout = TimeSpan.FromSeconds(timeoutSeconds);
            var startTime = DateTime.Now;

            while (DateTime.Now - startTime < timeout)
            {
                var app = GetSwAppFromProcess(processId, monikerName);
                if (app != null)
                    return app;

                Thread.Sleep(500);
            }

            throw new TimeoutException($"SolidWorks did not start within {timeoutSeconds} seconds.");
        }

        private static ISldWorks GetSwAppFromProcess(int processId, string monikerName)
        {
            IBindCtx context = null;
            IRunningObjectTable rot = null;
            IEnumMoniker monikers = null;

            try
            {
                CreateBindCtx(0, out context);

                context.GetRunningObjectTable(out rot);
                rot.EnumRunning(out monikers);

                var moniker = new IMoniker[1];

                while (monikers.Next(1, moniker, IntPtr.Zero) == 0)
                {
                    var curMoniker = moniker.FirstOrDefault();

                    string name = null;

                    if (curMoniker != null)
                    {
                        try
                        {
                            curMoniker.GetDisplayName(context, null, out name);
                        }
                        catch (UnauthorizedAccessException)
                        {
                        }
                    }

                    if (string.Equals(monikerName, name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        rot.GetObject(curMoniker, out object app);
                        return app as ISldWorks;
                    }
                }
            }
            finally
            {
                if (monikers != null)
                    Marshal.ReleaseComObject(monikers);

                if (rot != null)
                    Marshal.ReleaseComObject(rot);

                if (context != null)
                    Marshal.ReleaseComObject(context);
            }

            return null;
        }

        /// <summary>
        /// Releases all resources and closes all managed instances.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases resources.
        /// </summary>
        /// <param name="disposing">True if called from Dispose(), false if from finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                CloseAllInstances(forceKill: true);
            }

            _disposed = true;
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~MultiInstanceSolidworksManager()
        {
            Dispose(false);
        }
    }

    /// <summary>
    /// Contains information about a started SolidWorks instance.
    /// </summary>
    [Obsolete("This class is part of MultiInstanceSolidworksManager which is not practical for production use.")]
    public class SolidWorksInstanceInfo
    {
        /// <summary>
        /// Gets the SolidWorks application interface.
        /// </summary>
        public ISldWorks Application { get; }

        /// <summary>
        /// Gets the Windows process for this SolidWorks instance.
        /// </summary>
        public Process Process { get; }

        /// <summary>
        /// Gets the unique journal folder path used by this instance.
        /// </summary>
        public string JournalFolder { get; }

        /// <summary>
        /// Gets the SolidWorks version string (e.g., "SOLIDWORKS 2024").
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Gets the original registry values for all modified versions (registry key path -> original value).
        /// </summary>
        internal Dictionary<string, string> OriginalRegistryValues { get; }

        internal SolidWorksInstanceInfo(
            ISldWorks application,
            Process process,
            string journalFolder,
            Dictionary<string, string> originalRegistryValues,
            string version)
        {
            Application = application;
            Process = process;
            JournalFolder = journalFolder;
            OriginalRegistryValues = originalRegistryValues;
            Version = version;
        }
    }
}
