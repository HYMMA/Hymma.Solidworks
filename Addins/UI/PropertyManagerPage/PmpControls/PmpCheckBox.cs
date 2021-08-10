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
        internal bool checkedByUser;

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="isChecked">initial state</param>
        /// <param name="caption">caption for this check box</param>
        public PmpCheckBox(string caption, bool isChecked = false) : base(swPropertyManagerPageControlType_e.swControlType_Checkbox)
        {
            checkedByUser = isChecked;
            Caption = caption;
            OnRegister += PmpCheckBox_OnRegister;
            OnDisplay += PmpCheckBox_OnDisplay;
        }

        //whenever propety manager page is shown the checkbox state should reflect
        //the previous setup. previous setup is the last time property manager page was shown
        //this way users will get a consistent experience
        private void PmpCheckBox_OnDisplay()
        {
            SolidworksObject.Checked = IsChecked;
        }

        private void PmpCheckBox_OnRegister()
        {
            SolidworksObject.Checked = IsChecked;
        }

        /// <summary>
        /// status of this checkbox
        /// </summary>
        public bool IsChecked
        {
            get => checkedByUser;
            set
            {
                checkedByUser = value;
                if (SolidworksObject != null)
                    SolidworksObject.Checked = value;
            }
        }

        /// <summary>
        /// SOLIDWORKS will call this once the checkbox is clicked on
        /// </summary>
        public Action<bool> OnChecked { get; set; }
    }
}
