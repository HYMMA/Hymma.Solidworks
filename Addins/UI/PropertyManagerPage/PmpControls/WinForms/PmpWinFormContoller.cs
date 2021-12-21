using SolidWorks.Interop.sldworks;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// Allows using a Windows form or Windows Form user control in the property manager page
    /// </summary>
    public class PmpWinFormContoller<T> : PmpControl<IPropertyManagerPageWindowFromHandle> where T : System.Windows.Forms.UserControl, new()
    {
        /// <summary>
        /// the ui control that is added to solidworks
        /// </summary>
        /// <remarks>use this property to assign values to the controller before they get displayed such as initial members of a list</remarks>
        internal T UiController { get; set; }

        /// <summary>
        /// contructor
        /// </summary>
        /// <param name="height">height of the control in the property manager page if set to zero the control will disapear from the property manager page</param>
        /// <param name="caption">a caption or message on top of folder browser dialogue</param>
        /// <param name="tip">tip of the controller</param>
        public PmpWinFormContoller(int height, string caption = "", string tip = "") : base(SolidWorks.Interop.swconst.swPropertyManagerPageControlType_e.swControlType_WindowFromHandle, caption, tip)
        {
            OnRegister += () => SolidworksObject.Height = height;
            OnDisplay += (s, e) =>
            {
                UiController = Activator.CreateInstance(typeof(T)) as T;
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
    }
}
