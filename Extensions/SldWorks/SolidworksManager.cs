//**********************
//Copyright(C) 2024 Xarial Pty Limited
//Reference: https://www.codestack.net/solidworks-api/getting-started/stand-alone/connect-csharp/
//License: https://www.codestack.net/license/
//**********************

using SolidWorks.Interop.sldworks;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// Handles <see cref="ISldWorks"/>  
    /// </summary>
    public static class SolidworksManager
    {
        /// <summary>
        /// starts a new SOLIDWORSK instance and returns the <see cref="ISldWorks"/> 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="timeoutSec"></param>
        /// <returns>A new instance of <see cref="ISldWorks"/></returns>
        /// <exception cref="TimeoutException"></exception>
        public static ISldWorks StartNewSolidworksApp(ProcessStartInfo info, int timeoutSec = 10)
        {
            if (string.IsNullOrEmpty(info.FileName))
                info.FileName = @"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\SLDWORKS.exe";

            var timeout = TimeSpan.FromSeconds(timeoutSec);

            var startTime = DateTime.Now;

            if (info.WindowStyle == ProcessWindowStyle.Hidden)
                info.UseShellExecute = true;

            var prc = Process.Start(info);
            ISldWorks app = null;

            while (app == null)
            {
                if (DateTime.Now - startTime > timeout)
                {
                    throw new TimeoutException();
                }

                app = GetSwAppFromProcess(prc.Id);
            }

            return app;

        }

        /// <summary>
        /// starts a new SOLIDWORSK instance and returns the <see cref="ISldWorks"/> 
        /// </summary>
        /// <param name="executablePath">Its best to ask users to explicitly browse to the location of solidworks install. 
        /// However the default install path is the value of this parameter. When calling from your addins, use extension <code>ISldWorks.GetExecutableFullFileName()  </code></param>
        /// <param name="timeoutSec"></param>
        /// <returns>A new instance of <see cref="ISldWorks"/></returns>
        /// <exception cref="TimeoutException"></exception>
        public static ISldWorks StartNewSolidworksApp(string executablePath = @"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\SLDWORKS.exe",
                                                      int timeoutSec = 10)
        {
            return StartNewSolidworksApp(new ProcessStartInfo(executablePath), timeoutSec);
        }


        [DllImport("ole32.dll")]
        static extern int CreateBindCtx(uint reserved, out IBindCtx ppbc);

        static ISldWorks GetSwAppFromProcess(int processId)
        {
            var monikerName = "SolidWorks_PID_" + processId.ToString();

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
                    var curMoniker = moniker.First();

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

                    if (string.Equals(monikerName,
                        name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        object app;
                        rot.GetObject(curMoniker, out app);
                        return app as ISldWorks;
                    }
                }
            }
            finally
            {
                if (monikers != null)
                {
                    Marshal.ReleaseComObject(monikers);
                }

                if (rot != null)
                {
                    Marshal.ReleaseComObject(rot);
                }

                if (context != null)
                {
                    Marshal.ReleaseComObject(context);
                }
            }

            return null;
        }
    }
}
