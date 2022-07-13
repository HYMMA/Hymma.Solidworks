// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;

namespace Hymma.Solidworks.Addins
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
