using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// a wrapper for solidworks property manager page groups
    /// </summary>
    public class SwGroupBox
    {
        /// <summary>
        /// identifier for this group box in Property manager page
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// caption
        /// </summary>
        public string Caption { get; set; }
        
        /// <summary>
        /// bitwise options as defined by <see cref="swAddGroupBoxOptions_e"/> default values correspond to a group that is expanded and is set to be visible
        /// </summary>
        public swAddGroupBoxOptions_e Options { get; set; }
            = swAddGroupBoxOptions_e.swGroupBoxOptions_Expanded | swAddGroupBoxOptions_e.swGroupBoxOptions_Visible;
        
        /// <summary>
        /// a list of solidworks controllers
        /// </summary>
        public List<SwPMPControl> Controls { get; set;}

    }
}