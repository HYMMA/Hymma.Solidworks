using Hymma.SolidTools.Addins;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// controls the general settings/Events of aproperty manger page
    /// </summary>
    public class PmpUi : PropertyManagerPageUIBase, IFluent, IPmpUi
    {
        /// <inheritdoc/>
        public PmpUi(ISldWorks solidworks) : base(solidworks)
        {

        }
        
        /// <inheritdoc/>
        public IPmpGroup AddGroup(string caption)
        {
            var group = new PropertyManagerPageGroup(caption, false)
            {

                //update the group propety
                PropertyManagerPageUIBase = this
            };

            //add group to the end of the list
            this.PmpGroups.Add(group);

            //return the boject in the list
            return this.PmpGroups[this.PmpGroups.Count - 1] as PropertyManagerPageGroup;
        }

        /// <inheritdoc/>
        public IPmpUi WithOptions(swPropertyManagerPageOptions_e options)
        {
            this.Options = (int)options;
            return this;
        }

        
        ///<inheritdoc/>
        public IPmpUi AfterClose(Action doThis)
        {
            this.OnAfterClose = doThis;
            return this;
        }

        /// <inheritdoc/>
        public IPmpUi WhileClosing(Action<PMPCloseReason> doThis)
        {
            this.OnClose = doThis;
            return this;
        }

   
        /// <inheritdoc/>
        public IPmpUi AfterActivation(Action action)
        {
            this.OnAfterActivation = action;
            return this;
        }

        /// <inheritdoc/>
        public IPmpUi On_TabClicked(Func<int, bool> doThis)
        {
            OnTabClicked = doThis;
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
        /// the addin model that hosts this ui
        /// </summary>
        internal AddinModelBuilder AddinModel { get;set; }
    }
}
