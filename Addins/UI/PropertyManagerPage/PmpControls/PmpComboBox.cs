using SolidWorks.Interop.swconst;
using System.Collections.Generic;

namespace Hymma.SolidTools.Addins
{
    public class PmpComboBox : PmpControl
    {
        public PmpComboBox():base(swPropertyManagerPageControlType_e.swControlType_Combobox)
        {

        }
        public IEnumerable<string> Items { get; set; }
        public short Height { get; set; }
    }
}
