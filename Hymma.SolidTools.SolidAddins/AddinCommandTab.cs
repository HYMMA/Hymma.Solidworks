using Hymma.SolidTools.SolidAddins.Infrastructures;
using System.Collections.Generic;
using SolidWorks.Interop.swconst;
namespace Hymma.SolidTools.SolidAddins
{
    public class AddinCommandTab : CommandTabBase
    {

        /// <summary>
        /// title of command tab 
        /// </summary>
        public string TabTitle { get; set; }

        /// <summary>
        /// list of command boxes that this command tab has <br/>
        /// these are separated by a '|' in solidworks tabs
        /// </summary>
        public IEnumerable<AddinCommandBox> CommandBoxes { get; set; }

    }
}
