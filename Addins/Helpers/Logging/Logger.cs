using System;
using System.Diagnostics;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// logs to Event Viewer
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Source of this logger in Event Viewer
        /// </summary>
        public static string Source { set; get; }

        /// <summary>
        /// registers this source in Event Viewer
        /// </summary>
        /// <param name="source"></param>
        public static void RegisterLogger(string source)
        {
            // Create the source, if it does not already exist.
            if (!EventLog.SourceExists(source))
            {
                //An event log source should not be created and immediately used.
                //There is a latency time to enable the source, it should be created
                //prior to executing the application that uses the source.
                EventLog.CreateEventSource(source, "Application");
                // once The source is created.  Exit the application to allow it to be registered.
                return;
            }
        }

        /// <summary>
        /// deletes a source from Event Viewer
        /// </summary>
        /// <param name="source"></param>
        public static void UnRegisterLogger(string source)
        {
            if (EventLog.SourceExists(source))
                EventLog.DeleteEventSource(source);
        }

        /// <summary>
        /// logs a message to Event Viewer
        /// </summary>
        /// <param name="message">message to log <br/>
        /// use keywords 'Warning' or 'Error' to define warning and error log types other wise it will be of type information</param>
        public static void Log(string message)
        {
            try
            {
                if (message.StartsWith("Error!", System.StringComparison.OrdinalIgnoreCase))
                {
                    EventLog.WriteEntry(Source, message, EventLogEntryType.Error);
                }
                else if (message.StartsWith("Warning!", System.StringComparison.OrdinalIgnoreCase))
                {
                    EventLog.WriteEntry(Source, message, EventLogEntryType.Warning);
                }
                else
                {
                    EventLog.WriteEntry(Source, message);
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// logs an exception as Error
        /// </summary>
        /// <param name="exception"></param>
        public static void Log(Exception exception)
        {
            Log($"Error! {exception.Message}");
        }
    }
}
