using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a button in property manager page
    /// </summary>
    public class PmpButton : PmpButtonBase
    {

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="caption"> button text</param>
        /// <param name="tip">tooltip text</param>
        public PmpButton(string caption, string tip = "") : base(swPropertyManagerPageControlType_e.swControlType_Button, caption, tip)
        {
            Registering += PmpButton_Registering;
        }

        /// <summary>
        /// solidworks ojbect
        /// </summary>
        public PropertyManagerPageButton SolidworksObject { get; internal set; }

        private void PmpButton_Registering()
        {
            SolidworksObject = (PropertyManagerPageButton)Control;
        }
    }
}
