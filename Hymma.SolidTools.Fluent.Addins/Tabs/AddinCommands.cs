using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTools.Fluent.Addins
{
    internal class AddinCommands
    {
        public AddinCommands()
        {
                
        }
        public void Commands(Action<IEnumerable<IAddinCommand>> action)
        {
            action.Invoke()
        }

        public void Commands(IEnumerable<IAddinCommand> commands)
        {

        }
    }
}
