using SolidWorks.Interop.swconst;
using System;
using System.Drawing;

namespace Hymma.SolidTools.Addins
{
        /// <summary>
        /// a button in property manager page
        /// </summary>
    public class PmpButton : PmpControl
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="caption"> button text</param>
        public PmpButton(string caption) : base(swPropertyManagerPageControlType_e.swControlType_Button)
        {
            Caption = caption;
        }

        /// <summary>
        /// fires when button is pressed
        /// </summary>
        public Action OnPress { get; set; }
    }
}
