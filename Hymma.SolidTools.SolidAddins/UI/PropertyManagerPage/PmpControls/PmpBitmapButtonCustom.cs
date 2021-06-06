using System.Collections.Generic;
using System.Drawing;

namespace Hymma.SolidTools.SolidAddins
{
    public class PmpBitmapButtonCustom : PmpControl
    {

        /// <summary>
        /// a customised bitmap button for property manager pages
        /// </summary>
        public PmpBitmapButtonCustom():base(SolidWorks.Interop.swconst.swPropertyManagerPageControlType_e.swControlType_BitmapButton)
        {
        }

        /// <summary>
        /// <list type="bullet">
        /// <listheader> images should be</listheader>
        /// <item>20 x 20 pixels </item>
        /// <item>32 x 32 pixels </item>
        /// <item>40 x 40 pixels </item>
        /// <item>64 x 64 pixels </item>
        /// <item>96 x 96 pixels </item>
        /// <item>128 x 128 pixels </item>
        /// </list>
        /// </summary>
        public string[] ImageList { get; set; }

        /// <summary>
        /// Specify your own image masks using the MaskImageList argument, which should display the images exactly as you created them. <br/>
        /// If you specify an empty array for MaskImageList, then SOLIDWORKS generates what it needs; however, the images might appear blurred.<br/>
        /// <remark>
        /// Portable Network Graphics images (.png) do not support masking. <br/>
        /// To use this method with .png files, pass an array of .png file names to ImageList and a blank string for each item in MaskImageList
        /// that corresponds to an item in ImageList</remark>
        /// </summary>
        public string[] MaskedImageList { get; set; }
    }
}
