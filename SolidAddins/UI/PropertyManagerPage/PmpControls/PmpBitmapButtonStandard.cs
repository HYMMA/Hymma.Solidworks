using SolidWorks.Interop.swconst;

namespace Hymma.SolidTools.SolidAddins
{
    public class PmpBitmapButtonStandard : PmpControl
    {
        public PmpBitmapButtonStandard() : base(swPropertyManagerPageControlType_e.swControlType_BitmapButton)
        {

        }

        /// <summary>
        /// PropertyManager page bitmap buttons.
        /// </summary>
        public swPropertyManagerPageBitmapButtons_e Image { get; set; }
    }
}
