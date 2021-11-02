using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a checkable gorup in proeprty manager page
    /// </summary>
    public class PmpGroupCheckable : PmpGroup
    {
        private bool _isChecked;

        /// <summary>
        /// construct a property manage page group to host numerous <see cref="IPmpControl"/>
        /// </summary>
        /// <param name="caption">text that appears next to a group box</param>
        /// <param name="visible">if set to false gorup will be hiddend by default</param>
        /// <param name="isChecked">if set to true gorup will appear checked by default</param>
        /// <param name="expanded">if set to true group will appear expanded by default</param>
        public PmpGroupCheckable(string caption , bool visible = true, bool isChecked = true, bool expanded = true) : base(caption,expanded:expanded,visible:visible)
        {
            IsChecked = isChecked;
            _options = swAddGroupBoxOptions_e.swGroupBoxOptions_Checkbox;
        }


        /// <summary>
        /// construct a property manager page group to host numerous <see cref="IPmpControl"/>
        /// </summary>
        /// <param name="caption">text that appears next to a group box</param>
        /// <param name="controls">list of controls to add to this group</param>
        /// <param name="visible">if set to false gorup will be hiddend by default</param>
        /// <param name="isChecked">if set to true gorup will appear checked by default</param>
        /// <param name="expanded">if set to true group will appear expanded by default</param>
        public PmpGroupCheckable(string caption, List<IPmpControl> controls, bool visible = true, bool isChecked = true , bool expanded = true) : this(caption,visible,isChecked,expanded)
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
                if (_isChecked )
                    _options |= swAddGroupBoxOptions_e.swGroupBoxOptions_Checked;
                else 
                    _options &= ~swAddGroupBoxOptions_e.swGroupBoxOptions_Checked;

                if (SolidworksObject != null)
                {
                    SolidworksObject.Checked = _isChecked;
                    GroupChecked(value);
                }
                    
                else
                {
                    OnRegister += () => SolidworksObject.Checked = _isChecked;
                    GroupChecked(value);
                }
            }
        }
        internal void GroupChecked(bool status)
        {
            Controls.ForEach(c => c.Visible = status);
            OnGroupCheck?.Invoke(this, status);
        }

        /// <summary>
        /// method to invoke when user checks a group <br/>
        /// this delegate requires a bool variable to indicate the IsChecked status of the group
        /// </summary>
        public event EventHandler<bool> OnGroupCheck;
    }
}
