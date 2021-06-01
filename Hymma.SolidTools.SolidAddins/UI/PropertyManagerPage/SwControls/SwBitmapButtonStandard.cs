using SolidWorks.Interop.swconst;

namespace Hymma.SolidTools.SolidAddins
{
    public class SwBitmapButtonStandard : SwPMPConcreteControl
    {
        public SwBitmapButtonStandard() : base(swPropertyManagerPageControlType_e.swControlType_BitmapButton)
        {

        }

        /// <summary>
        /// PropertyManager page bitmap buttons.
        /// </summary>
        public swPropertyManagerPageBitmapButtons_e Image { get; set; }
    }
}
