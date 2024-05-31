// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.Linq;

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
        internal PmpUiModel UiModel;

        /// <summary>
        /// handles events based on their id
        /// </summary>
        internal readonly PropertyManagerPage2Handler9 eventHandler;

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
        internal PropertyManagerPageBase(PropertyManagerPage2Handler9 eventHandler, PmpUiModel uiModel)
        {
            #region set up fields
            this.UiModel = uiModel ?? throw new ArgumentNullException();
            this.eventHandler = eventHandler;
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
            propertyManagerPage = Solidworks.CreatePropertyManagerPage(UiModel.Title, (int)UiModel.Options, eventHandler, ref errors) as IPropertyManagerPage2;

            //error is passed to object by reference
            if (propertyManagerPage != null && errors == (int)swPropertyManagerPageStatus_e.swPropertyManagerPage_Okay)
            {
                //add controls
                try
                {
                    UiModel.RegisteringCallBack(propertyManagerPage);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// displays this property manager page inside solidworks
        /// </summary>
        public abstract void Show();

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
                throw e;
            }
        }
    }
}
