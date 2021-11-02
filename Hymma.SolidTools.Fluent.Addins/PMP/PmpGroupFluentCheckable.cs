using Hymma.SolidTools.Addins;
using Hymma.SolidTools.Core;
using System;
using System.Collections.Generic;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// a group in property manager page that host the <see cref="IPmpControl"/>
    /// </summary>
    public class PmpGroupFluentCheckable : PmpGroupCheckable, IPmpGroupFluentCheckable
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="caption">caption of thie control inside the pmp</param>
        /// <param name="visible"></param>
        /// <param name="isChecked"></param>
        /// <param name="expanded">exapnsion state of the group upon load</param>
        public PmpGroupFluentCheckable(string caption, bool visible = true, bool isChecked = true, bool expanded = true) : base(caption, visible, isChecked, expanded)
        {

        }

        /// <inheritdoc/>
        internal IPmpUiModelFluent PropertyManagerPageUIBase { get; set; }

        /// <inheritdoc/>
        public IPmpGroupFluentCheckable That()
        {
            return this;
        }

        /// <inheritdoc/>
        public IPmpGroupFluentCheckable And()
        {
            return this;
        }

        /// <inheritdoc/>
        public IPmpGroupFluentCheckable IsExpanded(bool isExpanded = true)
        {
            Expanded = isExpanded;
            return this;
        }

        ///<inheritdoc/>
        public IPmpGroupFluentCheckable AndOnExpansionChange(Action<PmpGroup, bool> doThis)
        {
            OnGroupExpand += (sender, e) => { doThis?.Invoke((PmpGroup)sender, e); };
            return this;
        }

        /// <inheritdoc/>
        public IPmpGroupFluentCheckable HasTheseControls(Func<IEnumerable<IPmpControl>> controlMaker)
        {

            AddControls(controlMaker.Invoke());
            return this;
        }
        /// <inheritdoc/>
        public IPmpGroupFluentCheckable HasTheseControls(IEnumerable<IPmpControl> controls)
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
        public IPmpGroupFluentCheckable Color(SysColor sysColor)
        {
            BackgroundColor = sysColor;
            return this;
        }

        /// <summary>
        /// if set to true hides this group by default
        /// </summary>
        /// <param name="isHidden"></param>
        /// <returns></returns>
        public IPmpGroupFluentCheckable IsHidden(bool isHidden = true)
        {
            Visible = !isHidden;
            return this;
        }

        ///<inheritdoc/>
        public IPmpGroupFluentCheckable Checked(bool status = true)
        {
            base.IsChecked = status;
            return this;
        }

        ///<inheritdoc/>
        public IPmpGroupFluentCheckable WhenChecked(EventHandler<bool> doThis)
        {
            base.OnGroupCheck += doThis;
            return this;
        }

        ///<inheritdoc/>
        public IPmpGroupFluentCheckable WhenDisplayed(EventHandler doThis)
        {
            OnDisplay += doThis;
            return this;
        }
    }
}
