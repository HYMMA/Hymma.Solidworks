// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// Provides extension methods for creating add-in user interfaces using the fluent API.
    /// </summary>
    /// <remarks>
    /// This class is the entry point for the fluent API. Use <see cref="GetBuilder"/> to start
    /// building your add-in's user interface with a chainable, readable syntax.
    /// </remarks>
    public static class AddinUserInterfaceExtensions
    {
        /// <summary>
        /// Creates a new <see cref="AddinModelBuilder"/> to build the add-in's user interface
        /// using the fluent API pattern.
        /// </summary>
        /// <param name="addinMaker">The add-in instance to build the UI for.</param>
        /// <returns>A new <see cref="AddinModelBuilder"/> instance for fluent configuration.</returns>
        /// <remarks>
        /// <para>
        /// This is the entry point for the fluent API. Chain method calls to define command tabs,
        /// command groups, property manager pages, and other UI elements.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>Using the fluent API to build an add-in UI:</para>
        /// <code>
        /// public override AddinUserInterface GetUserInterFace()
        /// {
        ///     var builder = this.GetBuilder();
        ///     return builder
        ///         .AddCommandTab()
        ///             .WithTitle("My Tools")
        ///             .That()
        ///             .IsVisibleIn(new[] { swDocumentTypes_e.swDocPART })
        ///             .SetCommandGroup(1)
        ///                 .WithTitle("My Commands")
        ///                 .WithIcon(Properties.Resources.MyIcon)
        ///                 .Has()
        ///                 .Commands(() => new AddinCommand[]
        ///                 {
        ///                     new AddinCommand("My Command", "Hint", "Tooltip",
        ///                         Properties.Resources.CmdIcon,
        ///                         nameof(MyCallback), nameof(MyEnabler))
        ///                 })
        ///             .SaveCommandGroup()
        ///         .SaveCommandTab()
        ///         .WithIconsPath(new DirectoryInfo(iconsPath))
        ///         .Build();
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="AddinModelBuilder"/>
        /// <seealso cref="IFluentCommandTab"/>
        public static AddinModelBuilder GetBuilder(this AddinMaker addinMaker)
        {
            return new AddinModelBuilder();
        }
    }
}
