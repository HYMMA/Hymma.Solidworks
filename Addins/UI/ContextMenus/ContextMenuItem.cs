// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.Drawing;

namespace Hymma.Solidworks.Addins.ContextMenus
{
    /// <summary>
    /// Represents a single context menu item.
    /// </summary>
    public sealed class ContextMenuItem
    {
        /// <summary>
        /// Display text for the menu item.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Tooltip for the menu item.
        /// </summary>
        public string ToolTip { get; }

        /// <summary>
        /// Determines whether the item is enabled for the current selection context.
        /// </summary>
        public Func<ContextMenuContext, bool> CanExecute { get; }

        /// <summary>
        /// Invoked when the menu item is clicked.
        /// </summary>
        public Action<ContextMenuContext> OnClick { get; }

        /// <summary>
        /// Optional icon for the menu item.
        /// </summary>
        public Bitmap Icon { get; }

        public ContextMenuItem(string title, Action<ContextMenuContext> onClick, Func<ContextMenuContext, bool> canExecute = null, string toolTip = null, Bitmap icon = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Menu item title cannot be empty.", nameof(title));
            if (onClick == null)
                throw new ArgumentNullException(nameof(onClick));

            Title = title;
            OnClick = onClick;
            CanExecute = canExecute ?? (_ => true);
            ToolTip = toolTip ?? title;
            Icon = icon;
        }
    }
}
