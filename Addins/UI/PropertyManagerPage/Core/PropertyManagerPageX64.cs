using SolidWorks.Interop.sldworks;
using System.Linq;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// Generates Property Manager Page for a Solidworks Addin of type <see cref="AddinMaker"/> that runs on X64 base CPU<br/>
    /// <c>since 2015 solidworks supports X64 CPU only </c>
    /// </summary>
    public class PropertyManagerPageX64 : PmpBase
    {
        /// <summary>
        /// default constructor 
        /// </summary>
        /// <param name="uiModel">an object that hosts differet inheritances of <see cref="IPmpControl"/> </param>
        public PropertyManagerPageX64(PropertyManagerPageUIBase uiModel)
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

            #region set window forms
            //this needs to be called every time pmp is shown
            //if (winFormHandlers != null)
            //{
            //    foreach (var handler in winFormHandlers)
            //    {
            //        if (handler.ElementHost == null || !handler.WindowsControl.HasContent)
            //            continue;
            //        handler.ElementHost.Child = handler.WindowsControl;
            //        handler.SolidworksObject.SetWindowHandlex64(handler.ElementHost.Handle.ToInt64());
            //    }
            //}
            #endregion

            //assign active document to each property manager page control
            var controls = uiModel.PmpGroups.SelectMany(p => p.Controls).ToList();

            uiModel.PmpGroups.ForEach(g => g.Display());
            foreach (var control in controls)
            {
                control.ActiveDoc = (ModelDoc2)uiModel.Solidworks.ActiveDoc;
                control.Display();
            }

            propertyManagerPage.Show();
        }
    }
}
