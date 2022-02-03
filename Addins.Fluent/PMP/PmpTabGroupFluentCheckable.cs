using Hymma.Solidworks.Extensions;
using System;
using System.Collections.Generic;

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// a checkeckable group inside a tab inside a property manager page
    /// </summary>
    public class PmpTabGroupFluentCheckable : PmpGroupCheckable, IPmpTabGroupFluentCheckable
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="visible"></param>
        /// <param name="isChecked"></param>
        /// <param name="expanded"></param>
        public PmpTabGroupFluentCheckable(string caption, bool visible = true, bool isChecked = true, bool expanded = true) : base(caption, visible, isChecked, expanded)
        {

        }

        /// <summary>
        /// the tab this group belongs to
        /// </summary>
        public PmpTabFluent Tab { get; internal set; }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable And()
        {
            return this;
        }
        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable OnExpansion(OnPmpGroupExpansionEventHandler onPmpGroupExpandedEventHandler)
        {
            ExpansionChanged += (sender, e) => onPmpGroupExpandedEventHandler?.Invoke((PmpGroup)sender, e);
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable Color(SysColor sysColor)
        {
            BackgroundColor = sysColor;
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable HasTheseControls(MakePmpControls makePmpControls)
        {
            AddControls(makePmpControls?.Invoke());
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable HasTheseControls(PmpControl[] controls)
        {
            AddControls(controls);
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable SetExpansion(bool isExpanded = true)
        {
            Expanded = isExpanded;
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable SetVisibility(bool visibility)
        {
            Visible = visibility;
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabFluent SaveGroup()
        {
            return Tab;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable That()
        {
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable SetCheckedStatus(bool status = true)
        {
            base.IsChecked = status;
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable OnChecked(EventHandler<bool> doThis)
        {
            Checked += doThis;
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable OnDisplaying(EventHandler doThis)
        {
            Displaying += doThis;
            return this;
        }
    }
}
