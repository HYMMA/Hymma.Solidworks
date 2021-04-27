using SolidWorks.Interop.swconst;
using System.Collections.Generic;

namespace Hymma.SolidTools.SolidAddins.Infrastructures
{
    public class CommandTabBase
    {
        /// <summary>
        /// document types that this should be visible in 
        /// </summary>
        public IEnumerable<swDocumentTypes_e> Types { get; set; }
    }
}
