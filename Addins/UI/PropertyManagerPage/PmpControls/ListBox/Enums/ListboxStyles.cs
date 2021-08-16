using System;

namespace Hymma.SolidTools.Addins
{

    /// <summary>
    /// PropertyManager page list box styles. Bitmask. 
    /// </summary>
    [Flags]
    public enum ListboxStyles
    {
        /// <summary>
        /// allow multiple selection in the list box
        /// </summary>
        AllowMultiSelect = 4,

        /// <summary>
        /// no integral height
        /// </summary>
        NoIntegralHeight = 2,

        /// <summary>
        /// Sort the list in alphabetically
        /// </summary>
        SortAlphabetically = 1
    }
}
