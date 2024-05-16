﻿// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Hymma.Solidworks.Addins
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
            Id = Counter.GetNextPmpId();
        }

        #endregion

        #region properties

        /// <summary>
        /// id of this tab used by solidworks 
        /// </summary>
        public int Id { get; internal set; }

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
        public List<PmpGroup> TabGroups { get; set; } = new List<PmpGroup>();
        #endregion

        #region methods

        /// <summary>
        /// Activates this tab in the PropertyManager page.  
        /// </summary>
        public void Activate()
        {
            Displaying += () => SolidworksObject.Activate();
        }

        internal void Register(IPropertyManagerPage2 propertyManagerPage)
        {
            string iconAddress = "";
            var sb = new StringBuilder();
            sb.Append("tab").Append(Id).Append(".bmp");
            if (_icon != null)
            {
                iconAddress = Path.Combine(IconDir.FullName, sb.ToString());
                using (var icon = new Bitmap(_icon, 16, 18))
                {
                    try
                    {
                        icon.Save(iconAddress, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            SolidworksObject = propertyManagerPage.AddTab(Id, Caption, iconAddress, 0);
            foreach (var group in TabGroups)
                group.Register(SolidworksObject);
        }
        #endregion

        #region call back
        internal void DisplayingCallback() => Displaying?.Invoke();
        internal void ClickedCallback() => Clicked?.Invoke();
        #endregion

        #region events
        /// <summary>
        /// fires a moment before the property manger page is displayed
        /// </summary>
        public event Action Displaying;

        /// <summary>
        /// invoked once user clicked on this tab
        /// </summary>
        public event Action Clicked;

        /// <summary>
        /// directory where this tab's main image get saved to
        /// </summary>
        public DirectoryInfo IconDir { get; internal set; }

        #endregion
    }
}
