using Hymma.SolidTools.Addins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// a group in property manager page that host the <see cref="IPmpControl"/>
    /// </summary>
    public class PropertyManagerPageGroup : PmpGroup, IFluent
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="caption">caption of thie control inside the pmp</param>
        /// <param name="expanded">exapnsion state of the group upon load</param>
        public PropertyManagerPageGroup(string caption , bool expanded) : base (caption,expanded)
        {

        }
        
        /// <summary>
        /// the <see cref="PropertyManagerPageUIBase"/> that hosts this group
        /// </summary>
        public PmpUi PropertyManagerPageUIBase { get; internal set; }
    }
}
