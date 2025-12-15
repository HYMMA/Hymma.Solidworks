// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using System.Drawing;
using System.IO;

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// Fluent builder for creating SolidWorks add-in user interfaces.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class provides a fluent API for building add-in UIs with a chainable, readable syntax.
    /// Start by calling <see cref="AddinUserInterfaceExtensions.GetBuilder"/> from your add-in class.
    /// </para>
    /// <para>
    /// The builder supports creating:
    /// </para>
    /// <list type="bullet">
    ///   <item><description>Command tabs with command groups and buttons</description></item>
    ///   <item><description>Property manager pages with tabs, groups, and controls</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// <para>Complete example of building an add-in UI:</para>
    /// <code>
    /// public override AddinUserInterface GetUserInterFace()
    /// {
    ///     var iconsPath = Path.Combine(
    ///         Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    ///         "MyAddinIcons");
    ///
    ///     return this.GetBuilder()
    ///         .AddCommandTab()
    ///             .WithTitle("My Tools")
    ///             .That()
    ///             .IsVisibleIn(new[] { swDocumentTypes_e.swDocPART, swDocumentTypes_e.swDocASSEMBLY })
    ///             .SetCommandGroup(1)
    ///                 .WithTitle("Tools", AddinConstants.SolidworksMenu.Tools)
    ///                 .WithIcon(Properties.Resources.ToolsIcon)
    ///                 .Has()
    ///                 .Commands(() => new AddinCommand[]
    ///                 {
    ///                     new AddinCommand("Run Tool", "Runs the tool", "Run Tool",
    ///                         Properties.Resources.RunIcon,
    ///                         nameof(RunToolCallback), nameof(CanRunTool))
    ///                 })
    ///             .SaveCommandGroup()
    ///         .SaveCommandTab()
    ///         .AddPropertyManagerPage("Tool Settings", SolidWorks)
    ///             .AddGroup("Options")
    ///                 .That()
    ///                 .HasTheseControls(new List&lt;IPmpControl&gt;
    ///                 {
    ///                     new PmpTextBox("Default value"),
    ///                     new PmpCheckBox() { Checked = true }
    ///                 })
    ///             .SaveGroup()
    ///         .SavePropertyManagerPage(out PropertyManagerPageX64 pmpFactory)
    ///         .WithIconsPath(new DirectoryInfo(iconsPath))
    ///         .Build();
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="AddinUserInterfaceExtensions.GetBuilder"/>
    /// <seealso cref="IFluentCommandTab"/>
    /// <seealso cref="IPmpUiModelFluent"/>
    public class AddinModelBuilder : AddinUserInterface, IAddinModelBuilder
    {
        private PmpUiModelFluent pmp;

        /// <summary>
        /// Adds a new command tab to the add-in's user interface.
        /// </summary>
        /// <returns>
        /// An <see cref="IFluentCommandTab"/> for fluent configuration of the command tab.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Command tabs appear in the SolidWorks ribbon. Each tab can contain multiple command groups,
        /// and each group contains command buttons.
        /// </para>
        /// <para>
        /// Call <see cref="IFluentCommandTab.SaveCommandTab"/> when finished configuring the tab.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// builder.AddCommandTab()
        ///     .WithTitle("My Tab")
        ///     .That()
        ///     .IsVisibleIn(new[] { swDocumentTypes_e.swDocPART })
        ///     .SetCommandGroup(1)
        ///         // ... configure command group
        ///     .SaveCommandGroup()
        /// .SaveCommandTab();
        /// </code>
        /// </example>
        public IFluentCommandTab AddCommandTab()
        {
            return new FluentCommandTab(this);
        }

        /// <summary>
        /// Creates a standalone property manager page tab.
        /// </summary>
        /// <param name="caption">The caption text displayed on the tab.</param>
        /// <param name="icon">Optional icon displayed next to the caption.</param>
        /// <returns>An <see cref="IPmpTabFluent"/> for fluent configuration.</returns>
        /// <remarks>
        /// Use this method when you need to create a tab separately from the main fluent chain.
        /// For most cases, use <see cref="IPmpUiModelFluent.AddTab(string, Bitmap)"/> instead.
        /// </remarks>
        public IPmpTabFluent CreatePropertyManagerPageTab(string caption, Bitmap icon = null)
        {
            return new PmpTabFluent(caption, icon);
        }

        /// <summary>
        /// Adds a property manager page to the add-in.
        /// </summary>
        /// <param name="title">The title displayed at the top of the property manager page.</param>
        /// <param name="solidworks">The SolidWorks application instance.</param>
        /// <returns>An <see cref="IPmpUiModelFluent"/> for fluent configuration of the page.</returns>
        /// <remarks>
        /// <para>
        /// Property manager pages appear in the left panel of SolidWorks. They can contain
        /// tabs, groups, and various controls like text boxes, selection boxes, and buttons.
        /// </para>
        /// <para>
        /// Call <see cref="IPmpUiModelFluent.SavePropertyManagerPage"/> when finished configuring.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// builder.AddPropertyManagerPage("My Settings", SolidWorks)
        ///     .AddGroup("Options")
        ///         .That()
        ///         .HasTheseControls(new List&lt;IPmpControl&gt;
        ///         {
        ///             new PmpTextBox("Enter value"),
        ///             new PmpButton("Apply", "Apply changes")
        ///         })
        ///         .SetExpansion(true)
        ///     .SaveGroup()
        ///     .OnClosing((pmp, reason) => { /* handle closing */ })
        ///     .OnAfterClose((pmp, reason) => { /* handle after close */ })
        /// .SavePropertyManagerPage(out PropertyManagerPageX64 pmpFactory);
        /// </code>
        /// </example>
        public IPmpUiModelFluent AddPropertyManagerPage(string title, ISldWorks solidworks)
        {
            pmp = new PmpUiModelFluent(solidworks, title)
            {
                Title = title,
                AddinModel = this
            };
            return pmp;
        }

        /// <summary>
        /// Sets the directory where add-in icons will be stored.
        /// </summary>
        /// <param name="iconsDir">
        /// The directory path. Should be a writable location like <c>LocalApplicationData</c>.
        /// </param>
        /// <returns>This builder instance for method chaining.</returns>
        /// <remarks>
        /// <para>
        /// This method must be called before <see cref="Build"/>. The directory will be created
        /// if it doesn't exist.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// var iconsPath = Path.Combine(
        ///     Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        ///     "MyAddinIcons");
        ///
        /// builder.WithIconsPath(new DirectoryInfo(iconsPath));
        /// </code>
        /// </example>
        public AddinModelBuilder WithIconsPath(DirectoryInfo iconsDir)
        {
            IconsRootDir = iconsDir;
            return this;
        }

        /// <summary>
        /// Builds and returns the configured <see cref="AddinUserInterface"/>.
        /// </summary>
        /// <returns>The fully configured <see cref="AddinUserInterface"/> instance.</returns>
        /// <exception cref="DirectoryNotFoundException">
        /// Thrown when <see cref="WithIconsPath"/> has not been called or was called with a null value.
        /// </exception>
        /// <remarks>
        /// <para>
        /// This method should be the last call in your fluent chain. It validates the configuration
        /// and returns the final <see cref="AddinUserInterface"/> that will be used by SolidWorks.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// return builder
        ///     .AddCommandTab()
        ///         // ... configure
        ///     .SaveCommandTab()
        ///     .WithIconsPath(new DirectoryInfo(iconsPath))
        ///     .Build();  // Returns AddinUserInterface
        /// </code>
        /// </example>
        public AddinUserInterface Build()
        {
            if (IconsRootDir is null)
            {
                throw new DirectoryNotFoundException("Icons directory is null. Call WithIconsPath() before Build().");
            }
            return this;
        }
    }
}