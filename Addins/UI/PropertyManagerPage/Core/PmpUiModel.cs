// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using WeakEvent;
using Environment = System.Environment;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a wrapper for solidworks property manager page UI
    /// </summary>
    public class PmpUiModel : IWrapSolidworksObject<IPropertyManagerPage2>
    {
        private PmpCloseReason closeReason;

        readonly WeakEventSource<bool> helpClickedSrc = new WeakEventSource<bool>();
        readonly WeakEventSource<EventArgs> afterActivationSrc = new WeakEventSource<EventArgs>();

        readonly WeakEventSource<PmpCloseEventArgs> afterCloseSrc = new WeakEventSource<PmpCloseEventArgs>();
        readonly WeakEventSource<PmpCloseEventArgs> closingSrc = new WeakEventSource<PmpCloseEventArgs>();
        readonly WeakEventSource<bool> previousPageClickedSrc = new WeakEventSource<bool>();
        readonly WeakEventSource<bool> nextPageClickedSrc = new WeakEventSource<bool>();
        readonly WeakEventSource<bool> previewSrc = new WeakEventSource<bool>();
        readonly WeakEventSource<EventArgs> whatsNewClickedSrc = new WeakEventSource<EventArgs>();
        readonly WeakEventSource<EventArgs> undoClickedSrc = new WeakEventSource<EventArgs>();
        readonly WeakEventSource<EventArgs> redoClickedSrc = new WeakEventSource<EventArgs>();
        readonly WeakEventSource<EventArgs> registeringSrc = new WeakEventSource<EventArgs>();
        readonly WeakEventSource<PmpTabClickedEventArgs> tabClickedSrc = new WeakEventSource<PmpTabClickedEventArgs>();
        readonly WeakEventSource<PmpKeyStrokeEventArgs> keyStrokeSrc = new WeakEventSource<PmpKeyStrokeEventArgs>();
        #region default constructor

        /// <summary>
        /// base class for making property manager page UI models and controls
        /// </summary>
        /// <param name="solidworks"></param>
        /// <param name="title">the title of the property manager page that appears on top of it in solidWORK</param>
        ///<remarks>the title of the property manger page has to be a unique value for each add-in</remarks>
        public PmpUiModel(ISldWorks solidworks, string title)
        {
            this.Solidworks = solidworks;
            Title = title;
            Closing += PmpUiModel_Closing;
        }

        private void PmpUiModel_Closing(object sender, PmpCloseEventArgs e)
        {
            this.closeReason = e.Reason;
        }

        #endregion
        #region methods
        /// <summary>
        /// The recommended size for bitmaps is a square from 18- to 22-cells wide.
        /// </summary>
        /// <remarks>
        /// The recommended size for bitmaps is a square from 18- to 22-cells wide. However, the bitmap can be any size, as long as it fits on the title bar.
        ///The bitmap appears transparent by mapping any white(RGB(255,255,255)) cells to the current PropertyManager page title bar background color.Remember the special use of this color as you design your bitmap.
        /// </remarks>
        public void SetTitleIcon(Bitmap bitmap)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Pmp").Append(Title).Append(".png");
            var iconName = Path.Combine(IconDir.FullName, sb.ToString());
            using (bitmap)
            {
                var icon = new Bitmap(bitmap, new Size(22, 22));
                using (icon)
                {
                    try
                    {
                        icon.Save(iconName);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            Registering += (s, e) => { SolidworksObject.SetTitleBitmap2(iconName); };
        }

        internal void UpdateOptions()
        {
            if (keyStrokeSrc != null && !Options.HasFlag(PmpOptions.HandleKeystrokes))
                Options |= PmpOptions.HandleKeystrokes;
        }

        /// <summary>
        /// a list of pop up menu items (right mouse button click menu items)
        /// </summary>
        public List<PopUpMenuItem> PopUpMenuItems { get; set; }

        /// <summary>
        /// Sets the message in this PropertyManager page.  
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="message"></param>
        /// <param name="messageVisibility"></param>
        /// <param name="pageMessageExpanded"></param>
        /// <remarks>This method should be useful when creating multi-page PropertyManager pages where you want different informational messages on each page. </remarks>
        public void SetMessage(string caption, string message, swPropertyManagerPageMessageVisibility messageVisibility, swPropertyManagerPageMessageExpanded pageMessageExpanded)
        {
            Registering += (s, e) => { SolidworksObject.SetMessage3(message, ((int)messageVisibility), ((int)pageMessageExpanded), caption); };
        }

        /// <summary>
        ///Sets the cursor after a selection is made in the SOLIDWORKS graphics area.
        /// </summary>
        /// <param name="styles"></param>
        public void SetCursor(PmpCursorStyles styles)
        {
            Registering += (s, e) => { SolidworksObject.SetCursor(((int)styles)); };
        }

        /// <summary>
        /// return a specific control type based on its id
        /// </summary>
        /// <param name="id">id of control to return</param>
        /// <returns></returns>
        public IPmpControl GetControl(int id) => AllControls?.Where(c => c.Id == id).FirstOrDefault();

        /// <summary>
        /// get all controls of type T in this proper manger page
        /// </summary>
        /// <typeparam name="T">type of control to return</typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetControls<T>() where T : IPmpControl
        {
            var controls = AllControls?
                .Where(c => c is T).Cast<T>();
            return controls;
        }
        #endregion

        #region public properties
        /// <summary>
        /// directory where the icons are stored on the hard drive
        /// </summary>
        public DirectoryInfo IconDir { get; internal set; }

        /// <summary>
        /// get a flattened list of all groups hosted by this UI model
        /// </summary>
        public List<PmpGroup> AllGroups { get; private set; }

        /// <summary>
        /// get a flattened list of all controls hosted by this UI model
        /// </summary>
        public List<IPmpControl> AllControls { get; private set; }

        /// <summary>
        /// bitwise option as defined in <see cref="PmpOptions"/> default has okay, cancel, push-pin buttons and page build is disabled during handlers
        /// </summary>
        public PmpOptions Options { get; set; } = PmpOptions.LockedPage | PmpOptions.OkayButton | PmpOptions.CancelButton | PmpOptions.PushpinButton | PmpOptions.DisablePageBuildDuringHandlers;

        /// <summary>
        /// solidworks group boxes that contain solidworks pmp controllers
        /// </summary>
        public List<PmpGroup> PmpGroups { get; internal set; } = new List<PmpGroup>();

        /// <summary>
        /// solidworks property manager tabs that in turn can contain other group boxes and controls
        /// </summary>
        public List<PmpTab> PmpTabs { get; set; } = new List<PmpTab>();

        /// <summary>
        /// a title for this property manager page
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// solidworks object
        /// </summary>
        public ISldWorks Solidworks { get; set; }

        ///<inheritdoc/>
        public IPropertyManagerPage2 SolidworksObject { get; set; }
        #endregion

        #region Events


        /// <summary>   
        /// method to invoke once user clicked on question mark button on property manager page
        /// </summary>
        public event EventHandler<bool> HelpClicked
        {
            add { helpClickedSrc.Subscribe(this, value); }
            remove { helpClickedSrc.Unsubscribe(value); }
        }

        /// <summary>
        /// method to invoke after the property manager page is activated
        /// </summary>
        public event EventHandler<EventArgs> AfterActivation
        {
            add { afterActivationSrc.Subscribe(this, value); }
            remove { afterActivationSrc.Unsubscribe(value); }
        }

        /// <summary>
        /// method to invoke after the property manager page is closed
        /// </summary>
        public event EventHandler<PmpCloseEventArgs> AfterClose
        {
            add { afterCloseSrc.Subscribe(this, value); }
            remove { afterCloseSrc.Unsubscribe(value); }
        }

        /// <summary>
        /// method to invoke while the property manager page is closing
        /// </summary>
        public event EventHandler<PmpCloseEventArgs> Closing
        {
            add { closingSrc.Subscribe(this, value); }
            remove { closingSrc.Unsubscribe(value); }
        }

        /// <summary>
        /// method to invoke when user goes to the previous page of a property manager page
        /// </summary>
        public event EventHandler<bool> PreviousPageClicked
        {
            add { previousPageClickedSrc.Subscribe(this, value); }
            remove { previousPageClickedSrc.Unsubscribe(value); }
        }

        /// <summary>
        /// method to invoke when user goes to next page of a property manager page
        /// </summary>
        public event EventHandler<bool> NextPageClicked
        {
            add { nextPageClickedSrc.Subscribe(this, value); }
            remove { nextPageClickedSrc.Unsubscribe(value); }
        }

        /// <summary>
        /// method to invoke when user previews the results 
        /// </summary>
        public event EventHandler<bool> Preview
        {
            add { previewSrc.Subscribe(this, value); }
            remove { previewSrc.Unsubscribe(value); }
        }

        /// <summary>
        /// method to invoke when user selects on Whet's new button
        /// </summary>
        public event EventHandler<EventArgs> WhatsNewClicked
        {
            add { whatsNewClickedSrc.Subscribe(this, value); }
            remove { whatsNewClickedSrc.Unsubscribe(value); }
        }

        /// <summary>
        /// method to invoke when user calls undo (ctrl+z)
        /// </summary>
        public event EventHandler<EventArgs> UndoClicked
        {
            add { undoClickedSrc.Subscribe(this, value); }
            remove { undoClickedSrc.Unsubscribe(value); }
        }
        /// <summary>
        /// method to invoke when user Re-do something (ctrl+y)
        /// </summary>
        public event EventHandler<EventArgs> RedoClicked
        {
            add { redoClickedSrc.Subscribe(this, value); }
            remove { redoClickedSrc.Unsubscribe(value); }
        }

        /// <summary>
        /// fired once your addin is loaded or when solidworks starts up
        /// </summary>
        public event EventHandler<EventArgs> Registering
        {
            add { registeringSrc.Subscribe(this, value); }
            remove { registeringSrc.Unsubscribe(value); }
        }

        /// <summary>
        /// method to invoke when user clicks on a tab
        /// </summary>
        public event EventHandler<PmpTabClickedEventArgs> TabClicked
        {
            add { tabClickedSrc.Subscribe(this, value); }
            remove { tabClickedSrc.Unsubscribe(value); }
        }

        /// <summary>
        ///Processes a keystroke that occurred on this PropertyManager page
        /// </summary>
        /// <remarks><see cref="PmpUiModel.Options"/> should have the <see cref="PmpOptions.HandleKeystrokes"/> for this action to work</remarks>
        public event EventHandler<PmpKeyStrokeEventArgs> KeyStroke
        {
            add { keyStrokeSrc.Subscribe(this, value); }
            remove { keyStrokeSrc.Unsubscribe(value); }
        }
        #endregion

        #region call backs
        internal bool HelpClickedCallBack()
        {
            helpClickedSrc.Raise(this, true);
            return true;
        }
        internal void AfterActivationCallBack() { afterActivationSrc.Raise(this, EventArgs.Empty); }
        internal void AfterCloseCallBack() { afterCloseSrc.Raise(this, new PmpCloseEventArgs(closeReason)); }
        internal void WhatsNewClickedCallBack() { whatsNewClickedSrc.Raise(this, EventArgs.Empty); }
        internal void UndoClickedCallBack() { undoClickedSrc.Raise(this, EventArgs.Empty); }
        internal void RedoClickedCallBack() { redoClickedSrc.Raise(this, EventArgs.Empty); }
        internal void ClosingCallBack(int reason) { closingSrc.Raise(this, new PmpCloseEventArgs((PmpCloseReason)reason)); }
        internal bool PreviousPageClickedCallBack()
        {
            if (!previewSrc.HasHandlers())
                return false;
            previewSrc.Raise(this, true);
            return true;
        }
        internal bool TabbedClickedCallBack(int id)
        {
            if (!tabClickedSrc.HasHandlers()) return false;
            tabClickedSrc.Raise(this, new PmpTabClickedEventArgs(id));
            return true;
        }
        internal bool PreviewCallBack()
        {
            if (!previewSrc.HasHandlers()) return false;
            previewSrc.Raise(this, true); return true;
        }
        internal bool NextPageClickedCallBack()
        {
            if (!nextPageClickedSrc.HasHandlers()) return false;
            nextPageClickedSrc.Raise(this, true); return true;
        }
        internal void RegisteringCallBack(IPropertyManagerPage2 propertyManagerPage)
        {
            SolidworksObject = propertyManagerPage;

            if (PopUpMenuItems != null)
            {
                for (int i = 0; i < PopUpMenuItems.Count; i++)
                {
                    var item = PopUpMenuItems.ElementAt(i);
                    item.Id = i + 1;
                    var created = SolidworksObject.AddMenuPopupItem(item.Id, item.ItemText, ((int)item.DocumentType), item.Hint);
#if DEBUG
                    if (!created)
                    {
                        System.Diagnostics.Debug.WriteLine(($"Could not create pop up menu item: {item.ItemText}"));
                    }
#endif
                }
            }

            short lastId = 0;
            AllGroups = new List<PmpGroup>();
            AllControls = new List<IPmpControl>();
            //register all tabs and their controllers
            for (int i = 0; i < PmpTabs.Count; i++)
            {
                var tab = PmpTabs.ElementAt(i);

                //update the Id and set the IconDir before registering
                tab.Id = ++lastId;
                tab.IconDir = IconDir;

                //register the PmpGroups that are inside each tab
                //before registering the tab
                for (int j = 0; j < tab.TabGroups.Count; j++)
                {
                    var group = tab.TabGroups[j];
                    AllGroups.Add(group);
                    group.Id = ++lastId;
                    RegisterControls(group.Controls);
                }
                //this registers the tabs groups and that ,in turn, registers the controls in the group 
                tab.Register(propertyManagerPage);
            }

            //register the controls that are directly under this ui (main tab)
            for (int i = 0; i < PmpGroups.Count; i++)
            {
                var group = PmpGroups.ElementAt(i);
                AllGroups.Add(group);
                group.Id = ++lastId;
                RegisterControls(group.Controls);
                group.Register(propertyManagerPage);
            }

            //register the controls in a group
            void RegisterControls(IEnumerable<IPmpControl> controls)
            {

                for (int j = 0; j < controls.Count(); j++)
                {
                    var control = controls.ElementAt(j);

                    //add to list
                    AllControls.Add(control);

                    //update IconDir of the controls
                    control.SharedIconsDir = IconDir;

                    //get next id
                    control.Id = ++lastId;

                    //update mark if PmpSelectionBox
                    if (control is PmpSelectionBox box)
                        box.Mark = (int)Math.Pow(2, lastId);

                }
            }

            registeringSrc?.Raise(this, EventArgs.Empty);
            registeringSrc?.ClearHandlers();
        }


        internal bool KeyStrokeCallBack(int Wparam, int Message, int Lparam)
        {
            if (!keyStrokeSrc.HasHandlers())
                return false;
            keyStrokeSrc.Raise(this, new PmpKeyStrokeEventArgs(Wparam, Message, Lparam));
            return true;
        }

        /// <summary>
        /// release solidworks object
        /// </summary>
        public void ReleaseSolidworksObject()
        {
            Marshal.ReleaseComObject(SolidworksObject);
            AllControls.ForEach(ReleaseControl);
            AllGroups.ForEach(ReleaseGroups);
            PmpTabs.ForEach(ReleaseTabs);
        }

        private static void ReleaseControl(IPmpControl control)
        {
            if (control is IReleaseSolidworksObject p)
            {
                p.ReleaseSolidworksObject();
            }
            if (control is IDisposable d)
            {
                d.Dispose();
            }
        }

        private static void ReleaseTabs(PmpTab tab)
        {
            tab.ReleaseSolidworksObject();
        }

        private static void ReleaseGroups(PmpGroup group)
        {
            group.ReleaseSolidworksObject();
        }

        internal void UnsubscribeFromEvents()
        {
            AllControls.ForEach(UnsubscribeControl);
            AllGroups.ForEach(UnsubscribeGroup);
            PmpTabs.ForEach(UnsubscribeTab);
        }

        private static void UnsubscribeControl(IPmpControl c)
        {
            c.UnsubscribeFromEvents();
        }

        private static void UnsubscribeGroup(PmpGroup group)
        {
            group.UnsubscribeFromEvents();
        }

        private static void UnsubscribeTab(PmpTab tab)
        {
            tab.UnsubscribeFromEvents();
        }
        #endregion
    }

    /// <summary>
    /// event args for tab clicked event
    /// </summary>
    public class PmpTabClickedEventArgs
    {
        /// <summary>
        /// constructor for tab clicked event args
        /// </summary>
        /// <param name="tabId"></param>
        public PmpTabClickedEventArgs(int tabId)
        {
            TabId = tabId;
        }
        /// <summary>
        /// id of the clicked tab
        /// </summary>
        public int TabId { get; }
    }

    /// <summary>
    /// event args for property manager page close event
    /// </summary>
    public class PmpCloseEventArgs
    {
        /// <summary>
        /// constructor for close event args
        /// </summary>
        /// <param name="reason"></param>
        public PmpCloseEventArgs(PmpCloseReason reason)
        {
            Reason = reason;
        }
        /// <summary>
        /// reason for closing the property manager page
        /// </summary>
        public PmpCloseReason Reason { get; }
    }
}