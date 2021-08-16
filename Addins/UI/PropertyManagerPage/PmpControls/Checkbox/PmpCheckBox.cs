using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a check box in a property manger page
    /// </summary>
    public class PmpCheckBox : PmpControl<PropertyManagerPageCheckbox>
    {
        #region private fields

        private bool _ischecked;
        private bool _wantConsistantUserExperience;
        #endregion

        #region constructor

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="isChecked">initial state</param>
        /// <param name="caption">caption for this check box</param>
        /// <param name="wantConsistantUserExperience">if set to true checkbox state will update automatically to reflect that of previous time the property manager page was used</param>
        /// <param name="tip">a tip for this checkbox</param>
        public PmpCheckBox(string caption, bool isChecked = false, bool wantConsistantUserExperience = true, string tip ="") : base(swPropertyManagerPageControlType_e.swControlType_Checkbox,caption,tip)
        {
            _wantConsistantUserExperience = wantConsistantUserExperience;
            _ischecked = isChecked;
            OnRegister += PmpCheckBox_OnRegister;
            OnDisplay += PmpCheckBox_OnDisplay;
        }


        #endregion

        #region call backs
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

        #region public properties

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
        #endregion

        #region events
        /// <summary>
        /// SOLIDWORKS will call this once the checkbox is clicked on
        /// </summary>
        public event CheckBox_EventHandler OnChecked;
        #endregion
    }
}
