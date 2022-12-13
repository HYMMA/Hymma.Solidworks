// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins.Logging;
using System.Diagnostics;
using System.IO;

namespace Hymma.Solidworks.Addins.Helpers.DotNet
{
    /// <summary>
    /// helper methods for <see cref="EventLog"/>
    /// </summary>
    public class EventLogHelper
    {

        static string localappdata = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);


        /// <summary>
        /// registers and event source into the local computer
        /// </summary>
        /// <param name="source">the source name</param>
        /// <param name="logName">the log name</param>
        /// <param name="backupLogFileName">file to log to in-case this operation did not go through. will be saved in local app data folder</param>
        public static void RegisterEventSource(string source, string logName, string backupLogFileName = "hymma-solidworks-addins.log")
        {
            if (!EventLog.SourceExists(source))
            {
                //log the error message into a file 
                string logFile = Path.Combine(localappdata, backupLogFileName);
                try
                {
                    EventLog.CreateEventSource(source, logName);

                    using (var st = new StreamWriter(logFile, true))
                    {
                        st.WriteLine($"registered {source} with log {logName} into EventLog");
                    }
                }
                catch (System.Exception e)
                {
                    using (var st = new StreamWriter(logFile, true))
                    {
                        st.WriteLine($"Could not register {source} into EventLog \r\n {e.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// deletes and event source from local machine
        /// </summary>
        /// <param name="source">the source name of the event log</param>
        /// <param name="logName">the log name of the event, make sure other sources are not writing to this log</param>
        /// <param name="backupLogFileName">file to log to in-case this operation did not go through. will be saved in local app data folder</param>
        public static void UnRegisterEventSource(string source, string logName, string backupLogFileName = "hymma-solidworks-addins.log")
        {
            if (EventLog.SourceExists(source))
            {
                string logFile = Path.Combine(localappdata, backupLogFileName);
                try
                {
                    // Find the log associated with this source.
                    var logForThisSource = EventLog.LogNameFromSourceName(source, ".");

                    var st = new StreamWriter(logFile, true);
                    // Make sure the source is in the log we believe it to be in.
                    if (logForThisSource != logName)
                    {
                        var logger = Logger.GetInstance(Properties.Resources.LogSource);
                        var msg = $"Did not un-register the source {source} from EventLog as its log was {logForThisSource} not the {logName} log";
                        logger.Warning(msg);
                        using (st)
                        {
                            st.WriteLine(msg);
                        }
                        return;
                    }
                    // Delete the source and the log.
                    EventLog.DeleteEventSource(source);
                    EventLog.Delete(logForThisSource);

                    using (st)
                    {
                        st.WriteLine($"unregistered {source} from EventLog");
                    }
                }
                catch (System.Exception e)
                {
                    using (var st = new StreamWriter(logFile, true))
                    {
                        st.WriteLine($"could not un-register {source} form EventLog \r\n {e.Message}");
                    }
                }
            }
        }
    }
}
