// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;

namespace Hymma.Solidworks.Addins.ContextMenus
{
    public sealed class ContextMenuRouter
    {
        private readonly Dictionary<string, ContextMenuItem> _items = new Dictionary<string, ContextMenuItem>(StringComparer.OrdinalIgnoreCase);

        public void Register(string token, ContextMenuItem item)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Context menu token cannot be empty.", nameof(token));
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _items[token] = item;
        }

        public void Unregister(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return;

            _items.Remove(token);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public void Execute(string token, ISldWorks solidworks)
        {
            if (!_items.TryGetValue(token ?? string.Empty, out var item))
                return;

            var context = new ContextMenuContext(solidworks);
            item.OnClick(context);
        }

        public int Enable(string token, ISldWorks solidworks)
        {
            if (!_items.TryGetValue(token ?? string.Empty, out var item))
                return 0;

            var context = new ContextMenuContext(solidworks);
            return item.CanExecute(context) ? 1 : 0;
        }
    }
}
