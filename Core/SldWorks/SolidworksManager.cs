using SolidWorks.Interop.sldworks;
using System;

namespace Hymma.SolidTools.Core
{
    public class SolidWorksManager
    {
        private readonly static object _lock = new object();
        private static SolidWorksManager _Instance;
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
