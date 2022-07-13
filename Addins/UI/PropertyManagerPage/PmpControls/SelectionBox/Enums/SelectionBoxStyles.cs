// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;

namespace Hymma.Solidworks.Addins
{

    /// <summary>
    /// styles for selection box in a property manager page
    /// </summary>
    [Flags]
    public enum SelectionBoxStyles
    {
        /// <summary>
        /// default selection box taht matches most soldiworks commands (recommended so your users dont feel alienated)
        /// </summary>
        Default = 0,
        /// <summary>
        /// Specifies that the selection box has a scroll bar so that interactive users can scroll through the list of items
        /// </summary>
        HScroll = 1,

        /// <summary>
        /// Specifies that you can select multiple items in the selection box
        /// </summary>
        MultipleItemSelect = 4,

        /// <summary>
        /// Specifies that selection listbox has up and down arrows so that interactive users can move items in the list up or down
        /// </summary>
        UpAndDownButtons = 2,

        /// <summary>
        /// Specifies that you want a notification sent when a user changes the selected item in a listbox or selection listbox
        /// </summary>
        WantListboxSelectionChanged = 8
    }
}
