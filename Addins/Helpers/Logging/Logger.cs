// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Hymma.Solidworks.Addins.Logging
{
    /// <summary>
    /// Singleton class that logs to EventLog in windows
    /// </summary>
    public class Logger
    {
        static readonly object lc = new object();
        static Logger _instance;
        static string _source;

        /// <summary>
        /// Get and instance of logger 
        /// </summary>
        /// <param name="source">source name of the logger</param>
        /// <returns></returns>
        public static Logger GetInstance(string source)
        {
            lock (lc)
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                    _source = source;
                    return _instance;
                }
                return _instance;
            }
        }
        Logger() { }

        /// <summary>
        /// logs a message 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        public void Log(string msg, EventLogEntryType type)
        {
            if (!EventLog.SourceExists(_source))
                return;
            try
            {
                var logger = new EventLog();
                using (logger = new EventLog())
                {
                    logger.Source = _source;
                    logger.WriteEntry(msg, type);
                }
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
