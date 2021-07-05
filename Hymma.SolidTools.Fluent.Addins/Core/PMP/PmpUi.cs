using Hymma.SolidTools.Addins;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// controls the general settings/Events of aproperty manger page
    /// </summary>
    public class PmpUi : PropertyManagerPageUIBase , IFluent
    {
       /// <summary>
       /// default constructor
       /// </summary>
       /// <param name="solidworks"></param>
        public PmpUi(ISldWorks solidworks):base(solidworks)
        {

        }
        /// <summary>
        /// the addin model that hosts this ui
        /// </summary>
        public AddinModel AddinModel { get; internal set; }
    }
}
