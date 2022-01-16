using SolidWorks.Interop.sldworks;
using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// allows creating a winform in a propeprty manager page
    /// </summary>
    public class PmpWinForm : PmpControl 
    {
        private System.Windows.Forms.Form _userControl;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="form">window form instance</param>
        /// <param name="height">if set to zero the form will not be shown</param>
        /// <param name="caption">caption for the form</param>
        /// <param name="tip">a tip for the controller</param>
        public PmpWinForm(System.Windows.Forms.Form form, int height, string caption = "", string tip = "") : base(SolidWorks.Interop.swconst.swPropertyManagerPageControlType_e.swControlType_WindowFromHandle, caption, tip)
        {
            OnRegister += () =>
            {
                SolidworksObject = (IPropertyManagerPageWindowFromHandle)Control;
                SolidworksObject.Height = height;
            };
            OnDisplay += (s, d) =>
            {
                //user needs to create the dotnet control at every display
                _userControl = Activator.CreateInstance(form.GetType()) as System.Windows.Forms.Form;

                _userControl.Enabled = Enabled;

                //this is suggested by solidworks website
                _userControl.TopLevel = false;

                //again from solidworks website
                _userControl.Show();
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

        /// <summary>
        /// solidworks object
        /// </summary>
        public IPropertyManagerPageWindowFromHandle SolidworksObject { get; private set; }
    }
}
