using Hymma.SolidTools.SolidAddins.Infrastructures;
using System.Collections.Generic;

namespace Hymma.SolidTools.SolidAddins
{
    public class AddinCommandBox: CommandTabBase
    {
        /// <summary>
        /// list of <see cref="AddinCommand"/> that this box contains
        /// </summary>
        public IEnumerable<AddinCommand> Commands { get; set; }
    }
}