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
    public class PropertyManagerPageGroup : PmpGroup, IPmpGroup, IFluent
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
        internal IPmpUi PropertyManagerPageUIBase { get; set; }


        /// <summary>
        /// add extra context to this <see cref="PmpGroup"/>
        /// </summary>
        /// <returns></returns>
        public IPmpGroup That()
        {
            return this;
        }

        /// <summary>
        /// add extra context to this <see cref="PmpGroup"/>
        /// </summary>
        /// <returns></returns>
        public IPmpGroup And()
        {
            return this;
        }

        /// <summary>
        /// determines if this <see cref="PmpGroup"/> is expanded or not
        /// </summary>
        /// <param name="isExpanded"></param>
        /// <returns></returns>
        public IPmpGroup IsExpanded(bool isExpanded = true)
        {
            this.Expanded = isExpanded;
            return this;
        }

        /// <summary>
        /// event handler for when user expands the group
        /// </summary>
        /// <param name="doThis">delegate that accepts a bool as the parameter and returns void. bool parameter will be assigned by solidworks and indicates the expansion state of the group.</param>
        /// <returns></returns>
        public IPmpGroup AndOnExpansionChange(Action<bool> doThis)
        {
            this.OnGroupExpand = doThis;
            return this;
        }

        /// <summary>
        /// add these controls to the <see cref="PmpGroup"/>
        /// </summary>
        /// <param name="controlMaker">function that returns an <see cref="IEnumerable{T}"/> where T : <see cref="IPmpControl"/></param>
        /// <returns></returns>
        public IPmpGroup HasTheseControls(Func<IEnumerable<IPmpControl>> controlMaker)
        {

            this.AddControls(controlMaker.Invoke());
            return this;
        }
        /// <summary>
        /// add these controls to the <see cref="PmpGroup"/>
        /// </summary>
        /// <param name="controls">an <see cref="IEnumerable{T}"/> where T : <see cref="IPmpControl"/></param>
        /// <returns></returns>
        public IPmpGroup HasTheseControls(IEnumerable<IPmpControl> controls)
        {

            this.AddControls(controls);
            return this;
        }

        /// <summary>
        /// save all the changes 
        /// </summary>
        /// <returns></returns>
        public IPmpUi SaveGroup()
        {
            return PropertyManagerPageUIBase as PmpUi;
        }
    }
}
