using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a wrapper for solidworks property manager page UI
    /// </summary>
    public class PropertyManagerPageUIBase
    {
        /// <summary>
        /// base class for making proeprty manager page UI models and controls
        /// </summary>
        /// <param name="solidworks"></param>
        public PropertyManagerPageUIBase(ISldWorks solidworks)
        {
            this.Solidworks = solidworks;
        }

        /// <summary>
        /// bitwise option as defined in <see cref="PmpOptions"/> default is 32807
        /// </summary>
        public int Options { get; set; } = 32807;

        /// <summary>
        /// solidworks group boxes that contain solidworks pmp controllers
        /// </summary>
        public List<PMPGroup> PmpGroups { get; internal set; } = new List<PMPGroup>();

        /// <summary>
        /// a title for this property manager page
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// return a specific control type based on its id
        /// </summary>
        /// <param name="id">id of control to return</param>
        /// <returns></returns>
        public IPmpControl GetControl(int id)
        {
            var control = PmpGroups?
                .SelectMany(g => g.Controls)
                .Where(ch => ch.Id == id).FirstOrDefault();
            return control;
        }

        /// <summary>
        /// get all controls of type T in this propery manger page
        /// </summary>
        /// <typeparam name="T">type of control to return</typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetControls<T>() where T : IPmpControl
        {
            var controls = PmpGroups?
                .SelectMany(g => g.Controls)
                .Where(c => c is T).Cast<T>();
            return controls;
        }

        /// <summary>   
        /// methode to invoke once user clicked on question mark button on property manager page
        /// </summary>
        public Func<bool> OnHelp { get; set; }

        /// <summary>
        /// methode to invoke after the propety manager page is actaved
        /// </summary>
        public Action OnAfterActivation { get; set; }

        /// <summary>
        /// methode to invoke after the propety manager page is closed
        /// </summary>
        public Action OnAfterClose { get; set; }

        /// <summary>
        /// methode to invoke while the property manager page is closing
        /// </summary>
        public Action<PMPCloseReason> OnClose { get; set; }

        /// <summary>
        /// methode to invoke when user goes to the previous page of a property manager page
        /// </summary>
        public Func<bool> OnPreviousPage { get; set; }


        /// <summary>
        /// methode to invoke when user goes to next page of a property manager page
        /// </summary>
        public Func<bool> OnNextPage { get; set; }

        /// <summary>
        /// methode to invoke when user previews the results 
        /// </summary>
        public Func<bool> OnPreview { get; set; }

        /// <summary>
        /// methode to invoke when user selects on Wahts new button
        /// </summary>
        public Action OnWhatsNew { get; set; }

        /// <summary>
        /// methode to invoke when user calls undo (ctrl+z)
        /// </summary>
        public Action OnUndo { get; set; }
        /// <summary>
        /// methode to invoke when user Re-do something (ctrl+y)
        /// </summary>
        public Action OnRedo { get; set; }

        /// <summary>
        /// method to invoke when user clickes on a tab
        /// </summary>
        public Func<int, bool> OnTabClicked { get; set; }

        /// <summary>
        /// solidworks object
        /// </summary>
        public ISldWorks Solidworks { get; }
    }

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


    /// <summary>
    /// defines the reason a property manager page was closed
    /// </summary>
    public enum PMPCloseReason
    {
        /// <summary>
        /// unknown
        /// </summary>
        UnknownReason = 0,

        /// <summary>
        /// user selected on the green check mark
        /// </summary>
        Okay = 1,

        /// <summary>
        /// user canceled by selecting the red cross mark
        /// </summary>
        Cancel = 2,

        /// <summary>
        /// main window closed first
        /// </summary>
        ParentClosed = 3,

        /// <summary>
        /// user closed
        /// </summary>
        Closed = 4,

        /// <summary>
        /// user pressed the escape button
        /// </summary>
        UserEscape = 5,

        /// <summary>
        /// user applied the changes
        /// </summary>
        Apply = 6,

        /// <summary>
        /// user selected the preview
        /// </summary>
        Preview = 7
    }

}



