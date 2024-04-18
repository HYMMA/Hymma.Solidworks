// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a check box in a property manger page
    /// </summary>
    public class PmpCheckBox : PmpControl<PropertyManagerPageCheckbox>
    {
        #region private fields

        private bool _isChecked;
        private bool _maintain;
        #endregion

        #region constructor

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="isChecked">initial state</param>
        /// <param name="caption">caption for this check box</param>
        /// <param name="tip">a tip for this checkbox</param>
        public PmpCheckBox(string caption, bool isChecked = false, string tip = "") : base(swPropertyManagerPageControlType_e.swControlType_Checkbox, caption, tip)
        {
            IsChecked = isChecked;

            //sync SOLIDWORKS and this
            Checked += PmpCheckBox_Checked;
        }

        private void PmpCheckBox_Checked(PmpCheckBox pmpCheckBox, bool isChecked)
        {
            if (_isChecked != isChecked)
                _isChecked = isChecked;
        }


        #endregion

        #region call backs
        internal void CheckedCallback(bool status) => Checked?.Invoke(this, status);
        #endregion

        #region public properties

        /// <summary>
        /// status of this checkbox
        /// </summary>
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;

                //if addin is loaded
                if (SolidworksObject != null)
                {
                    SolidworksObject.Checked = value;
                    //Checked(value);
                }
                else
                {
                    Registering += () => { SolidworksObject.Checked = value; };
                    //Checked(value);
                }
            }
        }

        /// <summary>
        /// provide a constant experience between sessions of calling a property manager 
        ///we update the status of the control to that of previous call
        /// </summary>
        /// <remarks>SolidWORKS requires us to register the control once the addin is loaded.
        ///then every time the property manage page is displayed the status of controls would reflect the registered state
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
                        var checkBox = sender as PmpCheckBox;
                        checkBox.IsChecked = _isChecked;
                    };
                }
            }
        }
        #endregion

        #region events
        /// <summary>
        /// SOLIDWORKS will call this once the checkbox is clicked on
        /// </summary>
        public event PmpCheckBoxCheckedEventHandler Checked;
        #endregion
    }
}
