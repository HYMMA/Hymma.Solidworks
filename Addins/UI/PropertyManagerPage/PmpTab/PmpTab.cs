using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// Tab for a PropertyManager page. 
    /// </summary>
    public class PmpTab : IWrapSolidworksObject<IPropertyManagerPageTab>
    {
        #region fields

        private Bitmap _icon;
        #endregion

        #region constructor

        /// <summary>
        /// create a tab for property manager page
        /// </summary>
        /// <param name="caption">caption for this property manager page tab</param>
        /// <param name="icon">The Bitmap argument allows you to place a bitmap before the text on the tab<br/>
        /// Any portions of the bitmap that are RGB(255,255,255) will be transparent, letting the tab background show through. this will be resized to 16x18 pixels</param>
        public PmpTab(string caption, Bitmap icon = null)
        {
            Caption = caption;
            _icon = icon;
        }

        #endregion

        #region properties
        /// <summary>
        /// caption for this property manager page tab
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Actual solidworks object
        /// </summary>
        public IPropertyManagerPageTab SolidworksObject { get; set; }

        /// <summary>
        /// Access the <see cref="PmpGroup"/>s in this tab
        /// </summary>
        public List<PmpGroup> Groups { get; set; }
        #endregion

        #region methods

        /// <summary>
        /// Activates this tab in the PropertyManager page.  
        /// </summary>
        public void Activate()
        {
            OnDisplay += () => { SolidworksObject.Activate(); };
        }
        internal void Register(IPropertyManagerPage2 propertyManagerPage)
        {
            int tabId = Counter.GetNextPmpId();
            string iconAddress = "";

            if (_icon != null)
            {
                iconAddress = Path.Combine(IconGenerator.GetDefaultIconFolder(), "tab" + tabId + ".png");
                using (var icon = new Bitmap(_icon, 16, 18))
                {
                    try
                    {
                        icon.Save(iconAddress);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            SolidworksObject = propertyManagerPage.AddTab(tabId, Caption, iconAddress, 0);

            foreach (var group in Groups)
                group.Register(propertyManagerPage);
        }
        #endregion

        #region events
        /// <summary>
        /// fires a moment before the the property manger page is displayed
        /// </summary>
        public Action OnDisplay { get; set; }
        #endregion
    }
}
