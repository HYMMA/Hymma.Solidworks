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
        private bool _IsChecked;

        /// <summary>
        /// make a new radio button for solidworks property manager pages
        /// </summary>
        public PmpRadioButton(bool IsChecked = false) : base(swPropertyManagerPageControlType_e.swControlType_Option)
        {
            this._IsChecked = IsChecked;
            OnRegister += PmpRadioButton_OnRegister;
            BeforeDisplay = () =>
            {
                SolidworksObject.Checked = _IsChecked;
            };
        }

        private void PmpRadioButton_OnRegister()
        {
            SolidworksObject.Checked = _IsChecked;
        }

        /// <summary>
        /// whether or not this radio button is checked
        /// </summary>
        public bool? IsChecked
        {
            get => _IsChecked;
            set
            {
                _IsChecked = value.GetValueOrDefault();
                //SolidworksObject.Checked = (bool)value;
            }
        }

        /// <summary>
        /// SOLIDWORKS will invoke this delegate once the user checks this radio button
        /// </summary>
        public Action OnChecked { get; set; }
    }
}
