using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a solidworks radio button in property managers
    /// </summary>
    public class PmpRadioButton : PmpControl<IPropertyManagerPageOption>
    {
        internal bool checkedByUser;

        /// <summary>
        /// make a new radio button for solidworks property manager pages
        /// </summary>
        /// <param name="caption">caption for this radio button</param>
        /// <param name="isChecked">whether it is going to be the checked or not</param>
        public PmpRadioButton(string caption, bool isChecked = false) : base(swPropertyManagerPageControlType_e.swControlType_Option)
        {
            checkedByUser = isChecked;
            Caption = caption;
            //solidworks requires us to regiter the control once the addin is loaded.
            //then everytime the property manage rpage is displayed the status of controls would reflect the registered state
            //to provide a consitant experience between sessions of calling a property manager 
            //we use the OnDisplay event to update the status of the control to that of previous call
            OnRegister += PmpRadioButton_OnRegister;
            OnDisplay += PmpRadioButton_OnDisplay;
        }

        private void PmpRadioButton_OnDisplay(object sender, OnDisplay_EventArgs e)
        {
            SolidworksObject.Checked = IsChecked;
        }

        private void PmpRadioButton_OnRegister()
        {
            PmpRadioButton_OnDisplay(null,null);
            SolidworksObject.Caption = Caption;
        }

        /// <summary>
        /// whether or not this radio button is checked
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
        /// SOLIDWORKS will invoke this delegate once the user checks this radio button
        /// </summary>
        public Action OnChecked { get; set; }
    }
}
