// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System.Drawing;

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// an interface to define a command group in solidworks
    /// </summary>
    public interface IFluentCommandGroup
    {
        /// <summary>
        /// add more context
        /// </summary>
        /// <returns></returns>
        IFluentCommandGroup And();

        /// <summary>
        /// Adds commands to this command group
        /// </summary>
        AddinCommands Has();

        /// <summary>
        /// To add a CommandGroup to an existing SOLIDWORKS menu, specify the name of the parent menu here.<br/>
        /// <param name="title">the title as appears in solidworks menu</param>
        /// <param name="menu">solidworks menus as defined in <see cref="Constants.SolidworksMenu"/></param>
        /// </summary>
        IFluentCommandGroup WithTitle(string title, string menu = Constants.SolidworksMenu.Tools);

        /// <summary>
        /// tooltip for this command group that appears under the button in SOLIDWORKS tab
        /// </summary>
        /// <param name="toolTip"></param>
        IFluentCommandGroup WithToolTip(string toolTip);

        /// <summary>
        /// provide a description for your users
        /// </summary>
        /// <param name="description"></param>
        IFluentCommandGroup WithDescription(string description);

        /// <summary>
        /// Provide a hint for your users
        /// </summary>
        /// <param name="hint"></param>
        IFluentCommandGroup WithHint(string hint);

        /// <summary>
        /// define an icon for this command group. it will be next to the command group name inside the command manager
        /// </summary>
        /// <param name="bitmap"></param>
        IFluentCommandGroup WithIcon(Bitmap bitmap);

/// <summary>
/// registers this command group into solidworks UI
/// </summary>
/// <returns></returns>
        IFluentCommandTab SaveCommandGroup();
    }
}
