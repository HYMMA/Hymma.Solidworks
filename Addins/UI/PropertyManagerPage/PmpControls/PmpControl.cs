using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
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
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="type">type of this controller as per <see cref="swPropertyManagerPageControlType_e"/></param>
        public PmpControl(swPropertyManagerPageControlType_e type)
        {
            Type = type;
            OnRegister += PmpControl_OnRegister;
            OnDisplay += PmpControl_OnDisplay;
        }

        private void PmpControl_OnDisplay()
        {
            _control.Visible = Visible;
            _control.Enabled = Enabled;
            _control.Width = Width;
            _control.Left = LeftEdge;
            _control.OptionsForResize = OptionsForResize;
        }

        private void PmpControl_OnRegister()
        {
            SolidworksObject = (T)ControlObject;
            _control = SolidworksObject as IPropertyManagerPageControl;
            PmpControl_OnDisplay();
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
            IconGenerator.GetPmpControlIcon(bitmap, fileName, out string image, out string maskedImage);
            _control.ShowBubbleTooltip(title, message, image);
        }


        /// <summary>
        /// Left edge of the control <br/>
        /// This property must be set before the control is displayed.<br/>
        /// The value is in dialog units relative to the group box that the control is in. The left edge of the group box is 0; the right edge of the group box is 100
        /// </summary>
        /// <remarks>By default, the left edge of a control is either the left edge of its group box or indented a certain distance. This is determined by the <see cref="LeftAlignment"/></remarks>
        public short LeftEdge { get; set; }

        /// <summary>
        /// By default, the width of the control is usually set so that it extends to the right edge of its group box (not for buttons). Using this API overrides that default.<br/>
        /// The value is in dialog units relative to the group box that the control is in. The width of the group box is 100
        /// </summary>
        public short Width { get; set; } = 100;

        /// <summary>
        /// Gets or sets the top edge of the control on a PropertyManager page
        /// </summary>
        public short? Top { get => _control?.Top; set => _control.Top = value.GetValueOrDefault(); } 

        /// <summary>
        /// Gets or sets how to override the SOLIDWORKS default behavior when changing the width of a PropertyManager page. <br/>
        /// Resize the PropertyManager page as defined in <see cref="PmpResizeStyles"/>
        /// </summary>
        public int OptionsForResize { get; set; } = (int)PmpResizeStyles.LockLeft | (int)PmpResizeStyles.LockRight;

        /// <summary>
        /// enables or disables this property control on
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// gets or sets the visibility of this control
        /// </summary>
        public bool Visible { get; set; } = true;

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
