// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.Solidworks.Addins.ContextMenus
{
    /// <summary>
    /// Strongly-typed wrappers for SolidWorks selection targets used by context menus.
    /// Uses IFrame.AddMenuPopupIcon2 which works for both graphics area and feature tree selections.
    /// </summary>
    public sealed class ContextMenuTarget
    {
        /// <summary>
        /// Gets the selection types that trigger this context menu.
        /// </summary>
        public IReadOnlyList<swSelectType_e> SelectionTypes { get; }

        private ContextMenuTarget(IEnumerable<swSelectType_e> selectionTypes)
        {
            SelectionTypes = selectionTypes.ToArray();
        }

        /// <summary>
        /// Creates a context menu target for the specified selection types.
        /// </summary>
        public static ContextMenuTarget From(params swSelectType_e[] selectionTypes)
            => new ContextMenuTarget(selectionTypes);

        /// <summary>
        /// Context menu for features selected in the FeatureManager design tree.
        /// Works for Hole Wizard, Boss-Extrude, Cut-Extrude, and other features.
        /// </summary>
        public static readonly ContextMenuTarget Features = new ContextMenuTarget(new[]
        {
            swSelectType_e.swSelBODYFEATURES,
            swSelectType_e.swSelSWIFTFEATURES,
            swSelectType_e.swSelINCONTEXTFEAT,
            swSelectType_e.swSelINCONTEXTFEATS
        });

        /// <summary>
        /// Context menu for faces selected in the graphics area.
        /// </summary>
        public static readonly ContextMenuTarget Faces = new ContextMenuTarget(new[] { swSelectType_e.swSelFACES });

        /// <summary>
        /// Context menu for edges selected in the graphics area.
        /// </summary>
        public static readonly ContextMenuTarget Edges = new ContextMenuTarget(new[] { swSelectType_e.swSelEDGES });

        /// <summary>
        /// Context menu for vertices selected in the graphics area.
        /// </summary>
        public static readonly ContextMenuTarget Vertices = new ContextMenuTarget(new[] { swSelectType_e.swSelVERTICES });

        /// <summary>
        /// Context menu for sketch segments.
        /// </summary>
        public static readonly ContextMenuTarget SketchSegments = new ContextMenuTarget(new[] { swSelectType_e.swSelSKETCHSEGS });

        /// <summary>
        /// Context menu for sketch points.
        /// </summary>
        public static readonly ContextMenuTarget SketchPoints = new ContextMenuTarget(new[] { swSelectType_e.swSelSKETCHPOINTS });

        /// <summary>
        /// Context menu for sketch profiles (sketches in the tree).
        /// </summary>
        public static readonly ContextMenuTarget SketchProfiles = new ContextMenuTarget(new[] { swSelectType_e.swSelSKETCHES });

        /// <summary>
        /// Context menu for generic browser/tree items.
        /// </summary>
        public static readonly ContextMenuTarget FeatureTreeItems = new ContextMenuTarget(new[] { swSelectType_e.swSelBROWSERITEM });

        /// <summary>
        /// Context menu for components in assemblies.
        /// </summary>
        public static readonly ContextMenuTarget Components = new ContextMenuTarget(new[] { swSelectType_e.swSelCOMPONENTS });

        /// <summary>
        /// Context menu for solid bodies.
        /// </summary>
        public static readonly ContextMenuTarget SolidBodies = new ContextMenuTarget(new[] { swSelectType_e.swSelSOLIDBODIES });

        /// <summary>
        /// Context menu for surface bodies.
        /// </summary>
        public static readonly ContextMenuTarget SurfaceBodies = new ContextMenuTarget(new[] { swSelectType_e.swSelSURFACEBODIES });
    }
}
