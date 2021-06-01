using SolidWorks.Interop.swconst;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// a wrapper for solidworks property manager page controllers
    /// </summary>
    public  abstract class SwPMPControl 
    {
        #region private fields

        private swPropertyManagerPageControlType_e _type;
        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="type">type of this controller as per <see cref="swPropertyManagerPageControlType_e"/></param>
        public SwPMPControl(swPropertyManagerPageControlType_e type)
        {
            this._type = type;
        }
        
        /// <summary>
        /// type of this controller as defined in <see cref="swPropertyManagerPageControlType_e"/>
        /// </summary>
        public swPropertyManagerPageControlType_e Type { get { return _type; } protected set { _type = value; } }

        /// <summary>
        /// bitmap to display in bubble ToolTip
        /// </summary>
        public abstract string BitmapBubble { get; set; }
              
        /// <summary>
        /// a caption or title for this controller
        /// </summary>
        public abstract string Caption { get; set; }
              
        /// <summary>
        /// tip for this controller
        /// </summary>
        public abstract string Tip { get; set; }
              
        /// <summary>
        /// id of this controller which gets used in command box
        /// </summary>
        public abstract  short Id { get;internal set; }

        /// <summary>
        /// default is 1<br/>
        /// The value is in dialog units relative to the group box that the control is in. The left edge of the group box is 0; the right edge of the group box is 100
        /// </summary>
        public virtual short LeftIndet { get; set; } = 1;

        /// <summary>
        /// bitwise options as defined in <see cref="swAddControlOptions_e"/>, default value coresponds to a visible and enabled control
        /// </summary>
        public virtual  int Options { get; set; } = 3;
        
        /// <summary>
        /// the message body of the bubble tooltip
        /// </summary>
        public abstract string BubbleMessage { get; set; }

        /// <summary>
        /// color bitmap for picture lable<br/>
        /// The image format for the bitmap is 18 x 18 pixels x 256 colors.<br/>
        /// </summary>
        public abstract  string ColorBitmap { get; set; }

        /// <summary>
        /// mask bitmap for picture lable<br/>
        /// The image format for the bitmap is 18 x 18 pixels x 256 colors.<br/>
        /// The pixels in MaskBitmap specify transparency through shades of grey with boundaries of black pixels = 100% opaque and white pixels = 100% transparent.
        /// </summary>
        public abstract  string MaskBitmap { get; set; }

        /// <summary>
        /// use this to override the left indent<br/>
        /// </summary>
        public abstract  short Left { get; set; }
        /// <summary>
        /// By default, the width of the control is usually set so that it extends to the right edge of its group box (not for buttons). Using this API overrides that default.<br/>
        /// The value is in dialog units relative to the group box that the control is in. The width of the group box is 100
        /// </summary>
        
        public abstract  short Width { get; set; }
        /// <summary>
        /// Gets or sets the top edge of the control on a PropertyManager page
        /// </summary>
        
        public abstract  short Top { get; set; }

        /// <summary>
        /// Gets or sets how to override the SOLIDWORKS default behavior when changing the width of a PropertyManager page. <br/>
        /// Resize the PropertyManager page as defined in <see cref="swPropMgrPageControlOnResizeOptions_e"/>
        /// <list type="bullet">
        /// <item>
        /// <term>swControlOptionsOnResize_LockLeft </term>
        /// <description>the control is locked in place relative to the left edge of the PropertyManager page. <br/>
        /// When the page width is changed, the control stays in place and its width does not change.</description>
        /// </item>
        /// <item>
        /// <term>swControlOptionsOnResize_LockRight</term>
        /// <description>the control is locked in place relative to the right edge of the PropertyManager page.<br/>
        /// When the page width is changed, the control shifts to the right, but its width does not change.</description>
        /// </item>
        /// <item>
        /// <term>swControlOptionsOnResize_LockLeft and swControlOptionsOnResize_LockRight specified</term>
        /// <description>the left edge of the control stays in place relative to the left edge and the right edge of the control stays in place relative to the right edge of the PropertyManager page,<br/>
        /// giving the effect that the control grows and shrinks with the PropertyManager page.</description>
        /// </item>
        /// </list>
        /// </summary>
        public abstract  int OptionsForResize { get; set; }

        /// <summary>
        /// enables or disables this property control on
        /// </summary>
        public virtual bool Enabled { get; set; } = true;

        /// <summary>
        /// gets or sets the visibility of thei control
        /// </summary>
        public virtual bool Visible { get; set; } = true;
    }
}