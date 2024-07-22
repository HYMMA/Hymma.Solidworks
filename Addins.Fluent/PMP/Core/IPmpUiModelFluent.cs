// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;
using System;
using System.Drawing;

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// create a new property manager page UI which hosts a number of controls
    /// </summary>
    public interface IPmpUiModelFluent : IFluent
    {

        /// <summary>
        /// Add a tab to this property manager page
        /// </summary>
        /// <param name="caption">caption that appears on the tab </param>
        /// <param name="icon">The Bitmap argument allows you to place a bitmap before the text on the tab<br/>
        /// Any portions of the bitmap that are RGB(255,255,255) will be transparent, letting the tab background show through. this will be resized to 16x18 pixels</param>
        /// <returns></returns>
        IPmpTabFluent AddTab(string caption, Bitmap icon = null);

        /// <summary>
        /// adds a tab to this property manager page
        /// </summary>
        /// <returns><see cref="IPmpUiModelFluent"/></returns>
        /// <remarks>use this method to share a tab between different property manager pages</remarks>
        IPmpUiModelFluent AddTab<T>() where T : PmpTab, new();

        /// <summary>
        /// adds a tab to this property manager page
        /// </summary>
        /// <param name="tab">the tab to add to this property manager page</param>
        /// <returns></returns>
        IPmpUiModelFluent AddTab(PmpTab tab);

        /// <summary>
        /// Add a group that hosts controls in a property manager page 
        /// </summary>
        /// <param name="caption">caption of the group as appears in solidworks</param>
        /// <returns></returns>
        IPmpGroupFluent AddGroup(string caption);

        /// <summary>
        /// Add a checkable group to this property manager page
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        IPmpGroupFluentCheckable AddCheckableGroup(string caption);

        /// <summary>
        /// bitwise option as defined in <see cref="PmpOptions"/> default has okay, cancel, push-pin buttons and page build is disabled during handlers
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        IPmpUiModelFluent WithPmpOptions(PmpOptions options);

        /// <summary>
        /// define void() to invoke after the property manager page is closed
        /// </summary>
        /// <param name="doThis">void to invoke</param>
        /// <returns></returns>
        IPmpUiModelFluent OnAfterClose(Action<PmpUiModel> doThis);

        /// <summary>
        ///Processes a keystroke that occurred on this PropertyManager page
        /// </summary>
        /// <param name="doThis">Action that takes two params, first one is the sender which would be the <see cref="PmpUiModel"/> and second one is the event argument for this keystroke</param>
        /// <remarks><see cref="WithPmpOptions(PmpOptions)"/> should have the <see cref="PmpOptions.HandleKeystrokes"/> for this action to work</remarks>
        IPmpUiModelFluent OnKeyStroke(EventHandler<PmpKeyStrokeEventArgs> doThis);

        /// <summary>
        /// Action to invoke while the property manger page is closing
        /// </summary>
        /// <param name="doThis"></param>
        /// <returns></returns>
        IPmpUiModelFluent OnClosing(Action<PmpUiModel, PmpCloseReason> doThis);

        /// <summary>
        /// action to take after the property manager page is activated
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IPmpUiModelFluent OnAfterActivation(Action<PmpUiModel> action);

        /// <summary>
        ///Sets the cursor after a selection is made in the SOLIDWORKS graphics area.
        /// </summary>
        IPmpUiModelFluent WithCursorStyle(PmpCursorStyles cursorStyles);

        /// <summary>
        /// Sets the message in this PropertyManager page.  
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="message"></param>
        /// <param name="messageVisibility"></param>
        /// <param name="pageMessageExpanded"></param>
        /// <returns></returns>
        IPmpUiModelFluent WithMessage(string caption, string message, swPropertyManagerPageMessageVisibility messageVisibility, swPropertyManagerPageMessageExpanded pageMessageExpanded);

        /// <summary>
        /// The recommended size for bitmaps is a square from 18- to 22-cells wide.
        /// </summary>
        /// <remarks>
        /// The recommended size for bitmaps is a square from 18- to 22-cells wide. However, the bitmap can be any size, as long as it fits on the title bar.
        ///The bitmap appears transparent by mapping any white(RGB(255,255,255)) cells to the current PropertyManager page title bar background color.Remember the special use of this color as you design your bitmap.
        /// </remarks>
        IPmpUiModelFluent WithIconInTitle(Bitmap icon);

        /// <summary>
        ///  Adds a menu item to the pop-up menu for this PropertyManager page. that appears in  the right mouse menu button while the property manager page is displayed 
        /// </summary>
        IPmpUiModelFluent AddMenuPopUpItem(PopUpMenuItem item);

        ///<summary>
        /// builds this property manager page and adds it to the <see cref="AddinUserInterface"/> <br/>
        /// use the <see cref="PropertyManagerPageX64"/>.Show() method in a <see cref="AddinCommand"/> callback function so users of your addin can actually see the property manger page once they clicked on a button
        /// </summary>
        /// <returns></returns>
        IAddinModelBuilder SavePropertyManagerPage(out PropertyManagerPageX64 propertyManagerPage);
    }
}