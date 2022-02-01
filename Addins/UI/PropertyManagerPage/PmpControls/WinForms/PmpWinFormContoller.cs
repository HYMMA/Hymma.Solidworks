using SolidWorks.Interop.sldworks;
using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// Allows using a Windows form or Windows Form user control in the property manager page
    /// </summary>
    public class PmpWinFormContoller : PmpControl 
    {
        /// <summary>
        /// the ui control that is added to solidworks
        /// </summary>
        /// <remarks>use this property to assign values to the controller before they get displayed such as initial members of a list</remarks>
        internal System.Windows.Forms.UserControl UiController { get; set; }

        /// <summary>
        /// contructor
        /// </summary>
        /// <param name="userControl"></param>
        /// <param name="height">height of the control in the property manager page if set to zero the control will disapear from the property manager page</param>
        /// <param name="caption">a caption or message on top of folder browser dialogue</param>
        /// <param name="tip">tip of the controller</param>
        public PmpWinFormContoller(System.Windows.Forms.UserControl userControl, int height, string caption = "", string tip = "") : base(SolidWorks.Interop.swconst.swPropertyManagerPageControlType_e.swControlType_WindowFromHandle, caption, tip)
        {
            Registering += () =>
            {
                SolidworksObject = (IPropertyManagerPageWindowFromHandle)Control;
                SolidworksObject.Height = height;
            };
            OnDisplay += (s, e) =>
            {
                //according to solidworks api it should be instantiated every time
                UiController = Activator.CreateInstance(userControl.GetType()) as System.Windows.Forms.UserControl;
                SolidworksObject?.SetWindowHandlex64(UiController.Handle.ToInt64());
            };
        }

        ///<inheritdoc/>
        public override bool Enabled
        {
            get => base.Enabled;
            set
            {
                base.Enabled = value;
                OnDisplay += (s, e) => UiController.Enabled = value;
            }
        }
        /// <summary>
        /// solidworks object
        /// </summary>
        public IPropertyManagerPageWindowFromHandle SolidworksObject { get; private set; }
    }
}
