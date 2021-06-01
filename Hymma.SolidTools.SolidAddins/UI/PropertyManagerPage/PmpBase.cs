using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// an abstract class for making propety manager page
    /// </summary>
    public abstract class PmpBase
    {
        #region protected fields
        /// <summary>
        /// pointer to the addin this pmp belongs to
        /// </summary>
        protected readonly AddinMaker addin;

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
        /// a collection of <see cref="SwWindowHandler"/> to hook property manager page with a <see cref="System.Windows.Controls.UserControl"/>
        /// </summary>
        protected IEnumerable<SwWindowHandler> winFormHandlers;

        /// <summary>
        /// Property manager page object
        /// </summary>
        protected IPropertyManagerPage2 PMP;

        protected List<object> controls = new List<object>();
        #endregion


        /// <summary>
        /// defaut constructor 
        /// </summary>
        /// <param name="addin">the addin of type <see cref="AddinMaker"/> to add thie property manger page to</param>
        /// <param name="eventHandler">object to handle events such as checkbox onclick etc...</param>
        /// <param name="uiModel">an object that hosts differet inheritances of <see cref="SwPMPControl"/> </param>
        /// <exception cref="ArgumentNullException"></exception>
        protected PmpBase(AddinMaker addin, PropertyManagerPage2Handler9 eventHandler, PmpUiModel uiModel)
        {
            if (addin == null || uiModel == null)
                throw new ArgumentNullException();

            #region set up fields
            this.addin = addin;
            this.uiModel = uiModel;
            this.eventHandler = eventHandler;
            Solidworks = this.addin.Solidworks;

            //get element host wrappers
            var winFormHandlers = uiModel.SwBoxes
              .Select(box => box.Controls
              .Where(c => c is SwWindowHandler));


            #endregion

            CreatePropertyManagerPage();
        }

        /// <summary>
        /// creates a propety manager page and adds controllers/>
        /// </summary>
        protected void CreatePropertyManagerPage()
        {
            int errors = -1;
            PMP = Solidworks.CreatePropertyManagerPage(uiModel.Title, uiModel.Options, eventHandler, ref errors) as IPropertyManagerPage2;

            //error is passed to object by reference
            if (PMP != null && errors == (int)swPropertyManagerPageStatus_e.swPropertyManagerPage_Okay)
            {
                //update Ids
                foreach (var box in uiModel.SwBoxes)
                {
                    box.Id = GetNextId();
                    foreach (var controller in box.Controls)
                    {
                        controller.Id = (short)GetNextId();
                    }
                }

                //add controls
                try
                {
                    uiModel.SwBoxes.ForEach(box => PMP.AddSwBox(box, controls));
                }
                catch (Exception e)
                {
                    Solidworks.SendMsgToUser2(e.Message, 0, 0);
                }
            }
        }
        private int id = 0;
        private int GetNextId()
        {
            return id++;
        }

        /// <summary>
        /// displays this property manager page inside solidworks
        /// </summary>
        public abstract void Show();

        //public PmpBase Instantiate()
        //{
        //    return this;
        //}
    }
}
