// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license
//
// EXAMPLE FILE - This file demonstrates how to use MultiInstanceSolidworksManager
// This file is not compiled into the library (not included in csproj)

#if false // This prevents compilation - remove to test

using System;
using System.Diagnostics;

namespace Hymma.Solidworks.Extensions.Examples
{
    /// <summary>
    /// Examples demonstrating how to use MultiInstanceSolidworksManager
    /// to start multiple SolidWorks instances without journal file conflicts.
    /// </summary>
    public class MultiInstanceExamples
    {
        /// <summary>
        /// Basic example: Start multiple instances of the latest SolidWorks version.
        /// </summary>
        public void StartMultipleInstances()
        {
            // The manager implements IDisposable - when disposed, all instances
            // are closed and registry settings are restored
            using (var manager = new MultiInstanceSolidworksManager())
            {
                // Start 3 instances of the latest installed SolidWorks
                // By default, modifies registry for ALL installed versions (2023, 2024, 2025, etc.)
                var instances = manager.StartInstances(
                    count: 3,
                    executablePath: null,           // null = use latest version
                    timeoutSecondsPerInstance: 60,  // Wait up to 60 seconds per instance
                    delayBetweenStartsMs: 2000,     // 2 second delay between starts
                    windowStyle: ProcessWindowStyle.Normal
                );

                Console.WriteLine($"Started {instances.Count} SolidWorks instances:");
                foreach (var instance in instances)
                {
                    Console.WriteLine($"  - {instance.Version} (PID: {instance.Process.Id})");

                    // Access the SolidWorks API
                    var app = instance.Application;
                    Console.WriteLine($"    Revision: {app.RevisionNumber()}");
                }

                // Do your work with the instances here...
                // For example, open different files in each instance

                // When 'using' block ends, all instances are closed
                // and registry is restored to original values
            }
        }

        /// <summary>
        /// Start a specific SolidWorks version by year.
        /// </summary>
        public void StartSpecificVersion()
        {
            using (var manager = new MultiInstanceSolidworksManager())
            {
                // Start SolidWorks 2024 specifically
                var sw2024 = manager.StartInstanceByYear(
                    year: 2024,
                    timeoutSeconds: 60,
                    windowStyle: ProcessWindowStyle.Normal,
                    modifyAllVersions: true  // Still modify all versions to prevent conflicts
                );

                Console.WriteLine($"Started {sw2024.Version}");
                Console.WriteLine($"Process ID: {sw2024.Process.Id}");
                Console.WriteLine($"Journal Folder: {sw2024.JournalFolder}");

                // Work with SolidWorks 2024...
                var app = sw2024.Application;

                // Close this specific instance before Dispose if needed
                manager.CloseInstance(sw2024, forceKill: false);
            }
        }

        /// <summary>
        /// Start instances of different SolidWorks versions simultaneously.
        /// </summary>
        public void StartDifferentVersions()
        {
            using (var manager = new MultiInstanceSolidworksManager())
            {
                // Start one instance of each installed version
                var sw2023 = manager.StartInstanceByYear(2023);
                var sw2024 = manager.StartInstanceByYear(2024);
                var sw2025 = manager.StartInstanceByYear(2025);

                Console.WriteLine("Started instances:");
                Console.WriteLine($"  2023: PID {sw2023.Process.Id}");
                Console.WriteLine($"  2024: PID {sw2024.Process.Id}");
                Console.WriteLine($"  2025: PID {sw2025.Process.Id}");

                // Each instance has its own unique journal folder,
                // so no conflicts occur
            }
        }

        /// <summary>
        /// Query installed SolidWorks versions without starting any instances.
        /// </summary>
        public void ListInstalledVersions()
        {
            // Static methods - no need to create manager instance

            // Get all installed versions (ordered by year, descending)
            var versions = MultiInstanceSolidworksManager.GetInstalledVersions();
            Console.WriteLine("Installed SolidWorks versions:");
            foreach (var version in versions)
            {
                Console.WriteLine($"  - {version}");
            }

            // Get latest version
            var latest = MultiInstanceSolidworksManager.GetLatestInstalledVersion();
            Console.WriteLine($"\nLatest: {latest}");

            // Get executable paths for all versions
            var installations = MultiInstanceSolidworksManager.GetAllInstallations();
            Console.WriteLine("\nInstallation paths:");
            foreach (var kvp in installations)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }

            // Get path for specific year
            var path2024 = MultiInstanceSolidworksManager.GetExecutablePathForYear(2024);
            Console.WriteLine($"\nSolidWorks 2024 path: {path2024}");
        }

        /// <summary>
        /// Start instance with hidden window (background processing).
        /// </summary>
        public void StartHiddenInstance()
        {
            using (var manager = new MultiInstanceSolidworksManager())
            {
                var instance = manager.StartInstance(
                    executablePath: null,
                    timeoutSeconds: 60,
                    windowStyle: ProcessWindowStyle.Hidden  // Start hidden
                );

                var app = instance.Application;

                // Use FreezeGraphics for truly invisible operation
                // (from SldWorksExtensions)
                // app.FreezeGraphics();

                // Do background processing...
                // Open files, export, etc.

                // app.UnFreezeGraphics();
            }
        }

        /// <summary>
        /// Manually control instance lifecycle without using 'using' block.
        /// </summary>
        public void ManualLifecycleControl()
        {
            var manager = new MultiInstanceSolidworksManager();

            try
            {
                var instance1 = manager.StartInstance();
                var instance2 = manager.StartInstance();

                // Do work...

                // Close instances individually
                manager.CloseInstance(instance1, forceKill: false);

                // instance2 is still running...

                // Force close remaining instances
                manager.CloseAllInstances(forceKill: true);
            }
            finally
            {
                // Always dispose to ensure cleanup
                manager.Dispose();
            }
        }

        /// <summary>
        /// Only modify registry for the specific version being started.
        /// Use this if you're certain which version will be launched.
        /// </summary>
        public void ModifyOnlyTargetVersion()
        {
            using (var manager = new MultiInstanceSolidworksManager())
            {
                // Only modify registry for SolidWorks 2024
                // (slightly faster, but may cause conflicts if another version starts)
                var instance = manager.StartInstance(
                    executablePath: MultiInstanceSolidworksManager.GetExecutablePathForYear(2024),
                    timeoutSeconds: 60,
                    windowStyle: ProcessWindowStyle.Normal,
                    modifyAllVersions: false,       // Only modify target version
                    targetVersion: "SOLIDWORKS 2024"
                );

                // Work with instance...
            }
        }
    }
}

#endif
