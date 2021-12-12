using SolidWorks.Interop.sldworks;
using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// Allows using a Windows form or Windows Form user control in the property manager page
    /// </summary>
    public class PmpWinFormContoller<T> : PmpControl<IPropertyManagerPageWindowFromHandle> where T : System.Windows.Forms.UserControl, new()
    {
        private T _userControl;

        /// <summary>
        /// contructor
        /// </summary>
        /// <param name="height">height of the control in the property manager page if set to zero the control will disapear from the property manager page</param>
        /// <param name="caption">a caption for the controller</param>
        /// <param name="tip">tip of the controller</param>
        public PmpWinFormContoller(int height, string caption = "", string tip = "") : base(SolidWorks.Interop.swconst.swPropertyManagerPageControlType_e.swControlType_WindowFromHandle, caption, tip)
        {
            OnRegister += () => SolidworksObject.Height = height;
            OnDisplay += (s, d) =>
            {
                _userControl = Activator.CreateInstance(typeof(T)) as T;
                _userControl.Enabled = Enabled;
                SolidworksObject?.SetWindowHandlex64(_userControl.Handle.ToInt64());
            };
        }
        ///<inheritdoc/>
        public override bool Enabled
        {
            get => base.Enabled;
            set
            {
                base.Enabled = value;
                if (_userControl != null)
                    _userControl.Enabled = value;
            }
        }
    }
}
