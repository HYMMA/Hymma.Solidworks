// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.Collections.Generic;

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// a checkable group inside a tab inside a property manager page
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
        public IPmpTabGroupFluentCheckable OnExpansionChange(Action<PmpGroup, bool> doThis)
        {
            this.Expanded += (sender, e) => doThis?.Invoke((PmpGroup)sender, e);
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable Color(SysColor sysColor)
        {
            BackgroundColor = sysColor;
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable HasTheseControls(Func<IEnumerable<IPmpControl>> controlMaker)
        {
            AddControls(controlMaker?.Invoke());
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable HasTheseControls(IEnumerable<IPmpControl> controls)
        {
            AddControls(controls);
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable SetExpansion(bool isExpanded = true)
        {
            IsExpanded = isExpanded;
            return this;
        }

        ///<inheritdoc/>
        public IPmpTabGroupFluentCheckable IsHidden(bool isHidden = true)
        {
            Visible = !isHidden;
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
            this.Checked += doThis;
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
