using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.Linq;
using static Hymma.SolidTools.Addins.Logger;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// an abstract class for making propety manager page
    /// </summary>
    public abstract class PmpBase : IFluent
    {
        #region protected fields
        /// <summary>
        /// pointer to the addin this pmp belongs to
        /// </summary>
        protected readonly AddinMaker addin;

        /// <summary>
        /// wrapper for ui objects
        /// </summary>
        protected readonly PropertyManagerPageUIBase uiModel;

        /// <summary>
        /// handles evetns based on their id
        /// </summary>
        protected readonly PropertyManagerPage2Handler9 eventHandler;

        /// <summary>
        /// solidworks object
        /// </summary>
        protected readonly ISldWorks Solidworks;

        /// <summary>
        /// a collection of <see cref="PmpWindowHandler"/> to hook property manager page with a <see cref="System.Windows.Controls.UserControl"/>
        /// </summary>
        protected IEnumerable<PmpWindowHandler> winFormHandlers;

        /// <summary>
        /// Property manager page object
        /// </summary>
        protected IPropertyManagerPage2 PMP;

        /// <summary>
        /// a dictionary of controls and their id where key is the id of control
        /// </summary>
        protected Dictionary<int, object> Controls  = new Dictionary<int, object>();
        #endregion

        #region private methodes
        private int id = 1;
        private int GetNextId()
        {
            return id++;
        }
        #endregion

        /// <summary>
        /// default constructor 
        /// </summary>
        /// <param name="addin">the addin of type <see cref="AddinMaker"/> to add thie property manger page to</param>
        /// <param name="eventHandler">object to handle events such as checkbox onclick etc...</param>
        /// <param name="uiModel">an object that hosts differet inheritances of <see cref="IPmpControl"/> </param>
        /// <exception cref="ArgumentNullException"></exception>
        protected PmpBase(AddinMaker addin, PropertyManagerPage2Handler9 eventHandler, PropertyManagerPageUIBase uiModel)
        {
            if (addin == null || uiModel == null)
                throw new ArgumentNullException();

            #region set up fields
            this.addin = addin;
            this.uiModel = uiModel;
            this.eventHandler = eventHandler;
            Solidworks = this.addin.Solidworks;

            //get element host wrappers
            var winFormHandlers = uiModel.PmpGroups
              .Select(box => box.Controls
              .Where(c => c is PmpWindowHandler));


            #endregion

            CreatePropertyManagerPage();
        }

        /// <summary>
        /// creates a propety manager page and adds controllers/>
        /// </summary>
        protected void CreatePropertyManagerPage()
        {
            int errors = -1;

            Log($"Makin property manager page {nameof(PmpBase)}");
            PMP = Solidworks.CreatePropertyManagerPage(uiModel.Title, uiModel.Options, eventHandler, ref errors) as IPropertyManagerPage2;

            //error is passed to object by reference
            if (PMP != null && errors == (int)swPropertyManagerPageStatus_e.swPropertyManagerPage_Okay)
            {
                //update Ids
                foreach (var box in uiModel.PmpGroups)
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
                    uiModel.PmpGroups.ForEach(box => PMP.AddGroup(box, Controls));
                }
                catch (Exception e)
                {
                    Solidworks.SendMsgToUser2(e.Message, 0, 0);
                }
            }
        }

        /// <summary>
        /// displays this property manager page inside solidworks
        /// </summary>
        public abstract void Show();
    }
}
