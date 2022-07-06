// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;

namespace Hymma.Solidworks.Addins
{

    /// <summary>
    /// styles for a label in property manager pages
    /// </summary>
    [Flags]
    public enum LabelStyles
    {
        /// <summary>
        /// align to center
        /// </summary>
        CenterText = 2,

        /// <summary>
        /// aling to the left (the defual value)
        /// </summary>
        LeftText = 1,

        /// <summary>
        /// align to the right
        /// </summary>
        RightText = 4,

        /// <summary>
        /// with shadow
        /// </summary>
        Sunken = 8,
    }
}
