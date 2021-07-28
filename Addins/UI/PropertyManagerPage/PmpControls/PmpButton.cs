using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a button in property manager page
    /// </summary>
    public class PmpButton : PmpControl<IPropertyManagerPageButton>
    {

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="caption"> button text</param>
        public PmpButton(string caption) : base(swPropertyManagerPageControlType_e.swControlType_Button)
        {
            Caption = caption;
            OnRegister += PmpButton_OnRegister;
        }

        private void PmpButton_OnRegister()
        {
        }

        /// <summary>
        /// fires when button is pressed
        /// </summary>
        public Action OnPress { get; set; }
    }
}
