using Hymma.SolidTools.Addins;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTools.Fluent.Addins
{
    public class AddinFactory : IFluent
    {
        public PmpBase AddPmp()
        {
            return new AddinMaker()
        }
    }
}
