// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System.Collections.Generic;
using System.IO;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// Represents the complete user interface definition for a SolidWorks add-in,
    /// including command tabs, property manager pages, and icon storage configuration.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class serves as a container for all UI elements that your add-in will register
    /// with SolidWorks. It is returned by <see cref="AddinMaker.GetUserInterFace"/> and
    /// processed during add-in initialization.
    /// </para>
    /// <para>
    /// The <see cref="IconsRootDir"/> property is required and must point to a writable
    /// directory where the framework will store processed icon files.
    /// </para>
    /// </remarks>
    /// <example>
    /// <para>Creating an AddinUserInterface:</para>
    /// <code>
    /// public override AddinUserInterface GetUserInterFace()
    /// {
    ///     var iconsPath = Path.Combine(
    ///         Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    ///         "MyAddinIcons");
    ///
    ///     return new AddinUserInterface
    ///     {
    ///         IconsRootDir = new DirectoryInfo(iconsPath),
    ///         CommandTabs = new List&lt;AddinCommandTab&gt;
    ///         {
    ///             new MyCommandTab()
    ///         },
    ///         PropertyManagerPages = new List&lt;PropertyManagerPageX64&gt;
    ///         {
    ///             new MyPropertyManagerPage(_solidworks)
    ///         }
    ///     };
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="AddinMaker"/>
    /// <seealso cref="AddinCommandTab"/>
    /// <seealso cref="PropertyManagerPageX64"/>
    public class AddinUserInterface
    {
        /// <summary>
        /// Gets or sets the root directory where add-in icons will be stored.
        /// </summary>
        /// <value>
        /// A <see cref="DirectoryInfo"/> pointing to a writable directory.
        /// </value>
        /// <remarks>
        /// <para>
        /// This directory is used by the framework to store processed icon files in various
        /// sizes required by SolidWorks. The directory will be created if it doesn't exist.
        /// </para>
        /// <para>
        /// <b>Recommended location:</b> Use <c>Environment.SpecialFolder.LocalApplicationData</c>
        /// to ensure the add-in has write permissions.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// IconsRootDir = new DirectoryInfo(
        ///     Path.Combine(Environment.GetFolderPath(
        ///         Environment.SpecialFolder.LocalApplicationData), "MyAddinIcons"));
        /// </code>
        /// </example>
        public DirectoryInfo IconsRootDir { get; set; }

        /// <summary>
        /// Gets the unique identifier (cookie) assigned to this add-in by SolidWorks.
        /// </summary>
        /// <value>
        /// An integer cookie that uniquely identifies this add-in instance within SolidWorks.
        /// </value>
        /// <remarks>
        /// This value is assigned by SolidWorks during the <c>ConnectToSW</c> call and is used
        /// internally by the framework. You typically don't need to access this directly.
        /// </remarks>
        public int Id { get; internal set; }

        /// <summary>
        /// Gets or sets the list of property manager pages for this add-in.
        /// </summary>
        /// <value>
        /// A list of <see cref="PropertyManagerPageX64"/> instances. Defaults to an empty list.
        /// </value>
        /// <remarks>
        /// <para>
        /// Property manager pages are custom panels that appear in the SolidWorks property manager
        /// area (left side panel). They provide a standard way to collect user input and display
        /// options for your add-in's features.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// PropertyManagerPages = new List&lt;PropertyManagerPageX64&gt;
        /// {
        ///     new MyPropertyManagerPage(_solidworks)
        /// };
        /// </code>
        /// </example>
        /// <seealso cref="PropertyManagerPageX64"/>
        /// <seealso cref="PmpUiModel"/>
        public List<PropertyManagerPageX64> PropertyManagerPages { get; set; } = new List<PropertyManagerPageX64>();

        /// <summary>
        /// Gets or sets the list of command tabs for this add-in.
        /// </summary>
        /// <value>
        /// A list of <see cref="AddinCommandTab"/> instances. Defaults to an empty list.
        /// </value>
        /// <remarks>
        /// <para>
        /// Command tabs appear in the SolidWorks ribbon/toolbar area. Each tab contains one or more
        /// command groups, which in turn contain individual command buttons.
        /// </para>
        /// <para>
        /// Tabs can be configured to appear only in specific document types (parts, assemblies, drawings).
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// CommandTabs = new List&lt;AddinCommandTab&gt;
        /// {
        ///     new AddinCommandTab
        ///     {
        ///         Title = "My Tools",
        ///         DocTypes = new[] { swDocumentTypes_e.swDocPART, swDocumentTypes_e.swDocASSEMBLY },
        ///         CommandGroup = new MyCommandGroup()
        ///     }
        /// };
        /// </code>
        /// </example>
        /// <seealso cref="AddinCommandTab"/>
        /// <seealso cref="AddinCommandGroup"/>
        public List<AddinCommandTab> CommandTabs { get; set; } = new List<AddinCommandTab>();
    }
}
