#define TRACE
// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace QRify.Logging
{
    /// <summary>
    /// Singleton class that logs to EventLog in windows
    /// </summary>
    public sealed class QRifyLogger
    {
        private static string logSource = "Qrify Addin";
        private static EventLogTraceListener traceListener;

        public QRifyLogger()
        {
            traceListener = new EventLogTraceListener();
            if (!Trace.Listeners.Contains(traceListener))
                Trace.Listeners.Add(traceListener);
        }

        /// <summary>
        /// uses <see cref="System.Diagnostics.Trace"/> and writes to EventLog
        /// </summary>
        /// <param name="message"></param>
        public void TraceLog(string message)
        {
            if (EventLog.SourceExists(logSource))
            {
                traceListener.WriteLine(message);
            }
        }

        /// <summary>
        /// logs a message 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        public static void Log(string msg, EventLogEntryType type)
        {
            try
            {
                if (!EventLog.SourceExists(logSource))
                    return;
                EventLog.WriteEntry(logSource, msg, type);//also disposes the object
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        ///     An error event. This indicates a significant problem the user should know about;
        ///     usually a loss of functionality or data.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="origin"></param>
        /// <param name="line"></param>
        /// <param name="member"></param>
        public void Error(Exception exception,
            [CallerFilePath] string origin = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            Log(GetFormattedLog(line, member, origin, exception.Message, EventLogEntryType.Error), EventLogEntryType.Error);
        }
        /// <summary>
        ///     A warning event. This indicates a problem that is not immediately significant,
        ///     but that may signify conditions that could cause future problems.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="origin"></param>
        /// <param name="line"></param>
        /// <param name="member"></param>
        public void Warning(string msg,
            [CallerFilePath] string origin = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            Log(GetFormattedLog(line, member, origin, msg, EventLogEntryType.Warning), EventLogEntryType.Warning);
        }
        /// <summary>
        ///     An information event. This indicates a significant, successful operation.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="origin"></param>
        /// <param name="line"></param>
        /// <param name="member"></param>
        public void Info(string msg,
            [CallerFilePath] string origin = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            Log(GetFormattedLog(line, member, origin, msg, EventLogEntryType.Information), EventLogEntryType.Information);
        }
        /// <summary>
        ///     A success audit event. This indicates a security event that occurs when an audited
        ///     access attempt is successful; for example, logging on successfully.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="origin"></param>
        /// <param name="line"></param>
        /// <param name="member"></param>
        public void SuccessAudit(string msg,
            [CallerFilePath] string origin = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            Log(GetFormattedLog(line, member, origin, msg, EventLogEntryType.SuccessAudit), EventLogEntryType.SuccessAudit);
        }
        /// <summary>
        ///     A failure audit event. This indicates a security event that occurs when an audited
        ///     access attempt fails; for example, a failed attempt to open a file.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="origin"></param>
        /// <param name="line"></param>
        /// <param name="member"></param>
        public void FailureAudit(string msg,
            [CallerFilePath] string origin = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            Log(GetFormattedLog(line, member, origin, msg, EventLogEntryType.FailureAudit), EventLogEntryType.FailureAudit);
        }


        private string GetFormattedLog(int line, string member, string file, string msg, EventLogEntryType type)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} ===> [time:{1}] [line: {2}] [member: {3}] [file: {4}] [msg: {5}] {6}", type, DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"), line, member, file, msg, Environment.NewLine);
            return sb.ToString();
        }
    }
}
