// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins.Core;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using WeakEvent;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// allows access to a pop up menu item
    /// </summary>
    public class PopUpMenuItem
    {
        /// <summary>
        /// Adds a menu item to the pop-up menu
        /// </summary>
        /// <param name="itemText">Text for the item</param>
        /// <param name="hint">Text displayed in the SOLIDWORKS status bar when the user moves the cursor over this pop-up menu item</param>
        /// <param name="documentTypes">Document types for which this pop-up menu item is displayed as defined in <see cref="swDocumentTypes_e"/></param>
        public PopUpMenuItem(string itemText, string hint, swDocumentTypes_e documentTypes)
        {
            this.ItemText = itemText; this.Hint = hint; this.DocumentType = documentTypes;
        }

        /// <summary>
        /// Determines which item was selected when the user selects a pop-up menu item. 
        /// </summary>
        public event EventHandler<EventArgs> Pressed { add
            {
                _pressedEvents.Subscribe(this,value);
            }
            remove
            {
                _pressedEvents.Unsubscribe(value);
            }
        }

        /// <summary>
        ///  When Windows attempts to select or deselected and enable or disable the pop-up menu item, SOLIDWORKS calls this method to get the state of the menu item from the add-in. 
        /// </summary>
        /// State of the specified unique user-defined pop-up menu item:
        /// 0 - Not selected(i.e., not checked) and disabled(i.e., grayed out)
        /// 1 - Not selected and enabled
        /// 2 - Selected(i.e., checked) and disabled
        /// 3 - Selected and enabled
        public event EventHandler<int> Updated { add
            {
                _updatedEvents.Subscribe(this,value);
            }
            remove
            {
                _updatedEvents.Unsubscribe(value);
            }
        }

        /// <summary>
        /// item text in the menu
        /// </summary>
        public string ItemText { get; set; }

        /// <summary>
        /// text that appears on the status bar once user hovers over this menu item
        /// </summary>
        public string Hint { get; set; }

        /// <summary>
        /// document types that this item should appear in
        /// </summary>
        public swDocumentTypes_e DocumentType { get; set; }
        
        /// <summary>
        /// id of this menu pop up item that's assigned to it once the item was created
        /// </summary>
        public int Id { get;internal set; }

        #region call backs
        readonly WeakEventSource<int> _updatedEvents = new WeakEventSource<int>();
        readonly WeakEventSource<EventArgs> _pressedEvents = new WeakEventSource<EventArgs>();
        internal void UpdatedCallback(int u) => _updatedEvents?.Raise(this,u);
        internal void PressedCallback() => _pressedEvents?.Raise(this,EventArgs.Empty);

        /// <summary>
        /// Unsubscribes all events
        /// </summary>
        public void UnsubscribeFromEvents()
        {
            _pressedEvents.ClearHandlers();
            _updatedEvents.ClearHandlers();
            //Updated?.GetInvocationList()?.ToList()?.ForEach(d=> Updated -= (Action<int>)d);
            //Pressed?.GetInvocationList()?.ToList()?.ForEach(d => Pressed -= (Action)d);
        }
        #endregion
    }
}
