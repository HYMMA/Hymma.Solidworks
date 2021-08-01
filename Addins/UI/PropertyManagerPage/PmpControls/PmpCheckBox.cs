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
        private readonly bool _isChecked;

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="isChecked">initial state</param>
        public PmpCheckBox(bool isChecked = false) : base(swPropertyManagerPageControlType_e.swControlType_Checkbox)
        {
            _isChecked = isChecked;
            OnRegister += PmpCheckBox_OnRegister;
            //whenever propety manager page is shown the checkbox state should reflect
            //the previous setup. previous setup is the last time property manager page was shown
            //this way users will get a consistent experience
            BeforeDisplay = () =>
            {
                SolidworksObject.Checked = IsChecked.GetValueOrDefault();
            };
        }

        private void PmpCheckBox_OnRegister()
        {
            IsChecked = _isChecked;
        }

        /// <summary>
        /// status of this checkbox
        /// </summary>
        public bool? IsChecked
        {
            get => SolidworksObject?.Checked;
            set => SolidworksObject.Checked = value.GetValueOrDefault();
        }

        /// <summary>
        /// SOLIDWORKS will call this once the checkbox is clicked on
        /// </summary>
        public Action<bool> OnChecked { get; set; }
    }
}
