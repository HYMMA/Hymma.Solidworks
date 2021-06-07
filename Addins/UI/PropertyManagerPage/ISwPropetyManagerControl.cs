using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// A solidworks Property manager control
    /// </summary>
    public interface ISwPropetyManagerControl
    {
        /// <summary>
        /// type of this controller as defined in <see cref="swPropertyManagerPageControlType_e"/>
        /// </summary>
        int Type { get;}

        /// <summary>
        /// bitmap to display in bubble ToolTip
        /// </summary>
        BitMap BitmapBubble { get; set; }

        /// <summary>
        /// a caption or title for this controller
        /// </summary>
        string Caption { get; set; }

        /// <summary>
        /// tip for this controller
        /// </summary>
        string Tip { get; set; }

        /// <summary>
        /// id of this controller which gets used in command box
        /// </summary>
        short Id { get; set; }

        /// <summary>
        /// default is 1<br/>
        /// The value is in dialog units relative to the group box that the control is in. The left edge of the group box is 0; the right edge of the group box is 100
        /// </summary>
        short LeftIndet { get; set; }

        /// <summary>
        /// bitwise options as defined in <see cref="swAddControlOptions_e"/>, default value coresponds to a visible and enabled control
        /// </summary>
        int Options { get; set; }

        /// <summary>
        /// a property manager page group box that houses this controller
        /// </summary>
        SwGroupBox Box { get; set; }

        /// <summary>
        /// the message body of the bubble tooltip
        /// </summary>
        string BubbleMessage { get; set; }

        /// <summary>
        /// color bitmap for picture lable<br/>
        /// The image format for the bitmap is 18 x 18 pixels x 256 colors.<br/>
        /// </summary>
        string ColorBitmap { get; set; }

        /// <summary>
        /// mask bitmap for picture lable<br/>
        /// The image format for the bitmap is 18 x 18 pixels x 256 colors.<br/>
        /// The pixels in MaskBitmap specify transparency through shades of grey with boundaries of black pixels = 100% opaque and white pixels = 100% transparent.
        /// </summary>
        string MaskBitmap { get; set; }

        /// <summary>
        /// use this to override the left indent<br/>
        /// </summary>
        short Left { get; set; }
        /// <summary>
        /// By default, the width of the control is usually set so that it extends to the right edge of its group box (not for buttons). Using this API overrides that default.<br/>
        /// The value is in dialog units relative to the group box that the control is in. The width of the group box is 100
        /// </summary>
        short Width { get; set; }
        /// <summary>
        /// Gets or sets the top edge of the control on a PropertyManager page
        /// </summary>
        short Top { get; set; }

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
        int OptionsForResize { get; set; }

        /// <summary>
        /// enables or disables this property control on
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// gets or sets the visibility of thei control
        /// </summary>
        bool Visible { get; set; }
    }
}