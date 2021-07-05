using Hymma.SolidTools.Addins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTools.Fluent.Addins
{
    public static class AddinModelExtensions
    {

        public static PmpUi AddPropertyManagerPage(this AddinModel addinModel, string title)
        {
            var pmp = new PmpUi();
            pmp.Title = title;
            addinModel.PropertyManagerPages.Add((PmpBase)pmp as); 
            return pmp;
        }
    }
}
