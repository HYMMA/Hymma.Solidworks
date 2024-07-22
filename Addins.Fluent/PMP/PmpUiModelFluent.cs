// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// controls the general settings/Events of property manger page
    /// </summary>
    public class PmpUiModelFluent : PmpUiModel, IPmpUiModelFluent
    {
        /// <inheritdoc/>
        public PmpUiModelFluent(ISldWorks solidworks, string title) : base(solidworks, title)
        {

        }

        /// <inheritdoc/>
        public IPmpGroupFluent AddGroup(string caption)
        {
            var group = new PmpGroupFluent(caption, false)
            {
                //update the group property
                PropertyManagerPageUIBase = this
            };

            //add group to the end of the list
            this.PmpGroups.Add(group);

            //return the object in the list
            return this.PmpGroups[this.PmpGroups.Count - 1] as PmpGroupFluent;
        }

        /// <inheritdoc/>
        public IPmpUiModelFluent WithPmpOptions(PmpOptions options)
        {
            this.Options = options;
            return this;
        }


        ///<inheritdoc/>
        public IPmpUiModelFluent OnAfterClose(Action<PmpUiModel,PmpCloseReason> doThis)
        {
            this.AfterClose += doThis;
            return this;
        }

        /// <inheritdoc/>
        public IPmpUiModelFluent OnClosing(Action<PmpUiModel, PmpCloseReason> doThis)
        {
            this.Closing += doThis;
            return this;
        }


        /// <inheritdoc/>
        public IPmpUiModelFluent OnAfterActivation(Action<PmpUiModel> action)
        {
            AfterActivation += action;
            return this;
        }

        /// <inheritdoc/>
        public IAddinModelBuilder SavePropertyManagerPage(out PropertyManagerPageX64 propertyManagerPage)
        {

            propertyManagerPage = new PropertyManagerPageX64(this);
            this.AddinModel.PropertyManagerPages.Add(propertyManagerPage);
            return this.AddinModel;
        }

        /// <summary>
        /// add a tab to this property manager page 
        /// </summary>
        /// <param name="caption">text that appears on the tab</param>
        /// <param name="icon">The Bitmap argument allows you to place a bitmap before the text on the tab<br/>
        /// Any portions of the bitmap that are RGB(255,255,255) will be transparent, letting the tab background show through. this will be resized to 16x18 pixels</param>
        /// <returns></returns>
        public IPmpTabFluent AddTab(string caption, Bitmap icon = null)
        {
            var tab = new PmpTabFluent(caption, icon) { PmpUiModel = this };
            PmpTabs.Add(tab);
            return tab;
        }

        ///<inheritdoc/>
        public IPmpUiModelFluent AddTab(PmpTab tab)
        {
            PmpTabs.Add(tab);
            return this;
        }

        ///<inheritdoc/>
        public IPmpUiModelFluent AddTab<T>() where T : PmpTab, new()
        {
            var pmpTab2 = Activator.CreateInstance(typeof(T)) as T;
            PmpTabs.Add(pmpTab2);
            return this;
        }

        ///<inheritdoc/>
        public IPmpUiModelFluent WithCursorStyle(PmpCursorStyles cursorStyles)
        {
            SetCursor(cursorStyles);
            return this;
        }

        ///<inheritdoc/>
        public IPmpUiModelFluent WithMessage(string caption, string message, swPropertyManagerPageMessageVisibility messageVisibility, swPropertyManagerPageMessageExpanded pageMessageExpanded)
        {
            SetMessage(caption, message, messageVisibility, pageMessageExpanded);
            return this;
        }

        ///<inheritdoc/>
        public IPmpUiModelFluent WithIconInTitle(Bitmap icon)
        {
            SetTitleIcon(icon);
            return this;
        }

        ///<inheritdoc/>
        public IPmpUiModelFluent AddMenuPopUpItem(PopUpMenuItem item)
        {
            if (PopUpMenuItems == null)
                PopUpMenuItems = new List<PopUpMenuItem>();
            PopUpMenuItems.Add(item);
            return this;
        }

        ///<inheritdoc/>
        IPmpUiModelFluent IPmpUiModelFluent.OnKeyStroke(EventHandler<PmpKeyStrokeEventArgs> doThis)
        {
            KeyStroke += doThis;
            return this;
        }
        ///<inheritdoc/>
        public IPmpGroupFluentCheckable AddCheckableGroup(string caption)
        {
            var group = new PmpGroupFluentCheckable(caption)
            {
                //update the group property
                PropertyManagerPageUIBase = this
            };

            //add group to the end of the list
            this.PmpGroups.Add(group);

            //return the object in the list
            return this.PmpGroups[this.PmpGroups.Count - 1] as PmpGroupFluentCheckable;
        }



        /// <summary>
        /// the addin model that hosts this ui
        /// </summary>
        internal AddinModelBuilder AddinModel { get; set; }
    }
}
