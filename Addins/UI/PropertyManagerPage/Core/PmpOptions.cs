using System;

namespace Hymma.SolidTools.Addins
{

    /// <summary>
    /// Options for PropertyManager pages. Bitmask.
    /// </summary>
    [Flags]
    public enum PmpOptions
    {
        /// <summary>
        /// Abort active command when PropertyManager page is displayed
        /// </summary>
        AbortCommands = 1024,

        /// <summary>
        /// include cancel button
        /// </summary>
        CancelButton = 2,

        /// <summary>
        ///  When a user presses the Esc key, the PropertyManager page will close the page; this enumerator only applies to pages that do not contain selection boxes
        /// </summary>
        CanEscapeCancel = 4096,
        /// <summary>
        /// include close dialog button
        /// </summary>
        CloseDialogButton = 8,
        /// <summary>
        /// Paint the PropertyManager page after control returns to SOLIDWORKS from the add-in's handler; may help eliminate any flickering of the PropertyManager page of the add-in changes the visibility of numerous controls on the PropertyManager page
        /// </summary>
        DisablePageBuildDuringHandlers = 32768,
        /// <summary>
        /// disable selection
        /// </summary>
        DisableSelection = 256,
        /// <summary>
        /// Controls whether a PropertyManager page should show its disabled selection list boxes so that they appear grayed out by coloring their backgrounds the same color as the divider box background. 
        /// </summary>
        /// <remarks>Disabled selection list boxes should be hidden; graying them out is an exception to SOLIDWORKS standard practice.</remarks>
        GrayOutDisabledSelectionListboxes = 65536,

        /// <summary>
        /// Enables processing of keystrokes while the PropertyManager page is displayed; disabled by default
        /// </summary>
        HandleKeystrokes = 8192,

        /// <summary>
        /// Specify the Locked option in the Options parameter when you create your PropertyManager page. It is important that when a handler (such as IPropertyManagerPage2Handler5::OnButtonPress or IPropertyManagerPage2Handler5::OnClose) is finished and control returns to SOLIDWORKS that the PropertyManager page is still there. If the PropertyManager page is not there, SOLIDWORKS might crash. Some methods try to close the PropertyManager page, but you can avoid this scenario by creating the PropertyManager page as Locked.
        /// </summary>
        LockedPage = 4,

        /// <summary>
        /// supports multiple tabs, will show arrows on top of property manager page
        /// </summary>
        MultiplePages = 16,

        /// <summary>
        /// ok or green check mark button
        /// </summary>
        OkayButton = 1,

        /// <summary>
        /// add a preview button, which is an eye on top of the page
        /// </summary>
        PreviewButton = 128,

        /// <summary>
        /// add a push pin button
        /// </summary>
        PushpinButton = 32,
        /// <summary>
        /// redo button
        /// </summary>
        RedoButton = 16384,

        /// <summary>
        /// If set, then show Select Chain:<br/>
        ///on the shortcut menu if a sketch entity is currently selected.<br/>
        ///- or -<br/>
        ///if nothing is selected, but the cursor is over a sketch entity when the right-mouse button was clicked.
        /// </summary>
        SupportsChainSelection = 131072,

        /// <summary>
        /// If set, then show Isolate item in right-mouse button menu of assembly components. This option must be used with LockedPage.
        /// </summary>
        SupportsIsolate = 262144,

        /// <summary>
        /// undo button
        /// </summary>
        UndoButton = 2048,

        /// <summary>
        /// Indicates that you want a What's New button to appear on your PropertyManager page
        /// </summary>
        WhatsNew = 512
    }
}
