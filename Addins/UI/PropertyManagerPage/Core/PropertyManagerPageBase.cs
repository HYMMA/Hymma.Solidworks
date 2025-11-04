// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// an abstract class for making property manager page
    /// </summary>
    public abstract class PropertyManagerPageBase
    {
        #region protected fields
        /// <summary>
        /// wrapper for ui objects
        /// </summary>
        ///<remarks>it is public for testing puposes</remarks>
        public PmpUiModel UiModel { get; private set; }

        /// <summary>
        /// handles events based on their id
        /// </summary>
        internal PmpEventHandler EventHandler { get; private set; }

        /// <summary>
        /// solidworks object
        /// </summary>
        internal readonly ISldWorks Solidworks;

        /// <summary>
        /// Property manager page object
        /// </summary>
        internal IPropertyManagerPage2 propertyManagerPage;
        #endregion

        /// <summary>
        /// default constructor 
        /// </summary>
        /// <param name="eventHandler">object to handle events such as check-box on-click etc...</param>
        /// <param name="uiModel">an object that hosts different inheritances of <see cref="IPmpControl"/> </param>
        /// <exception cref="ArgumentNullException"></exception>
        internal PropertyManagerPageBase(PmpEventHandler eventHandler, PmpUiModel uiModel)
        {
            #region set up fields
            this.UiModel = uiModel ?? throw new ArgumentNullException();
            this.EventHandler = eventHandler;
            Solidworks = this.UiModel.Solidworks;
            #endregion
        }

        /// <summary>
        /// creates a property manager page and adds controllers/>
        /// </summary>
        internal void CreatePropertyManagerPage()
        {
            int errors = -1;

            UiModel.UpdateOptions();
            propertyManagerPage = Solidworks.CreatePropertyManagerPage(UiModel.Title, (int)UiModel.Options, EventHandler, ref errors) as IPropertyManagerPage2;

            //error is passed to object by reference
            if (propertyManagerPage != null && errors == (int)swPropertyManagerPageStatus_e.swPropertyManagerPage_Okay)
            {
                //add controls
                try
                {
                    UiModel.RegisteringCallBack(propertyManagerPage);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// displays this property manager page inside solidworks
        /// </summary>
        public abstract void Show();

        /// <summary>
        /// releases the COM object and all its references from memory, unsusbscibes from events
        /// </summary>
        public void Release()
        {
            try
            {
                UiModel.UnsubscribeFromEvents();
                UiModel.ReleaseSolidworksObject();
                if (propertyManagerPage != null)
                {
                    Marshal.ReleaseComObject(propertyManagerPage);
                    propertyManagerPage = null;
                }
                Marshal.ReleaseComObject(Solidworks);
                EventHandler.UiModel.ReleaseSolidworksObject();
                EventHandler.UiModel.UnsubscribeFromEvents();
                UiModel = null;
                EventHandler = null;
            }
            catch (Exception e)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(e.Message);
#endif
            }
        }


        /// <summary>
        /// closes the property manager page 
        /// </summary>
        /// <param name="Okay"></param>
        public void Close(bool Okay)
        {
            try
            {
                propertyManagerPage.Close(Okay);
            }
            catch (Exception e)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(e.Message);
#endif
                throw e;
            }
        }
    }
}