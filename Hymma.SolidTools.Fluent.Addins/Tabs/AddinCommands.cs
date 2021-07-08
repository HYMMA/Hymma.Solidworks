using Hymma.SolidTools.Addins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTools.Fluent.Addins
{
    public class AddinCommands
    {
        public AddinCommands(AddinCmdGrp group)
        {
            this.Group = group;
        }

        public AddinCmdGrp Group { get; }

        public IAddinCmdGroup Commands(Func<IEnumerable<AddinCmdBase>> action)
        {
            Group.Commands.ToList().Add(action.Invoke());
            return Group;
        }

        public void Commands(IEnumerable<IAddinCommand> commands)
        {
            Group.
        }
    }
}

