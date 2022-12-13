// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using System;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// provides connection and initiation methods for a solidworks app
    /// </summary>
    public class SolidWorksManager
    {
        private readonly static object _lock = new object();
        private static SolidWorksManager _Instance;
        
        /// <summary>
        /// creates or retrieves an instance of <see cref="SolidWorksManager"/>
        /// </summary>
        /// <returns></returns>
        public static SolidWorksManager InitiateSolidApp()
        {
            lock (_lock)
            {
                if (_Instance == null)
                {
                    _Instance = new SolidWorksManager();
                }
                return _Instance;
            }
        }

        /// <summary>
        /// connects to a running instance of solidworks or creates a new one and returns the <see cref="SldWorks"/> object
        /// </summary>
        /// <returns>a running instance of solidworks <see cref="SldWorks"/></returns>
        /// <remarks>this function uses <see cref="System.Runtime.InteropServices.Marshal"/> which is <strong>not supported in .net core </strong></remarks>
        public static SldWorks GetSolidworks()
        {
            try
            {
                try
                {
                    return System.Runtime.InteropServices.Marshal.GetActiveObject("SldWorks.Application") as SldWorks;
                }
                catch
                {
                    Type solidworksAppType = System.Type.GetTypeFromProgID("SldWorks.Application");
                    var app = System.Activator.CreateInstance(solidworksAppType) as SldWorks;
                    //Must be set visible explicitly
                    app.Visible = true;
                    return app;
                }
            }
            catch (Exception)
            {
                throw new MemberAccessException("Could not get an instance of Solidworks");
            }
        }
    }
}
