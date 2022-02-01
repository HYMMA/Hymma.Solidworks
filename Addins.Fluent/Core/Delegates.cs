using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// Equivalent to Aciton&lt;bool&gt;
    /// </summary>
    /// <param name="group"></param>
    /// <param name="val"></param>
    public delegate void OnPmpGroupExpandedEventHandler(PmpGroup group, bool val);

    /// <summary>
    /// Equivalent to Func&lt;IEnumerable&lt;PmpControl&gt;&gt;
    /// </summary>
    /// <returns></returns>
    public delegate PmpControl[] MakePmpControls();

}
