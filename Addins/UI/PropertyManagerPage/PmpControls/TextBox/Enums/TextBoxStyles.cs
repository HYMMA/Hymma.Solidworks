using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// PropertyManager page textbox styles. Bitmask. 
    /// </summary>
    [Flags]
    public enum TexTBoxStyles
    {
        /// <summary>
        /// Do not send notification every time a character in the text box changes; instead, only send notification when text box loses focus after the user has made all changes
        /// </summary>
        NotifyOnlyWhenFocusLost = 1,

        /// <summary>
        /// text box will be read only
        /// </summary>
        ReadOnly = 2,

        /// <summary>
        /// remove borders
        /// </summary>
        NoBorder = 4,


        /// <summary>
        /// multiple lines
        /// </summary>
        Multiline = 8
    }
}
