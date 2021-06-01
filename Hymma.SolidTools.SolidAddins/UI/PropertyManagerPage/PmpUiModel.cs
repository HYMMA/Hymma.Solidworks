using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// a wrapper for solidworks property manager page UI
    /// </summary>
    public class PmpUiModel 
    {
        /// <summary>
        /// bitwise option as defined in <see cref="swPropertyManagerPageOptions_e"/> default is 35
        /// </summary>
        public int Options { get; set; } = 35;

        /// <summary>
        /// solidworks group boxes that contain solidworks pmp controllers
        /// </summary>
        public  List<SwGroupBox> SwBoxes { get; set; }

        /// <summary>
        /// a title for this property manager page
        /// </summary>
        public string Title { get; set; }
        
    }
}
