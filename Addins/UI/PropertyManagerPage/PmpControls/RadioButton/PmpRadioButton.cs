using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a solidworks radio button in property managers
    /// </summary>
    public class PmpRadioButton : PmpControl<IPropertyManagerPageOption>
    {
        private bool _isChecked;

        /// <summary>
        /// make a new radio button for solidworks property manager pages
        /// </summary>
        /// <param name="caption">caption for this radio button</param>
        /// <param name="isChecked">whether it is going to be the checked or not</param>
        public PmpRadioButton(string caption, bool isChecked = false) : base(swPropertyManagerPageControlType_e.swControlType_Option, caption)
        {
            IsChecked = isChecked;
        }
        
        #region properties

        /// <summary>
        /// whether or not this radio button is checked
        /// </summary>
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                //if addin was loaded
                if (SolidworksObject != null)
                    SolidworksObject.Checked = value;
                else
                    Registering += () => { SolidworksObject.Checked = value; };
            }
        }

        /// <summary>
        /// provide a consitant experience between sessions of calling a property manager 
        ///we update the status of the control to that of previous call
        /// </summary>
        /// <remarks>solidworks requires us to regiter the control once the addin is loaded.
        ///then everytime the property manage rpage is displayed the status of controls would reflect the registered state
        ///but to provide a consitant experience between sessions of calling a property manager 
        ///we use this property to update the status of the control to that of previous call</remarks>
        public bool MaintainState
        {
            get => _maintain;
            set
            {
                _maintain = value;
                if (_maintain)
                {
                    Displaying += (sender, e) =>
                    {
                        var radioButton = sender as PmpRadioButton;
                        radioButton.IsChecked = _isChecked;
                    };
                }
            }
        }
        #endregion

        #region call backs
        internal void CheckedCallback() =>Checked?.Invoke(this, EventArgs.Empty);
        #endregion

        #region events
        /// <summary>
        /// SOLIDWORKS will invoke this delegate once the user checks this radio button
        /// </summary>
        public EventHandler Checked;
        private bool _maintain;
        #endregion
    }
}
