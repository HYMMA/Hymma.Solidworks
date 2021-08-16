using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a check box in a property manger page
    /// </summary>
    public class PmpCheckBox : PmpControl<PropertyManagerPageCheckbox>
    {
        private bool _ischecked;
        private bool _wantConsistantUserExperience;

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="isChecked">initial state</param>
        /// <param name="caption">caption for this check box</param>
        /// <param name="wantConsistantUserExperience">if set to true checkbox state will update automatically to reflect that of previous time the property manager page was used</param>
        public PmpCheckBox(string caption, bool isChecked = false, bool wantConsistantUserExperience=true) : base(swPropertyManagerPageControlType_e.swControlType_Checkbox)
        {
            _wantConsistantUserExperience = wantConsistantUserExperience;
            _ischecked = isChecked;
            Caption = caption;
            OnRegister += PmpCheckBox_OnRegister;
            OnDisplay += PmpCheckBox_OnDisplay;
        }

        #region call backs

        //whenever propety manager page is shown the checkbox state should reflect
        //the previous setup. previous setup is the last time property manager page was shown
        //this way users will get a consistent experience
        private void PmpCheckBox_OnDisplay(object sender, OnDisplay_EventArgs e)
        {
            SolidworksObject.Checked = IsChecked;
        }

        private void PmpCheckBox_OnRegister()
        {
            SolidworksObject.Checked = IsChecked;
        }

        internal void Checked(bool status)
        {
            if (_wantConsistantUserExperience)
                _ischecked = status;
            OnChecked?.Invoke(this, status);
        }

        #endregion

        /// <summary>
        /// status of this checkbox
        /// </summary>
        public bool IsChecked
        {
            get => _ischecked;
            set
            {
                _ischecked = value;
                if (SolidworksObject != null)
                    SolidworksObject.Checked = value;
            }
        }

        /// <summary>
        /// SOLIDWORKS will call this once the checkbox is clicked on
        /// </summary>
        public event CheckBox_EventHandler OnChecked;
    }
}
