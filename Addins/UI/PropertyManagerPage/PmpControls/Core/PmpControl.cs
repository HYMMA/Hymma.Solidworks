
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a wrapper for solidworks property manager page controllers
    /// </summary>
    public abstract class PmpControl
    {
        private short _top;
        private bool _enabled = true;
        private bool _visible = true;
        private short _width;
        private short _left;
        private ControlResizeStyles _optionForResize;

        #region constructor

        internal PmpControl(swPropertyManagerPageControlType_e type, string caption, string tip)
        {
            Id = (short)Counter.GetNextPmpId();
            Type = type;
            Caption = caption;
            Tip = tip;
        }
        #endregion

        #region properties
        /// <summary>
        /// a caption or title for this controller
        /// </summary>
        public string Caption { get; }

        /// <summary>
        /// this property is automatically assigned to local app folder/AddinTtile/PmpId/ContollerId
        /// </summary>
        public DirectoryInfo SharedIconsDir { get; set; }

        /// <summary>
        /// toolTip (hint) for this controller
        /// </summary>
        public string Tip { get; set; }

        /// <summary>
        /// property manager page control as an object
        /// </summary>
        protected PropertyManagerPageControl Control { get; set; }

        /// <summary>
        /// type of this controller as defined in <see cref="swPropertyManagerPageControlType_e"/>
        /// </summary>
        public swPropertyManagerPageControlType_e Type { get; internal set; }

        /// <summary>
        /// id of this controller which is used by SOLIDWORKS to identify it
        /// </summary>
        public short Id { get; }

        /// <summary>
        /// Left alignment of this control as defined in <see cref="swPropertyManagerPageControlLeftAlign_e"/>
        /// </summary>
        /// <remarks>this property will be used when the page is displayed or while it is closed</remarks>
        private short LeftAlignment { get; set; } = (short)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;

        /// <summary>
        /// bitwise options as defined in <see cref="swAddControlOptions_e"/>, default value coresponds to a visible and enabled control
        /// </summary>
        private swAddControlOptions_e Options { get; set; } = swAddControlOptions_e.swControlOptions_Enabled | swAddControlOptions_e.swControlOptions_Visible;

        /// <summary>
        /// the solidworks document where the property manager page is displayed in. you can use this proeprty before the property manager page is displayed
        /// </summary>
        public ModelDoc2 ActiveDoc { get; internal set; }

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
                if (Control != null)
                    Control.Top = value;
                else
                    OnRegister += () => { Control.Top = value; };
            }
        }

        /// <summary>
        /// enables or disables this property control on
        /// </summary>
        public virtual bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                if (_enabled)
                    Options |= swAddControlOptions_e.swControlOptions_Enabled;
                else
                    Options &= ~swAddControlOptions_e.swControlOptions_Enabled;
                if (Control != null)
                    Control.Enabled = value;
                else
                    OnRegister += () => { Control.Enabled = value; };
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
                if (_visible)
                    Options |= swAddControlOptions_e.swControlOptions_Visible;
                else
                    Options &= ~swAddControlOptions_e.swControlOptions_Visible;

                if (Control != null)
                    Control.Visible = value;
                else
                    OnRegister += () => { Control.Visible = value; };
            }
        }


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
                if (Control != null)
                    Control.OptionsForResize = ((int)value);
                else
                    OnRegister += () => { Control.OptionsForResize = ((int)value); };
            }
        }

        /// <summary>
        /// Left edge of the control <br/>
        /// Use this proeprty and <see cref="Top"/> to palce controls side by side<br/>
        /// The value is in dialog units relative to the group box that the control is in. The left edge of the group box is 0; the right edge of the group box is 100
        /// </summary>
        /// <remarks>By default, the left edge of a control is either the left edge of its group box or indented a certain distance. this property overrides that default value</remarks>
        public short Left
        {
            get => _left;
            set
            {
                _left = value;
                if (Control != null)
                    Control.Left = value;
                else
                    OnRegister += () => { Control.Left = value; };
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
                if (Control != null)
                    Control.Width = value;
                else
                    OnRegister += () => { Control.Width = value; };
            }
        }
        #endregion

        #region methods

        /// <summary>
        /// Add this control to a group in a property manager page 
        /// </summary>
        /// <param name="group">the group that this contorl should be registerd to</param>
        internal void Register(IPropertyManagerPageGroup group)
        {
            Control = group.AddControl2(Id, (short)Type, Caption, LeftAlignment, ((int)Options), Tip) as PropertyManagerPageControl;
            _top = Control.Top;
            _width = Control.Width;
            _left = Control.Left;
            _visible = Control.Visible;
            _enabled = Control.Enabled;
            //we raise this event here to give multiple controls set-up their initial state. some of the proeprties of a controller has to be set prior a property manager page is displayed or after it's closed
            OnRegister?.Invoke();
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
            if (Control != null)
                SetPictureLabelForControl(bitmap, fileName);
            else
                OnRegister += () => { SetPictureLabelForControl(bitmap, fileName); };
        }
        private void SetPictureLabelForControl(Bitmap bitmap, string fileName)
        {
            var fullFileName = Path.Combine(SharedIconsDir.CreateSubdirectory(Id.ToString()).FullName, fileName);
            MaskedBitmap.SaveAsPng(bitmap, new Size(18, 18), ref fullFileName);
            Control.SetPictureLabelByName(fullFileName, "");
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
            if (Control != null)
                ShowBubbleTooltipForControl(title, message, bitmap, fileName);
            else
                OnRegister += () => { ShowBubbleTooltipForControl(title, message, bitmap, fileName); };
        }

        private void ShowBubbleTooltipForControl(string title, string message, Bitmap bitmap, string fileName)
        {
            var fullFileName = Path.Combine(SharedIconsDir.CreateSubdirectory(Id.ToString()).FullName, fileName);
            MaskedBitmap.SaveAsPng(bitmap, new Size(18, 18), ref fullFileName);
            Control.ShowBubbleTooltip(title, message, fullFileName);
        }
        #endregion

        #region call backs
        /// <summary>
        /// will be called just before this property manager page is displayed inside solidworks 
        /// </summary>
        ///<inheritdoc/>
        internal virtual void Display()
        {
            OnDisplay?.Invoke(this, new OnDisplay_EventArgs(Control));
        }

        internal virtual void GainedFocus()
        {
            OnGainedFocus?.Invoke(this, EventArgs.Empty);
        }

        internal virtual void LostFocus()
        {
            OnLostFocus?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region events
        /// <summary>
        /// fired when this controller is registerd in a property manager page which is when the add-in is loaded. Either when solidworks starts or when user re-loads the addin
        /// </summary>
        internal event Action OnRegister;

        /// <summary>
        /// event handler for a <see cref="OnDisplay"/> event
        /// </summary>
        /// <param name="sender">the </param>
        /// <param name="eventArgs"></param>
        [ComVisible(true)]
        public delegate void PmpcontrolOnDisplayEventHandler(PmpControl sender, OnDisplay_EventArgs eventArgs);

        /// <summary>
        /// fired a moment before property manager page is displayed
        /// </summary>
        public event PmpcontrolOnDisplayEventHandler OnDisplay;

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