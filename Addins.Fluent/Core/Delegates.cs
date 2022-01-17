using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.Solidworks.Addins.Fluent
{
    public delegate void CustomAction(PmpGroup group, bool val);

    /// <summary>
    /// Equivalent to Func&lt;IEnumerable&lt;PmpControl&gt;&gt;
    /// </summary>
    /// <returns></returns>
    public delegate PmpControl[] CustomFunc();

}
