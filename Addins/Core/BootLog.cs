// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
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
    /// Output goes to <c>%LOCALAPPDATA%\Hymma.Solidworks\ConnectLogs\connect-yyyyMMdd.log</c>.
    /// </para>
    /// </remarks>
    internal static class BootLog
    {
        private static readonly object _gate = new object();
        private static string _path;

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

        /// <summary>Records an informational lifecycle message.</summary>
        public static void Info(string message) => Write("INF", message, null);

        /// <summary>Records a failure with its full exception detail.</summary>
        public static void Error(string message, Exception ex) => Write("ERR", message, ex);

        private static void Write(string level, string message, Exception ex)
        {
            try
            {
                if (_path == null)
                    Init();
                if (_path == null)
                    return;

                var sb = new StringBuilder();
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
                  .Append(" [").Append(level).Append("] ")
                  .Append(message);
                if (ex != null)
                    sb.AppendLine().Append(ex);

                lock (_gate)
                    File.AppendAllText(_path, sb.ToString() + Environment.NewLine);
            }
            catch
            {
                // Never throw from the logger.
            }
        }
    }
}
