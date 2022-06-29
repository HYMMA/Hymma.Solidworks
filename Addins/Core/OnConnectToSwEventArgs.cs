using SolidWorks.Interop.sldworks;
using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// event arguments for when solidworks connects or disconnects from your add-in
    /// </summary>
    public class OnConnectToSwEventArgs : EventArgs
    {
        /// <summary>
        /// solidworks object
        /// </summary>
        public ISldWorks Solidworks { get; set; }

        /// <summary>
        /// the identifier for this addin
        /// </summary>
        public int Cookie { get; set; }
    }
}
