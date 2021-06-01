using SolidWorks.Interop.swconst;
using System.Drawing;

namespace Hymma.SolidTools.SolidAddins
{
    public class SwButton : SwPMPConcreteControl
    {
        public SwButton() : base(swPropertyManagerPageControlType_e.swControlType_Button)
        {

        }
    }
}
