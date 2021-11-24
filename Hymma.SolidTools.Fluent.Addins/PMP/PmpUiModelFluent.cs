using Force.DeepCloner;
using Hymma.SolidTools.Addins;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// controls the general settings/Events of aproperty manger page
    /// </summary>
    public class PmpUiModelFluent : PmpUiModel, IPmpUiModelFluent
    {
        /// <inheritdoc/>
        public PmpUiModelFluent(ISldWorks solidworks) : base(solidworks)
        {

        }

        /// <inheritdoc/>
        public IPmpGroupFluent AddGroup(string caption)
        {
            var group = new PmpGroupFluent(caption, false)
            {
                //update the group propety
                PropertyManagerPageUIBase = this
            };

            //add group to the end of the list
            this.PmpGroups.Add(group);

            //return the boject in the list
            return this.PmpGroups[this.PmpGroups.Count - 1] as PmpGroupFluent;
        }

        /// <inheritdoc/>
        public IPmpUiModelFluent WithPmpOptions(PmpOptions options)
        {
            this.Options = options;
            return this;
        }


        ///<inheritdoc/>
        public IPmpUiModelFluent AfterClose(Action doThis)
        {
            this.OnAfterClose = doThis;
            return this;
        }

        /// <inheritdoc/>
        public IPmpUiModelFluent WhileClosing(Action<PmpCloseReason> doThis)
        {
            this.OnClose = doThis;
            return this;
        }


        /// <inheritdoc/>
        public IPmpUiModelFluent AfterActivation(Action action)
        {
            this.OnAfterActivation = action;
            return this;
        }

        /// <inheritdoc/>
        public IAddinModelBuilder SavePropertyManagerPage(out PmpFactoryX64 propertyManagerPage)
        {
            propertyManagerPage = new PmpFactoryX64(this);
            this.AddinModel.PropertyManagerPages.Add(propertyManagerPage);
            return this.AddinModel;
        }

        /// <summary>
        /// add a tab to this property manager page 
        /// </summary>
        /// <param name="caption">text taht appears on the tab</param>
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
        public IPmpTabFluent AddTab(PmpTabFluent pmpTab)
        {
           var pmpTab2 = pmpTab;
            pmpTab2.PmpUiModel = this;
            PmpTabs.Add(pmpTab2);
            return pmpTab2;
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
        public IPmpUiModelFluent AddMenuePopUpItem(PopUpMenueItem item)
        {
            if (PopUpMenueItems == null)
                PopUpMenueItems = new List<PopUpMenueItem>();
            PopUpMenueItems.Add(item);
            return this;
        }

        ///<inheritdoc/>
        IPmpUiModelFluent IPmpUiModelFluent.OnKeyStroke(EventHandler<PmpOnKeyStrokeEventArgs> doThis)
        {
            OnKeyStroke += doThis;
            return this;
        }
        ///<inheritdoc/>
        public IPmpGroupFluentCheckable AddCheckableGroup(string caption)
        {
            var group = new PmpGroupFluentCheckable(caption)
            {
                //update the group propety
                PropertyManagerPageUIBase = this
            };

            //add group to the end of the list
            this.PmpGroups.Add(group);

            //return the boject in the list
            return this.PmpGroups[this.PmpGroups.Count - 1] as PmpGroupFluentCheckable;
        }



        /// <summary>
        /// the addin model that hosts this ui
        /// </summary>
        internal AddinModelBuilder AddinModel { get; set; }
    }
}
