using Hymma.Solidworks.Extensions;
using System;
using System.Collections.Generic;

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// a group in property manager page that host the <see cref="PmpControl"/>
    /// </summary>
    public class PmpGroupFluent : PmpGroup, IPmpGroupFluent
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="caption">caption of thie control inside the pmp</param>
        /// <param name="expanded">exapnsion state of the group upon load</param>
        public PmpGroupFluent(string caption, bool expanded) : base(caption, expanded)
        {

        }

        /// <inheritdoc/>
        internal IPmpUiModelFluent PropertyManagerPageUIBase { get; set; }

        /// <inheritdoc/>
        public IPmpGroupFluent That()
        {
            return this;
        }

        /// <inheritdoc/>
        public IPmpGroupFluent And()
        {
            return this;
        }

        /// <inheritdoc/>
        public IPmpGroupFluent IsExpanded(bool isExpanded = true)
        {
            Expanded = isExpanded;
            return this;
        }

        ///<inheritdoc/>
        public IPmpGroupFluent OnExpansionChange(Action<PmpGroup, bool> doThis)
        {
            ExpansionChanged += (sender, e) => { doThis?.Invoke((PmpGroup)sender, e); };
            return this;
        }

        /// <inheritdoc/>
        public IPmpGroupFluent HasTheseControls(Func<IEnumerable<PmpControl>> controlMaker)
        {

            AddControls(controlMaker.Invoke());
            return this;
        }
        /// <inheritdoc/>
        public IPmpGroupFluent HasTheseControls(IEnumerable<PmpControl> controls)
        {
            AddControls(controls);
            return this;
        }

        /// <inheritdoc/>
        public IPmpUiModelFluent SaveGroup()
        {
            return PropertyManagerPageUIBase as PmpUiModelFluent;
        }

        ///<inheritdoc/>
        public IPmpGroupFluent Color(SysColor sysColor)
        {
            BackgroundColor = sysColor;
            return this;
        }

        /// <summary>
        /// if set to true hides this group by default
        /// </summary>
        /// <param name="isHidden"></param>
        /// <returns></returns>
        public IPmpGroupFluent IsHidden(bool isHidden = true)
        {
            Visible = !isHidden;
            return this;
        }
    }
}
