using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTools.Addins
{
    public enum PMPCloseReason
    {
        UnknownReason = 0,
        Okay = 1,
        Cancel = 2,
        ParentClosed = 3,
        Closed = 4,
        UserEscape = 5,
        Apply = 6,
        Preview = 7
    }
}
