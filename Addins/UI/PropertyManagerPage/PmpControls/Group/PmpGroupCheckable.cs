using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a checkable gorup in proeprty manager page
    /// </summary>
    public class PmpGroupCheckable : PmpGroup
    {
        private bool _isChecked;

        /// <summary>
        /// construct a property manage page group to host numerous <see cref="PmpControl"/>
        /// </summary>
        /// <param name="caption">text that appears next to a group box</param>
        /// <param name="visible">if set to false gorup will be hiddend by default</param>
        /// <param name="isChecked">if set to true gorup will appear checked by default</param>
        /// <param name="expanded">if set to true group will appear expanded by default</param>
        public PmpGroupCheckable(string caption, bool visible = true, bool isChecked = true, bool expanded = true) : base(caption, expanded: expanded, visible: visible)
        {
            IsChecked = isChecked;
            _options = swAddGroupBoxOptions_e.swGroupBoxOptions_Checkbox;
            Displaying += PmpGroupCheckable_OnDisplay;
        }

        private void PmpGroupCheckable_OnDisplay(object sender, EventArgs e)
        {
            var group = sender as PmpGroupCheckable;
            group.Expanded = group.IsChecked;
            foreach (var control in Controls)
            {
                control.Visible = group.IsChecked;
                control.Enabled = group.IsChecked;
            }
        }


        /// <summary>
        /// construct a property manager page group to host numerous <see cref="PmpControl"/>
        /// </summary>
        /// <param name="caption">text that appears next to a group box</param>
        /// <param name="controls">list of controls to add to this group</param>
        /// <param name="visible">if set to false gorup will be hiddend by default</param>
        /// <param name="isChecked">if set to true gorup will appear checked by default</param>
        /// <param name="expanded">if set to true group will appear expanded by default</param>
        public PmpGroupCheckable(string caption, List<PmpControl> controls, bool visible = true, bool isChecked = true, bool expanded = true) : this(caption, visible, isChecked, expanded)
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
                    Registering += () => SolidworksObject.Checked = _isChecked;
                }
            }
        }
        internal void CheckedCallBack(bool status)
        {
            foreach (var control in Controls)
            {
                control.Visible=status;
                control.Enabled=status;
            }
            Checked?.Invoke(this, status);
        }

        /// <summary>
        /// method to invoke when user checks a group <br/>
        /// this delegate requires a bool variable to indicate the IsChecked status of the group
        /// </summary>
        public event EventHandler<bool> Checked;
    }
}
