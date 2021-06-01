using SolidWorks.Interop.swpublished;
using System.Linq;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// Generates Property Manager Page for a Solidworks <see cref="AddinMaker"/>
    /// </summary>
    public abstract class PmpBasex64 : PmpBase
    {
        /// <summary>
        /// default constructor 
        /// </summary>
        /// <param name="addin">the addin of type <see cref="AddinMaker"/> to add thie property manger page to</param>
        /// <param name="eventHandler">object to handle events such as checkbox onclick etc...</param>
        /// <param name="uiModel">an object that hosts differet inheritances of <see cref="SwPMPControl"/> </param>
        public PmpBasex64(AddinMaker addin, PropertyManagerPage2Handler9 eventHandler, PmpUiModel uiModel) : base(addin, eventHandler, uiModel)
        {
        }

        /// <summary>
        /// SOLIDWORKS calls this method to show the UI. the PMP gets generated once at start up
        /// </summary>
        public override void Show()
        {
            if (PMP == null)
            {
                Solidworks.SendMsgToUser("Could not create this property manager page");
                return;
            }

            #region show window forms
            //this needs to be called every time pmp is shown
            if (winFormHandlers!=null)
            {
                foreach (var handler in winFormHandlers)
                {
                    if (handler.ElementHost == null || !handler.WindowsControl.HasContent)
                        continue;
                    handler.ElementHost.Child = handler.WindowsControl;
                    handler.ProperptyManagerPageHandle.SetWindowHandlex64(handler.ElementHost.Handle.ToInt64());
                }
            }
            PMP.Show();
            #endregion
        }
    }
}
