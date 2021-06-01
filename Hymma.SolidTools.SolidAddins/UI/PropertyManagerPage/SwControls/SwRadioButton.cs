using SolidWorks.Interop.swconst;
using System.Drawing;

namespace Hymma.SolidTools.SolidAddins
{
    public class SwRadioButton : SwPMPConcreteControl
    {
        public SwRadioButton():base(swPropertyManagerPageControlType_e.swControlType_Option)
        {

        }
    }
}
