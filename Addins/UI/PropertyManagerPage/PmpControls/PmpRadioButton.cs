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
        /// <summary>
        /// make a new radio button for solidworks property manager pages
        /// </summary>
        public PmpRadioButton(bool isChecked = false) : base(swPropertyManagerPageControlType_e.swControlType_Option)
        {
            this.IsChecked = isChecked;

            //soli8dworks requires us to regiter the control once the addin is loaded.
            //then everytime the property manage rpage is displayed the status of controls would reflect the registered state
            //to provide a consitant experience between sessions of calling a property manager 
            //we use the OnDisplay event to update the status of the control to that of previous call
            OnRegister += PmpRadioButton_OnRegister;
            OnDisplay += PmpRadioButton_OnDisplay;
        }

        private void PmpRadioButton_OnDisplay()
        {
            SolidworksObject.Checked = IsChecked.GetValueOrDefault();
        }

        private void PmpRadioButton_OnRegister()
        {
            SolidworksObject.Checked = IsChecked.GetValueOrDefault();
        }

        /// <summary>
        /// whether or not this radio button is checked
        /// </summary>
        public bool? IsChecked{get;set;}

        /// <summary>
        /// SOLIDWORKS will invoke this delegate once the user checks this radio button
        /// </summary>
        public Action OnChecked { get; set; }
    }
}
