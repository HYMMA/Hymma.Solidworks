// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using System.Linq;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// Generates Property Manager Page for a Solidworks Addin of type <see cref="AddinMaker"/> that runs on X64 base CPU<br/>
    /// <c>since 2015 solidworks supports X64 CPU only </c>
    /// </summary>
    public class PmpFactoryX64 : PmpFactoryBase
    {
        /// <summary>
        /// default constructor 
        /// </summary>
        /// <param name="uiModel">an object that hosts different inheritances of <see cref="IPmpControl"/> </param>
        public PmpFactoryX64(PmpUiModel uiModel)
            : base(new PmpEventHandler(uiModel), uiModel)
        {
        }

        /// <summary>
        /// SOLIDWORKS calls this method to show the UI. the PMP gets generated once at start up
        /// </summary>
        public override void Show()
        {
            if (propertyManagerPage == null)
            {
                Solidworks.SendMsgToUser("Could not create this property manager page");
                return;
            }


            //assign active document to each property manager page control
            uiModel.AllControls
                .ToList()
                .ForEach(c=>c.ActiveDoc= (ModelDoc2)uiModel.Solidworks.ActiveDoc);
            
            
            //call display method on groups which in turn calls the display on all the controls it hosts
            uiModel.AllGroups
                .ToList()
                .ForEach(g => g.Display());

            //display Solidworks object
            propertyManagerPage.Show();
        }
    }
}
