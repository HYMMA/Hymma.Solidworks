// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Hymma.Solidworks.Addins.Core
{
    /// <summary>
    /// Framework-owned bootstrap logger for the add-in connect/disconnect lifecycle.
    /// </summary>
    /// <remarks>
    /// <para>
    /// It is initialised at the very start of <see cref="AddinMaker.ConnectToSW"/>, BEFORE any
    /// consumer code runs, so a failure during connect is always recorded even when the consumer's
    /// own logging never gets a chance to initialise. This exists because an exception thrown out
    /// of <c>ConnectToSW</c> crosses the COM boundary into native SOLIDWORKS, which swallows it and
    /// silently unchecks the add-in &#8212; no dialog, no consumer log.
    /// </para>
    /// <para>
    /// Every method is exception-safe: logging must never be the reason an add-in fails to load.
    /// Output always goes to <c>%LOCALAPPDATA%\Hymma.Solidworks\ConnectLogs\connect-yyyyMMdd.log</c>.
    /// A consumer can additionally route <see cref="Error"/> entries to the Windows Event Log via
    /// <see cref="ConfigureEventLog"/> (call it from the add-in constructor, BEFORE ConnectToSW) so
    /// connect failures reach an existing Event-Log-based telemetry pipeline. The framework never
    /// creates the event source — the consumer's installer must register it.
    /// </para>
    /// </remarks>
    internal static class BootLog
    {
        private static readonly object _gate = new object();
        private static string _path;
        private static string _eventSource;
        private static string _eventLogName;

        /// <summary>
        /// Full path of the current log file, or a placeholder if it could not be initialised.
        /// </summary>
        public static string LogPath => _path ?? "(connect log not initialised)";

        /// <summary>
        /// Resolves the log file path and ensures its directory exists. Safe to call repeatedly
        /// and safe to call concurrently. Never throws.
        /// </summary>
        public static void Init()
        {
            try
            {
                if (_path != null)
                    return;

                lock (_gate)
                {
                    if (_path != null)
                        return;

                    var dir = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Hymma.Solidworks", "ConnectLogs");
                    Directory.CreateDirectory(dir);
                    _path = Path.Combine(dir, "connect-" + DateTime.Now.ToString("yyyyMMdd") + ".log");
                }
            }
            catch
            {
                // Never throw from the logger.
            }
        }

        /// <summary>
        /// Routes <see cref="Error"/> entries to the Windows Event Log under <paramref name="source"/>
        /// in <paramref name="logName"/> (e.g. "Application"), IN ADDITION to the file. The source must
        /// already be registered (typically by the consumer's installer); the framework never creates
        /// it. Call this from the add-in constructor so it is set before <c>ConnectToSW</c> runs. Safe
        /// to call repeatedly; passing a null/empty source disables Event Log routing.
        /// </summary>
        public static void ConfigureEventLog(string source, string logName)
        {
            _eventSource = string.IsNullOrWhiteSpace(source) ? null : source;
            _eventLogName = string.IsNullOrWhiteSpace(logName) ? "Application" : logName;
        }

        /// <summary>Records an informational lifecycle message (file only).</summary>
        public static void Info(string message) => Write("INF", message, null, isError: false);

        /// <summary>
        /// Records a failure with its full exception detail. Written to the file and, when configured
        /// via <see cref="ConfigureEventLog"/>, to the Windows Event Log as an Error entry.
        /// </summary>
        public static void Error(string message, Exception ex) => Write("ERR", message, ex, isError: true);

        private static void Write(string level, string message, Exception ex, bool isError)
        {
            var sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
              .Append(" [").Append(level).Append("] ")
              .Append(message);
            if (ex != null)
                sb.AppendLine().Append(ex);
            string line = sb.ToString();

            // 1) Always append to the file (best-effort).
            try
            {
                if (_path == null)
                    Init();
                if (_path != null)
                    lock (_gate)
                        File.AppendAllText(_path, line + Environment.NewLine);
            }
            catch
            {
                // Never throw from the logger.
            }

            // 2) Forward failures to the consumer's Event Log source so they reach an
            //    Event-Log-based telemetry pipeline. Never creates the source, never throws.
            if (isError && _eventSource != null)
            {
                try
                {
                    EventLog.WriteEntry(_eventSource, line, EventLogEntryType.Error);
                }
                catch
                {
                    // Source not registered / no permission — the file copy still has it.
                }
            }
        }
    }
}
