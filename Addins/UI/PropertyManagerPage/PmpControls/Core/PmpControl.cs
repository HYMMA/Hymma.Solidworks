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
        private PropertyManagerPageControl _control;
        private short _top;
        private bool _enabled = true;
        private bool _visible = true;
        private short _width;
        private short _left;
        private ControlResizeStyles _optionForResize;
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
            OnRegister += () =>
            {
                SolidworksObject = (T)ControlObject;
                _control = ControlObject as PropertyManagerPageControl;
            };
        }
        #endregion

        #region call backs
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
        /// <remarks>returns -1 on error, <strong>MUST USE NUMERICAL VALUE TO SEE THE EFFECT</strong></remarks>
        public short Top
        {
            get => _top;
            set
            {
                _top = value;
                if (_control != null)
                    _control.Top = value;
                else
                    OnRegister += () => { _control.Top = value; };
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
                else
                    OnRegister += () => { _control.Enabled = value; };
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
                else
                    OnRegister += () => { _control.Visible = value; };
            }
        }

        ///<inheritdoc/>
        public T SolidworksObject { get; internal set; }

        #endregion

        #region methods

        /// <summary>
        /// Gets or sets how to override the SOLIDWORKS default behavior when changing the width of a PropertyManager page. <br/>
        /// Resize the PropertyManager page as defined in <see cref="ControlResizeStyles"/>
        /// you can use ths porperty only before the control is displayed or while it is closed
        /// </summary>
        public ControlResizeStyles ResizeStyles
        {
            get => _optionForResize;
            set
            {
                _optionForResize = value;
                if (_control != null)
                    _control.OptionsForResize = ((int)value);
                else
                    OnRegister += () => { _control.OptionsForResize = ((int)value); };
            }
        }

        /// <summary>
        /// Left edge of the control <br/>
        /// Use this proeprty and <see cref="PmpControl{T}.Top"/> to palce controls side by side<br/>
        /// The value is in dialog units relative to the group box that the control is in. The left edge of the group box is 0; the right edge of the group box is 100
        /// </summary>
        /// <remarks>By default, the left edge of a control is either the left edge of its group box or indented a certain distance. this property overrides that default value</remarks>
        public short Left
        {
            get => _left;
            set
            {
                _left = value;
                if (_control != null)
                    _control.Left = value;
                else
                    OnRegister += () => { _control.Left = value; };
            }
        }

        /// <summary>
        /// By default, the width of the control is usually set so that it extends to the right edge of its group box (not for buttons). Using this API overrides that default.<br/>
        /// The value is in dialog units relative to the group box that the control is in. The width of the group box is 100
        /// </summary>
        public short Width
        {
            get => _width;
            set
            {
                _width = value;
                if (_control != null)
                    _control.Width = value;
                else
                    OnRegister += () => { _control.Width = value; };
            }
        }


        /// <summary>
        /// Sets the bitmap label for this control that appears next to it on the left hand side.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="fileName">resultant bitmap file name on disk without extensions or directory</param>
        /// <remarks>
        /// You can only use this method on a PropertyManager page before the page is displayed, while it is displayed, or when it is closed. <br/>
        /// The image will be resized to 18 x 18
        /// </remarks>
        public void SetPictureLabel(Bitmap bitmap, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;
            if (_control != null)
                SetPictureLabelForControl(bitmap, fileName);
            else
                OnRegister += () => { SetPictureLabelForControl(bitmap, fileName); };
        }
        private void SetPictureLabelForControl(Bitmap bitmap, string fileName)
        {
            var fullFileName = Path.Combine(IconGenerator.GetDefaultIconFolder(), fileName);
            MaskedBitmap.SaveAsPng(bitmap,new Size(18,18), ref fullFileName);
            _control.SetPictureLabelByName(fullFileName, "");
        }

        /// <summary>
        /// Displays a bubble ToolTip containing the specified title, message, and bitmap. A bubble ToolTip is useful for validating data typed or selected by users in controls on a PropertyManager page.
        /// </summary>
        /// <param name="title">Title to display in bubble ToolTip</param>
        /// <param name="message">Message to display in bubble ToolTip</param>
        /// <param name="bitmap">bitmap object to use as icon in the tooltip</param>
        /// <param name="fileName">resultant bitmap file name on disk without extensions or directory</param>
        public void ShowBubleTooltip(string title, string message, Bitmap bitmap, string fileName)
        {
            if (_control != null)
                ShowBubbleTooltipForControl(title, message, bitmap, fileName);
            else
                OnRegister += () => { ShowBubbleTooltipForControl(title, message, bitmap, fileName); };
        }

        private void ShowBubbleTooltipForControl(string title, string message, Bitmap bitmap, string fileName)
        {
            var fullFileName = Path.Combine(IconGenerator.GetDefaultIconFolder(), fileName);
            MaskedBitmap.SaveAsPng(bitmap, new Size(18, 18), ref fullFileName);
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
