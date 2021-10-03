using Hymma.SolidTools.Addins;
using Hymma.SolidTools.Core;
using System;
using System.Collections.Generic;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// a group for a tab for property manager pages
    /// </summary>
    public class PmpTabGroupFluent : PmpGroup, IPmpTabGroupFluent
    {
        /// <summary>
        /// a group inside a tab for a property manager page
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="expanded"></param>
        public PmpTabGroupFluent(string caption, bool expanded = false) : base(caption, expanded)
        {

        }
        internal IPmpTabFluent Tab { get; set; }

        /// <summary>
        /// saves this group into the <see cref="PmpTab"/>
        /// </summary>
        /// <returns></returns>
        public IPmpTabFluent SaveGroup()
        {
            return Tab;
        }

        /// <inheritdoc/>
        public IPmpTabGroupFluent That()
        {
            return this;
        }

        /// <inheritdoc/>
        public IPmpTabGroupFluent And()
        {
            return this;
        }

        /// <inheritdoc/>
        public IPmpTabGroupFluent IsExpanded(bool isExpanded = true)
        {
            Expanded = isExpanded;
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluent AndOnExpansionChange(Action<PmpGroup, bool> doThis)
        {
            OnGroupExpand += (sender, e) => { doThis?.Invoke((PmpGroup)sender, e); };
            return this;
        }

        /// <inheritdoc/>
        public IPmpTabGroupFluent HasTheseControls(Func<IEnumerable<IPmpControl>> controlMaker)
        {

            AddControls(controlMaker.Invoke());
            return this;
        }
        /// <inheritdoc/>
        public IPmpTabGroupFluent HasTheseControls(IEnumerable<IPmpControl> controls)
        {

            AddControls(controls);
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluent Color(SysColor sysColor)
        {
            BackgroundColor = sysColor;
            return this;
        }
    }
}
