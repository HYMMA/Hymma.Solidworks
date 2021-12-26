using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using static Hymma.SolidTools.Addins.Logger;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a wrapper for solidworks property manager page UI
    /// </summary>
    public class PmpUiModel : IWrapSolidworksObject<IPropertyManagerPage2>
    {
        #region fields

        #endregion

        #region default constructor

        /// <summary>
        /// base class for making proeprty manager page UI models and controls
        /// </summary>
        /// <param name="solidworks"></param>
        public PmpUiModel(ISldWorks solidworks)
        {
            this.Solidworks = solidworks;
            Id = Counter.GetNextPmpId();
            StringBuilder sb = new StringBuilder();
            sb.Append("Pmp").Append(Id);
            IconDir = AddinMaker.GetIconsDir().CreateSubdirectory(sb.ToString());
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
                        OnRegister += () => { SolidworksObject.SetTitleBitmap2(iconName); };
                    }
                    catch (Exception e)
                    {
                        Log(e);
                    }
                }
            }
        }

        internal void UpdateOptions()
        {
            if (OnKeyStroke != null && !Options.HasFlag(PmpOptions.HandleKeystrokes))
                Options |= PmpOptions.HandleKeystrokes;
        }

        /// <summary>
        /// a list of pop up menue items (right mouse button click menue items)
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
            OnRegister += () => { SolidworksObject.SetMessage3(message, ((int)messageVisibility), ((int)pageMessageExpanded), caption); };
        }

        /// <summary>
        ///Sets the cursor after a selection is made in the SOLIDWORKS graphics area.
        /// </summary>
        /// <param name="styles"></param>
        public void SetCursor(PmpCursorStyles styles)
        {
            OnRegister += () => { SolidworksObject.SetCursor(((int)styles)); };
        }

        /// <summary>
        /// return a specific control type based on its id
        /// </summary>
        /// <param name="id">id of control to return</param>
        /// <returns></returns>
        public IPmpControl GetControl(int id) => AllControls?.Where(c => c.Id == id).FirstOrDefault();

        /// <summary>
        /// get all controls of type T in this propery manger page
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
        /// bitwise option as defined in <see cref="PmpOptions"/> default has okay, cancel, pushpin buttons and page build is disabled during handlers
        /// </summary>
        public PmpOptions Options { get; set; } = PmpOptions.LockedPage | PmpOptions.OkayButton | PmpOptions.CancelButton | PmpOptions.PushpinButton | PmpOptions.DisablePageBuildDuringHandlers;

        /// <summary>
        /// solidworks group boxes that contain solidworks pmp controllers
        /// </summary>
        public List<PmpGroup> PmpGroups { get; internal set; } = new List<PmpGroup>();

        /// <summary>
        /// solidworks property managager tabs that in turn can contain other group boxes and controls
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
        public Action<PmpCloseReason> OnClose { get; set; }

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
        /// fired once your addin is loaded or when solidworks starts up
        /// </summary>
        public Action OnRegister { get; set; }

        /// <summary>
        /// method to invoke when user clickes on a tab
        /// </summary>
        public Func<int, bool> OnTabClicked { get; set; }

        /// <summary>
        ///Processes a keystroke that occurred on this PropertyManager page
        /// </summary>
        /// <remarks><see cref="PmpUiModel.Options"/> should have the <see cref="PmpOptions.HandleKeystrokes"/> for this action to work</remarks>
        public event EventHandler<PmpOnKeyStrokeEventArgs> OnKeyStroke;
        #endregion

        #region call backs
        internal void Register(IPropertyManagerPage2 propertyManagerPage)
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
            
            //update icon dir in the tabs and pmp controllers
            AllControls.ToList().ForEach(c => c.SharedIconsDir = IconDir);
            PmpTabs.ForEach(tab => tab.IconDir = IconDir);

            //register the controllers
            PmpGroups.ForEach(group => group.Register(propertyManagerPage));
            PmpTabs.ForEach(tab => tab.Register(propertyManagerPage));

            OnRegister?.Invoke();
        }


        internal bool KeyStroke(int Wparam, int Message, int Lparam)
        {
            if (OnKeyStroke != null)
            {
                OnKeyStroke.Invoke(this, new PmpOnKeyStrokeEventArgs(Wparam, Message, Lparam));
                return true;
            }
            return false;
        }
        #endregion
    }
}