using SolidWorks.Interop.swconst;
using System.Collections.Generic;

namespace Hymma.SolidTools.SolidAddins
{
    public class SwListBox : SwPMPConcreteControl
    {
        public SwListBox():base(swPropertyManagerPageControlType_e.swControlType_Listbox)
        {

        }
        public IEnumerable<string> Items { get; set; }
        public short Height { get; set; }
    }
}
