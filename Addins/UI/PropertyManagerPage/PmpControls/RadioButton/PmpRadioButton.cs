// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a SolidWORKS radio button in property managers
    /// </summary>
    public class PmpRadioButton : PmpControl<IPropertyManagerPageOption>
    {
        private bool _isChecked;

        /// <summary>
        /// make a new radio button for SolidWORKS property manager pages
        /// </summary>
        /// <param name="caption">caption for this radio button</param>
        /// <param name="isChecked">whether it is going to be the checked or not</param>
        public PmpRadioButton(string caption, bool isChecked = false) : base(swPropertyManagerPageControlType_e.swControlType_Option, caption)
        {
            IsChecked = isChecked;

            //bind this to SolidWORKS
            Checked += PmpRadioButton_Checked;
        }

        private void PmpRadioButton_Checked(object sender, bool e)
        {
            if (_isChecked != e)
                _isChecked = e;
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
        /// provide a constant experience between sessions of calling a property manager 
        ///we update the status of the control to that of previous call
        /// </summary>
        /// <remarks>SolidWORKS requires us to register the control once the addin is loaded.
        ///then every time the property manager page is displayed the status of controls would reflect the registered state
        ///but to provide a constant experience between sessions of calling a property manager 
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
        internal void CheckedCallback() => Checked?.Invoke(this, true);
        #endregion

        #region events
        /// <summary>
        /// SOLIDWORKS will invoke this delegate once the user checks this radio button
        /// </summary>
        public event EventHandler<bool> Checked;
        private bool _maintain;
        #endregion
    }
}
