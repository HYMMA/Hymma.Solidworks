using SolidWorks.Interop.swconst;
using System;
using System.ComponentModel;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// a check box in a property manger page
    /// </summary>
    public class SwCheckBox : SwPMPConcreteControl
    {

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="isChecked">initial state</param>
        public SwCheckBox(bool isChecked) : base(swPropertyManagerPageControlType_e.swControlType_Checkbox)
        {
            IsChecked = isChecked;
        }

        /// <summary>
        /// status of this checkbox
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// SOLIDWORKS will call this once the checkbox is clicked on
        /// </summary>
        public OnClicked OnClicked { get; set; }
        
    }
        
}
