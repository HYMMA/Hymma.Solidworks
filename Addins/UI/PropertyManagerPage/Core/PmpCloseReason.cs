using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTools.Addins
{

    /// <summary>
    /// defines the reason a property manager page was closed
    /// </summary>
    public enum PmpCloseReason
    {
        /// <summary>
        /// unknown
        /// </summary>
        UnknownReason = 0,

        /// <summary>
        /// user selected on the green check mark
        /// </summary>
        Okay = 1,

        /// <summary>
        /// user canceled by selecting the red cross mark
        /// </summary>
        Cancel = 2,

        /// <summary>
        /// main window closed first
        /// </summary>
        ParentClosed = 3,

        /// <summary>
        /// user closed
        /// </summary>
        Closed = 4,

        /// <summary>
        /// user pressed the escape button
        /// </summary>
        UserEscape = 5,

        /// <summary>
        /// user applied the changes
        /// </summary>
        Apply = 6,

        /// <summary>
        /// user selected the preview
        /// </summary>
        Preview = 7
    }
}
