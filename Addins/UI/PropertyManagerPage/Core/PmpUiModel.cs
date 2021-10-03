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
        //identifier fo this ui model
        private int Id;
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
            sb.Append("pmpIcon").Append(Id).Append(".png");
            var path = Path.Combine(IconGenerator.GetDefaultIconFolder(), sb.ToString());

            using (bitmap)
            {
                var icon = new Bitmap(bitmap, new Size(22, 22));
                using (icon)
                {
                    try
                    {
                        icon.Save(path);
                        OnRegister += () => { SolidworksObject.SetTitleBitmap2(path); };
                    }
                    catch (Exception e)
                    {
                        Log(e);
                    }
                }
            }
        }

        /// <summary>
        ///  Adds a menu item to the pop-up menu for this PropertyManager page. 
        /// </summary>
        /// <param name="itemText">    Text for pop-up menu item</param>
        /// <param name="hint">Text displayed in the SOLIDWORKS status bar when the user moves the cursor over this pop-up menu item</param>
        /// <param name="documentTypes">Document types for which this pop-up menu item is displayed as defined in <see cref="swDocumentTypes_e"/></param>
        public void AddMenuePopUpItem(string itemText, string hint, swDocumentTypes_e documentTypes)
        {
            OnRegister+=()=>{ SolidworksObject.AddMenuPopupItem(Counter.GetNextPmpId(), itemText, ((int)documentTypes), hint); };
        }

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
        #endregion

        #region public properties

        /// <summary>
        /// bitwise option as defined in <see cref="PmpOptions"/> default is 32807
        /// </summary>
        public int Options { get; set; } = 32807;

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
        /// solidworks object
        /// </summary>
        public ISldWorks Solidworks { get; set; }

        ///<inheritdoc/>
        public IPropertyManagerPage2 SolidworksObject { get; set; }
        #endregion

        #region call backs

        internal void Register(IPropertyManagerPage2 propertyManagerPage)
        {
            SolidworksObject = propertyManagerPage;
            PmpGroups.ForEach(group => group.Register(propertyManagerPage));
            PmpTabs.ForEach(tab => tab.Register(propertyManagerPage));

            OnRegister?.Invoke();
        }
        #endregion
    }
}