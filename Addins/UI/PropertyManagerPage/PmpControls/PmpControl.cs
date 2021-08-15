using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Drawing;

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
        private bool _leftEdgeChanged;
        private short _leftEdge;
        private short _width = 100;
        private short _top;
        private int _optionForResize = (int)PmpResizeStyles.LockLeft | (int)PmpResizeStyles.LockRight;
        private bool _enabled = true;
        private bool _visible = true;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="type">type of this controller as per <see cref="swPropertyManagerPageControlType_e"/></param>
        /// <param name="caption">caption for this control</param>
        /// <param name="tip">tip for this control</param>
        public PmpControl(swPropertyManagerPageControlType_e type, string caption = "", string tip = "")
        {
            Type = type;
            Caption = caption;
            Tip = tip;
            OnRegister += PmpControl_OnRegister;
        }

        private void PmpControl_OnRegister()
        {
            SolidworksObject = (T)ControlObject;
            _control = SolidworksObject as IPropertyManagerPageControl;
            _control.Width = Width;

            //Solidworks automatically adjusts the leftedge in some controls like selection boxes whit up and down arrows
            //so we set this property only when user has specifically changed it
            if (_leftEdgeChanged)
                _control.Left = LeftEdge;

            _control.OptionsForResize = OptionsForResize;
            _control.Enabled = Enabled;
            _control.Visible = Visible;
        }

        /// <summary>
        /// Sets the bitmap label for this control on a PropertyManager page.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="fileName">resultant bitmap file name on disk without extensions or directory</param>
        /// <remarks>
        /// You can only use this method on a PropertyManager page before the page is displayed, while it is displayed, or when it is closed. <br/>
        /// The image will be resized to 18 x 18
        /// </remarks>
        public virtual void SetBitmap(Bitmap bitmap, string fileName)
        {
            if (_control == null) return;
            IconGenerator.GetPmpControlIcon(bitmap, fileName, out string image, out string maskedImage);
            _control.SetPictureLabelByName(image, maskedImage);
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
            if (_control == null) return;
            IconGenerator.GetPmpControlIcon(bitmap, fileName, out string image, out string maskedImage);
            _control.ShowBubbleTooltip(title, message, image);
        }


        /// <summary>
        /// Left edge of the control <br/>
        /// This property will be set the next time the control is displayed.<br/>
        /// Use this proeprty and <see cref="Top"/> to palce controls side by side<br/>
        /// The value is in dialog units relative to the group box that the control is in. The left edge of the group box is 0; the right edge of the group box is 100
        /// </summary>
        /// <remarks>By default, the left edge of a control is either the left edge of its group box or indented a certain distance. this property overrides that default value</remarks>
        public short LeftEdge
        {
            get => _leftEdge;
            set
            {
                _leftEdgeChanged = true;
                _leftEdge = value;
                if (_control != null)
                    _control.Left = value;
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
            }
        }

        /// <summary>
        /// Gets or sets the top edge of the control on a PropertyManager page
        /// </summary>
        /// <remarks>returns -1 on error</remarks>
        public short Top
        {
            get => _top;
            set
            {
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
        public T SolidworksObject { get; set; }

    }

    /// <summary>
    /// PropertyManager page control resize options. Bitmask. 
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>
    /// <term>LockLeft </term>
    /// <description>the control is locked in place relative to the left edge of the PropertyManager page. <br/>
    /// When the page width is changed, the control stays in place and its width does not change.</description>
    /// </item>
    /// <item>
    /// <term>LockRight</term>
    /// <description>the control is locked in place relative to the right edge of the PropertyManager page.<br/>
    /// When the page width is changed, the control shifts to the right, but its width does not change.</description>
    /// </item>
    /// <item>
    /// <term>LockLeft and LockRight specified</term>
    /// <description>the left edge of the control stays in place relative to the left edge and the right edge of the control stays in place relative to the right edge of the PropertyManager page,<br/>
    /// giving the effect that the control grows and shrinks with the PropertyManager page.</description>
    /// </item>
    /// </list></remarks>
    public enum PmpResizeStyles
    {
        /// <summary>
        /// the control is locked in place relative to the left edge of the PropertyManager page. <br/>
        /// When the page width is changed, the control stays in place and its width does not change.
        /// </summary>
        LockLeft = 1,

        /// <summary>
        /// the control is locked in place relative to the right edge of the PropertyManager page.<br/>
        /// When the page width is changed, the control shifts to the right, but its width does not change.
        /// </summary>
        LockRight = 2
    }
}
