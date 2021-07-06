using Hymma.SolidTools.Addins;
using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTools.Fluent.Addins

{
    /// <summary>
    /// fluent extensions for making of a solidworks addin
    /// </summary>
    public static class AddinModelExtensions
    {
        /// <summary>
        /// makes a new property amnage page ui that , in turn, makes Property manger page controls
        /// </summary>
        /// <param name="addinModel"></param>
        /// <param name="title">title [caption] of the property manager page</param>
        /// <param name="solidworks">solidworks object</param>
        /// <returns></returns>
        public static IPmpUi AddPropertyManagerPage(this AddinModel addinModel, string title, ISldWorks solidworks)
        {
            var pmp = new PmpUi(solidworks)
            {
                Title = title,
                AddinModel = addinModel
            };
            return pmp;
        }
    }
}
