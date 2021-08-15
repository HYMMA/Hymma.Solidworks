using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.SolidTools.Addins
{

    /// <summary>
    /// provides useful arguments and parameters for <see cref="IPmpControl.OnDisplay"/>
    /// </summary>
    public class OnDisplay_EventArgs : EventArgs
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="activeDocument"></param>
        public OnDisplay_EventArgs(ModelDoc2 activeDocument)
        {
            ActiveDoc = activeDocument;
        }
        /// <summary>
        /// the active document in this 
        /// </summary>
        public ModelDoc2 ActiveDoc { get; }
    }

}
