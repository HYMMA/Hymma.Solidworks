// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins.Core;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using WeakEvent;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a checkable group in property manager page
    /// </summary>
    public class PmpGroupCheckable : PmpGroup
    {
        private bool _isChecked;

        /// <summary>
        /// construct a property manage page group to host numerous <see cref="IPmpControl"/>
        /// </summary>
        /// <param name="caption">text that appears next to a group box</param>
        /// <param name="visible">if set to false group will be hidden by default</param>
        /// <param name="isChecked">if set to true group will appear checked by default</param>
        /// <param name="expanded">if set to true group will appear expanded by default</param>
        public PmpGroupCheckable(string caption, bool visible = true, bool isChecked = true, bool expanded = true) : base(caption, expanded: expanded, visible: visible)
        {
            IsChecked = isChecked;
            _options = swAddGroupBoxOptions_e.swGroupBoxOptions_Checkbox;
            Displaying += PmpGroupCheckable_OnDisplay;
            Checked += PmpGroupCheckable_Checked;
        }

        private void PmpGroupCheckable_Checked(object sender, bool e)
        {
            if (_isChecked!=e)
                _isChecked = e;
        }

        private void PmpGroupCheckable_OnDisplay(object sender, EventArgs e)
        {
            var group = sender as PmpGroupCheckable;
            group.IsExpanded = group.IsChecked;
            foreach (var control in Controls)
            {
                control.Visible = group.IsChecked;
                control.Enabled = group.IsChecked;
            }
        }


        /// <summary>
        /// construct a property manager page group to host numerous <see cref="IPmpControl"/>
        /// </summary>
        /// <param name="caption">text that appears next to a group box</param>
        /// <param name="controls">list of controls to add to this group</param>
        /// <param name="visible">if set to false group will be hidden by default</param>
        /// <param name="isChecked">if set to true group will appear checked by default</param>
        /// <param name="expanded">if set to true group will appear expanded by default</param>
        public PmpGroupCheckable(string caption, List<IPmpControl> controls, bool visible = true, bool isChecked = true, bool expanded = true) : this(caption, visible, isChecked, expanded)
        {
            Controls = controls;
        }

        /// <summary>
        /// Gets or sets if this property manager page group is checked
        /// </summary>
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                if (_isChecked)
                    _options |= swAddGroupBoxOptions_e.swGroupBoxOptions_Checked;
                else
                    _options &= ~swAddGroupBoxOptions_e.swGroupBoxOptions_Checked;

                if (SolidworksObject != null)
                {
                    SolidworksObject.Checked = _isChecked;
                }

                else
                {
                    Registering += (s,e) => SolidworksObject.Checked = _isChecked;
                }
            }
        }
        internal void GroupCheckedCallback(bool status)
        {
            //by default we want the visibility and enable status of controls in a group change when user expands a group for instance
            foreach (var control in Controls)
            {
                control.Visible=status;
                control.Enabled=status;
            }
            _checkedEvents?.Raise(this, status);
        }

        private readonly WeakEventSource<bool> _checkedEvents = new WeakEventSource<bool>();
        
        /// <summary>
        /// method to invoke when user checks a group <br/>
        /// this delegate requires a boolean variable to indicate the IsChecked status of the group
        /// </summary>
        public event EventHandler<bool> Checked
        {
            add { _checkedEvents.Subscribe(this,value); }
            remove { _checkedEvents.Unsubscribe(value); }
        }

        /// <summary>
        /// unsubscribe from all events
        /// </summary>
        public override void UnsubscribeFromEvents()
        {
            base.UnsubscribeFromEvents();
            _checkedEvents.ClearHandlers();
            //Checked?.GetInvocationList()?.ToList().ForEach(d =>
            //{
            //    Checked -= (EventHandler<bool>)d;
            //});
        }
    }
}
