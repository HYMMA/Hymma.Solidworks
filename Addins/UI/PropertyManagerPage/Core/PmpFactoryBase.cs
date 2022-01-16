﻿using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.Linq;
using static Hymma.Solidworks.Addins.Logger;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// an abstract class for making propety manager page
    /// </summary>
    public abstract class PmpFactoryBase
    {
        #region protected fields
        /// <summary>
        /// wrapper for ui objects
        /// </summary>
        protected readonly PmpUiModel uiModel;

        /// <summary>
        /// handles evetns based on their id
        /// </summary>
        protected readonly PropertyManagerPage2Handler9 eventHandler;

        /// <summary>
        /// solidworks object
        /// </summary>
        protected readonly ISldWorks Solidworks;

        /// <summary>
        /// a collection of <see cref="PmpWpfHost"/> to hook property manager page with a <see cref="System.Windows.Controls.UserControl"/>
        /// </summary>
        protected IEnumerable<PmpWpfHost> winFormHandlers;

        /// <summary>
        /// Property manager page object
        /// </summary>
        protected IPropertyManagerPage2 propertyManagerPage;
        #endregion

        /// <summary>
        /// default constructor 
        /// </summary>
        /// <param name="eventHandler">object to handle events such as checkbox onclick etc...</param>
        /// <param name="uiModel">an object that hosts differet inheritances of <see cref="PmpControl"/> </param>
        /// <exception cref="ArgumentNullException"></exception>
        protected PmpFactoryBase(PropertyManagerPage2Handler9 eventHandler, PmpUiModel uiModel)
        {
            #region set up fields
            this.uiModel = uiModel ?? throw new ArgumentNullException();
            this.eventHandler = eventHandler;
            Solidworks = this.uiModel.Solidworks;

            //get element host wrappers
            var winFormHandlers = uiModel.PmpGroups
              .Select(box => box.Controls
              .Where(c => c is PmpWpfHost));


            #endregion

            CreatePropertyManagerPage();
        }

        /// <summary>
        /// creates a propety manager page and adds controllers/>
        /// </summary>
        protected void CreatePropertyManagerPage()
        {
            int errors = -1;

            Log($"Makin property manager page {nameof(PmpFactoryBase)}");
            uiModel.UpdateOptions();
            propertyManagerPage = Solidworks.CreatePropertyManagerPage(uiModel.Title, (int)uiModel.Options, eventHandler, ref errors) as IPropertyManagerPage2;

            //error is passed to object by reference
            if (propertyManagerPage != null && errors == (int)swPropertyManagerPageStatus_e.swPropertyManagerPage_Okay)
            {
                //add controls
                try
                {
                    uiModel.Register(propertyManagerPage);
                }
                catch (Exception e)
                {
                    Log($"Error! {e}");
                }
            }
        }

        /// <summary>
        /// displays this property manager page inside solidworks
        /// </summary>
        public abstract void Show();

        /// <summary>
        /// closes the properyt manager page 
        /// </summary>
        /// <param name="Okay"></param>
        public void Close(bool Okay)
        {
            try
            {
                propertyManagerPage.Close(Okay);
            }
            catch (Exception)
            {
            }
        }
    }
}
