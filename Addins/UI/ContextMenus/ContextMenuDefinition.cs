// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.Solidworks.Addins.ContextMenus
{
    /// <summary>
    /// Defines a context menu and its items for one or more selection targets.
    /// </summary>
    public sealed class ContextMenuDefinition
    {
        public string Title { get; }
        public IReadOnlyList<ContextMenuTarget> Targets { get; }
        public IReadOnlyList<ContextMenuItem> Items { get; }

        public ContextMenuDefinition(string title, IEnumerable<ContextMenuTarget> targets, IEnumerable<ContextMenuItem> items)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Context menu title cannot be empty.", nameof(title));
            if (targets == null)
                throw new ArgumentNullException(nameof(targets));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            var targetList = targets.ToList();
            if (targetList.Count == 0)
                throw new ArgumentException("Context menu must have at least one target.", nameof(targets));

            var itemList = items.ToList();
            if (itemList.Count == 0)
                throw new ArgumentException("Context menu must have at least one item.", nameof(items));

            Title = title;
            Targets = targetList;
            Items = itemList;
        }
    }
}
