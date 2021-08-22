using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// a SOLIDWORKS property manager page control
    /// </summary>
    public class PmpControl<T> : IPmpControl, IWrapSolidworksObject<T>
    {
        #region privatre fields
        /// <summary>
        /// property manager page control that can be cast to all the property manager page members
        /// </summary>
        private IPropertyManagerPageControl _control;
        private short _top;
        private int _optionForResize;
        private bool _enabled = true;
        private bool _visible = true;
        private bool _topIsChanged;
        #endregion

        #region constructor

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="type">type of this controller as per <see cref="swPropertyManagerPageControlType_e"/></param>
        /// <param name="caption">caption for this control</param>
        /// <param name="tip">tip for this control</param>
        public PmpControl(swPropertyManagerPageControlType_e type, string caption = "", string tip = "")
            : base(type, caption, tip)
        {
            OnRegister += PmpControl_OnRegister;
        }
        #endregion

        #region call backs
        private void PmpControl_OnRegister()
        {
            SolidworksObject = (T)ControlObject;
            _control = SolidworksObject as IPropertyManagerPageControl;
            _control.OptionsForResize = OptionsForResize;
            _control.Enabled = Enabled;
            _control.Visible = Visible;

            if (_topIsChanged)
                _control.Top = Top;
        }

        ///<inheritdoc/>
        internal override void Display()
        {
            OnDisplay?.Invoke(this, new OnDisplay_EventArgs(_control));
        }

        internal override void GainedFocus()
        {
            OnGainedFocus?.Invoke(this, EventArgs.Empty);
        }

        internal override void LostFocus()
        {
            OnLostFocus?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region public properties
        /// <summary>
        /// Gets or sets the top edge of the control on a PropertyManager page
        /// </summary>
        /// <remarks>returns -1 on error</remarks>
        public short Top
        {
            get => _top;
            set
            {
                _topIsChanged = true;
                _top = value;
                if (_control != null)
                    _control.Top = value;
            }
        }

        /// <summary>
        /// Gets or sets how to override the SOLIDWORKS default behavior when changing the width of a PropertyManager page. <br/>
        /// Resize the PropertyManager page as defined in <see cref="PmpResizeStyles"/>
        /// </summary>
        public int OptionsForResize
        {
            get => _optionForResize;
            set
            {
                _optionForResize = value;
                if (_control != null)
                    _control.OptionsForResize = value;
            }
        }

        /// <summary>
        /// enables or disables this property control on
        /// </summary>
        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                if (_control != null)
                    _control.Enabled = value;
            }
        }

        /// <summary>
        /// gets or sets the visibility of this control
        /// </summary>
        public bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                if (_control != null)
                    _control.Visible = value;
            }
        }

        ///<inheritdoc/>
        public T SolidworksObject { get; internal set; }

        #endregion

        #region methods

        /// <summary>
        /// Displays a bubble ToolTip containing the specified title, message, and bitmap. A bubble ToolTip is useful for validating data typed or selected by users in controls on a PropertyManager page.
        /// </summary>
        /// <param name="title">Title to display in bubble ToolTip</param>
        /// <param name="message">Message to display in bubble ToolTip</param>
        /// <param name="bitmap">bitmap object to use as icon in the tooltip</param>
        /// <param name="fileName">resultant bitmap file name on disk without extensions or directory</param>
        public void ShowBubleTooltip(string title, string message, Bitmap bitmap, string fileName)
        {
            if (_control == null) return;
            var fullFileName = Path.Combine(IconGenerator.GetDefaultIconFolder(), fileName);
            MaskedBitmap.Save(new Bitmap(bitmap, 18, 18),ref fullFileName);
            _control.ShowBubbleTooltip(title, message, fullFileName);
        }

        #endregion

        #region events

        /// <summary>
        /// event handler for a <see cref="OnDisplay"/> event
        /// </summary>
        /// <param name="sender">the </param>
        /// <param name="eventArgs"></param>
        [ComVisible(true)]
        public delegate void Pmpcontrol_EventHandler_OnDisplay(IPmpControl sender, OnDisplay_EventArgs eventArgs);

        /// <summary>
        /// fired a moment before property manager page is displayed
        /// </summary>
        public event Pmpcontrol_EventHandler_OnDisplay OnDisplay;

        /// <summary>
        /// fired when user starts interacting with this control, such as start of typing in a text box
        /// </summary>
        public event EventHandler OnGainedFocus;

        /// <summary>
        /// fires when user browses away from this control
        /// </summary>
        public event EventHandler OnLostFocus;

        #endregion
    }
}
