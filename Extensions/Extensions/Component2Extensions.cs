// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="Component2"/>
    /// </summary>
    public static class Component2Extensions
    {
        /// <summary>
        /// Determines whether a component is a SolidWorks Toolbox part.
        /// Uses <c>ModelDoc2.Extension.ToolboxPartType</c> (SW 2014+) as the primary
        /// detection method, with a file-path fallback for lightweight or suppressed components.
        /// </summary>
        /// <param name="component">The component to check.</param>
        /// <returns><c>true</c> if the component is a Toolbox part; otherwise <c>false</c>.</returns>
        public static bool IsToolbox(this Component2 component)
        {
            return component.GetToolboxType() != swToolBoxPartType_e.swNotAToolboxPart;
        }

        /// <summary>
        /// Returns the <see cref="swToolBoxPartType_e"/> of a component.
        /// Uses <c>ModelDoc2.Extension.ToolboxPartType</c> (SW 2014+) as the primary
        /// detection method, with a file-path fallback for lightweight or suppressed components.
        /// </summary>
        /// <param name="component">The component to inspect.</param>
        /// <returns>
        /// The toolbox part type; <see cref="swToolBoxPartType_e.swNotAToolboxPart"/> if the
        /// component is not a toolbox part or the type cannot be determined.
        /// </returns>
        public static swToolBoxPartType_e GetToolboxType(this Component2 component)
        {
            if (component == null)
                return swToolBoxPartType_e.swNotAToolboxPart;

            try
            {
                var model = component.GetModelDoc2() as ModelDoc2;
                if (model != null)
                {
                    var extension = model.Extension;
                    if (extension != null)
                        return (swToolBoxPartType_e)extension.ToolboxPartType;
                }

                // Fallback for lightweight/suppressed components: check the file path
                var path = component.GetPathName();
                if (!string.IsNullOrEmpty(path) &&
                    path.Replace('/', '\\').ToUpperInvariant().Contains("\\TOOLBOX\\"))
                {
                    return swToolBoxPartType_e.swToolboxStandardPart;
                }
            }
            catch
            {
                // Ignore COM exceptions
            }

            return swToolBoxPartType_e.swNotAToolboxPart;
        }

        /// <summary>
        /// Determines whether a component is a SolidWorks library part.
        /// Detection is based on the file extension: library feature parts are saved
        /// with the <c>.sldlfp</c> extension. Parts that were <em>derived</em> from a
        /// library feature but saved as ordinary <c>.sldprt</c> files will not be detected.
        /// </summary>
        /// <param name="component">The component to check.</param>
        /// <returns><c>true</c> if the component file has the <c>.sldlfp</c> extension; otherwise <c>false</c>.</returns>
        public static bool IsLibraryPart(this Component2 component)
        {
            if (component == null)
                return false;

            try
            {
                // Try the loaded model path first
                var model = component.GetModelDoc2() as ModelDoc2;
                var path = model != null ? model.GetPathName() : component.GetPathName();
                return !string.IsNullOrEmpty(path) &&
                       path.EndsWith(".sldlfp", System.StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                // Ignore COM exceptions
            }

            return false;
        }
    }
}
