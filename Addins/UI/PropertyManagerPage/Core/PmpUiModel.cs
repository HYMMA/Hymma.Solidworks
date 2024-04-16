﻿// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a wrapper for solidworks property manager page UI
    /// </summary>
    public class PmpUiModel : IWrapSolidworksObject<IPropertyManagerPage2>
    {

        #region default constructor

        /// <summary>
        /// base class for making property manager page UI models and controls
        /// </summary>
        /// <param name="solidworks"></param>
        public PmpUiModel(ISldWorks solidworks)
        {
            this.Solidworks = solidworks;
            Id = Counter.GetNextPmpId();
            StringBuilder sb = new StringBuilder();
            sb.Append("Pmp").Append(Id);
            IconDir = AddinIcons.IconsDir.CreateSubdirectory(sb.ToString());
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
            sb.Append("Pmp").Append(Id).Append(".png");
            var iconName = Path.Combine(IconDir.FullName, sb.ToString());
            using (bitmap)
            {
                var icon = new Bitmap(bitmap, new Size(22, 22));
                using (icon)
                {
                    try
                    {
                        icon.Save(iconName);
                        Registering += () => { SolidworksObject.SetTitleBitmap2(iconName); };
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }

        internal void UpdateOptions()
        {
            if (KeyStroke != null && !Options.HasFlag(PmpOptions.HandleKeystrokes))
                Options |= PmpOptions.HandleKeystrokes;
        }

        /// <summary>
        /// a list of pop up menu items (right mouse button click menu items)
        /// </summary>
        public List<PopUpMenueItem> PopUpMenueItems { get; set; }

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
            Registering += () => { SolidworksObject.SetMessage3(message, ((int)messageVisibility), ((int)pageMessageExpanded), caption); };
        }

        /// <summary>
        ///Sets the cursor after a selection is made in the SOLIDWORKS graphics area.
        /// </summary>
        /// <param name="styles"></param>
        public void SetCursor(PmpCursorStyles styles)
        {
            Registering += () => { SolidworksObject.SetCursor(((int)styles)); };
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
        public DirectoryInfo IconDir { get; }

        /// <summary>
        ///identifier for this ui model
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// get a flattened list of all groups hosted by this UI model
        /// </summary>
        public IEnumerable<PmpGroup> AllGroups { get; private set; }

        /// <summary>
        /// get a flattened list of all controls hosted by this UI model
        /// </summary>
        public IEnumerable<IPmpControl> AllControls { get; private set; }

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
        public event Func<bool> HelpClicked;

        /// <summary>
        /// method to invoke after the property manager page is activated
        /// </summary>
        public event Action AfterActivation;

        /// <summary>
        /// method to invoke after the property manager page is closed
        /// </summary>
        public event Action AfterClose;

        /// <summary>
        /// method to invoke while the property manager page is closing
        /// </summary>
        public event Action<PmpCloseReason> Closing;

        /// <summary>
        /// method to invoke when user goes to the previous page of a property manager page
        /// </summary>
        public event Func<bool> PreviousPageClicked;

        /// <summary>
        /// method to invoke when user goes to next page of a property manager page
        /// </summary>
        public event Func<bool> NextPageClicked;

        /// <summary>
        /// method to invoke when user previews the results 
        /// </summary>
        public event Func<bool> Preview;

        /// <summary>
        /// method to invoke when user selects on Whats new button
        /// </summary>
        public event Action WhatsNewClicked;

        /// <summary>
        /// method to invoke when user calls undo (ctrl+z)
        /// </summary>
        public event Action UndoClicked;
        /// <summary>
        /// method to invoke when user Re-do something (ctrl+y)
        /// </summary>
        public event Action RedoClicked;

        /// <summary>
        /// fired once your addin is loaded or when solidworks starts up
        /// </summary>
        public event Action Registering;

        /// <summary>
        /// method to invoke when user clicks on a tab
        /// </summary>
        public event Func<int, bool> TabClicked;

        /// <summary>
        ///Processes a keystroke that occurred on this PropertyManager page
        /// </summary>
        /// <remarks><see cref="PmpUiModel.Options"/> should have the <see cref="PmpOptions.HandleKeystrokes"/> for this action to work</remarks>
        public event EventHandler<PmpKeyStrokeEventArgs> KeyStroke;
        #endregion

        #region call backs
        internal bool HelpClickedCallBack() => HelpClicked != null && HelpClicked.Invoke();
        internal void AfterActivationCallBack() => AfterActivation?.Invoke();
        internal void AfterCloseCallBack() => AfterClose?.Invoke();
        internal void WhatsNewClickedCallBack() => WhatsNewClicked?.Invoke();
        internal void UndoClickedCallBack() => UndoClicked?.Invoke();
        internal void RedoClickedCallBack() => RedoClicked?.Invoke();
        internal void ClosingCallBack(int reason) => Closing?.Invoke((PmpCloseReason)reason);
        internal bool PreviousPageClickedCallBack() => PreviousPageClicked != null && PreviousPageClicked.Invoke();
        internal bool TabedClickedCallBack(int id) => TabClicked != null && TabClicked.Invoke(id);
        internal bool PreviewCallBack() => Preview != null && Preview.Invoke();
        internal bool NextPageClickedCallBack() => NextPageClicked != null && NextPageClicked.Invoke();
        internal void RegisteringCallBack(IPropertyManagerPage2 propertyManagerPage)
        {
            SolidworksObject = propertyManagerPage;

            if (PopUpMenueItems != null)
            {
                foreach (var item in PopUpMenueItems)
                {
                    var result = SolidworksObject.AddMenuPopupItem(item.Id, item.ItemText, ((int)item.DocumentType), item.Hint);
                }
            }

            AllGroups = PmpTabs.SelectMany(t => t.TabGroups).Concat(PmpGroups);
            AllControls = AllGroups.SelectMany(g => g.Controls);

            //update icon _dir in the tabs and pmp controllers
            AllControls.ToList().ForEach(c => c.SharedIconsDir = IconDir);
            PmpTabs.ForEach(tab => tab.IconDir = IconDir);

            //register the controllers
            PmpGroups.ForEach(group => group.Register(propertyManagerPage));
            PmpTabs.ForEach(tab => tab.Register(propertyManagerPage));

            Registering?.Invoke();
        }


        internal bool KeyStrokeCallBack(int Wparam, int Message, int Lparam)
        {
            if (KeyStroke != null)
            {
                KeyStroke.Invoke(this, new PmpKeyStrokeEventArgs(Wparam, Message, Lparam));
                return true;
            }
            return false;
        }
        #endregion
    }
}