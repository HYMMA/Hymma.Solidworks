using SolidWorks.Interop.swconst;
using System;
using System.Drawing;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// a solidworks radio button in property managers
    /// </summary>
    public class PmpRadioButton : PmpControl
    {
        /// <summary>
        /// make a new radio button for solidworks property manager pages
        /// </summary>
        public PmpRadioButton(bool IsChecked=false):base(swPropertyManagerPageControlType_e.swControlType_Option)
        {
            this.IsChecked = IsChecked;
        }

        /// <summary>
        /// whether or not this radio button is checked
        /// </summary>
        public bool IsChecked { get; internal set; }

        /// <summary>
        /// SOLIDWORKS will invoke this delegate once the user checks this radio button
        /// </summary>
        public Action OnChecked { get; set; }
    }
}
