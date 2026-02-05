// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;

namespace Hymma.Solidworks.Addins.ContextMenus
{
    /// <summary>
    /// Provides context information for context menu callbacks.
    /// </summary>
    public sealed class ContextMenuContext
    {
        /// <summary>
        /// Gets the SolidWorks application instance.
        /// </summary>
        public ISldWorks Solidworks { get; }

        /// <summary>
        /// Gets the active model document, if any.
        /// </summary>
        public ModelDoc2 Model { get; }

        /// <summary>
        /// Gets the selection manager for the active document, if any.
        /// </summary>
        public ISelectionMgr SelectionManager { get; }

        /// <summary>
        /// Gets the number of selected objects in the active document.
        /// </summary>
        public int SelectionCount { get; }

        internal ContextMenuContext(ISldWorks solidworks)
        {
            Solidworks = solidworks;
            Model = solidworks?.ActiveDoc as ModelDoc2;
            SelectionManager = Model?.SelectionManager as ISelectionMgr;
            SelectionCount = SelectionManager?.GetSelectedObjectCount2(-1) ?? 0;
        }
    }
}
