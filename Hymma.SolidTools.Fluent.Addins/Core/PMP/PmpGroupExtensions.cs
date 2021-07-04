using Hymma.SolidTools.Addins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// extension methodes for <see cref="PmpGroup"/>
    /// </summary>
    public static class PmpGroupExtensions
    {

        /// <summary>
        /// add extra context to this <see cref="PmpGroup"/>
        /// </summary>
        /// <param name="pmpGroup"></param>
        /// <returns></returns>
        public static PmpGroup That(this PmpGroup pmpGroup)
        {
            return pmpGroup;
        }
        /// <summary>
        /// add extra context to this <see cref="PmpGroup"/>
        /// </summary>
        /// <param name="pmpGroup"></param>
        /// <returns></returns>
        public static PmpGroup And(this PmpGroup pmpGroup)
        {
            return pmpGroup;
        }

        /// <summary>
        /// determines if this <see cref="PmpGroup"/> is expanded or not
        /// </summary>
        /// <param name="pmpGroup"></param>
        /// <param name="isExpanded"></param>
        /// <returns></returns>
        public static PmpGroup IsExpanded(this PmpGroup pmpGroup, bool isExpanded=true)
        {
            pmpGroup.Expanded = isExpanded;
            return pmpGroup;
        }

        /// <summary>
        /// event handler for when user expands the group
        /// </summary>
        /// <param name="pmpGroup"></param>
        /// <param name="doThis">delegate that accepts a bool as the parameter and returns void. bool parameter will be assigned by solidworks and indicates the expansion state of the group.</param>
        /// <returns></returns>
        public static PmpGroup AndOnExpansionChange(this PmpGroup pmpGroup, Action<bool> doThis)
        {
            pmpGroup.OnGroupExpand = doThis;
            return pmpGroup;
        }

        /// <summary>
        /// add these controls to the <see cref="PmpGroup"/>
        /// </summary>
        /// <param name="pmpGroup"></param>
        /// <param name="controlMaker">function that returns an <see cref="IEnumerable{T}"/> where T : <see cref="IPmpControl"/></param>
        /// <returns></returns>
        public static PmpGroup HasTheseControls(this PmpGroup pmpGroup, Func<IEnumerable<IPmpControl>> controlMaker)
        {
            
            pmpGroup.AddControls(controlMaker.Invoke());
            return pmpGroup;
        }
        /// <summary>
        /// add these controls to the <see cref="PmpGroup"/>
        /// </summary>
        /// <param name="pmpGroup"></param>
        /// <param name="controls">an <see cref="IEnumerable{T}"/> where T : <see cref="IPmpControl"/></param>
        /// <returns></returns>
        public static PmpGroup HasTheseControls(this PmpGroup pmpGroup,IEnumerable<IPmpControl> controls)
        {
            
            pmpGroup.AddControls(controls);
            return pmpGroup;
        }
        
        /// <summary>
        /// save all the changes 
        /// </summary>
        /// <returns></returns>
        public static PmpUi SaveGroup(this PmpGroup pmpGroup)
        {
            return pmpGroup.PropertyManagerPageUIBase as PmpUi;
        }
    }
}
