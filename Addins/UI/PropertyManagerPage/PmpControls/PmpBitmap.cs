using System.Drawing;

namespace Hymma.SolidTools.Addins
{
    public class PmpBitmap : PmpControl
    {
        public PmpBitmap() : base(SolidWorks.Interop.swconst.swPropertyManagerPageControlType_e.swControlType_Bitmap)
        {

        }
        /// <summary>
        /// The typical image format for the two SOLIDWORKS bitmaps is 18 x 18 pixels x 256 colors. Using this method, you can specify a bigger bitmap, e.g., 24 x 24 pixels, to get extra detail.<br/>
        /// The pixels in MaskBitmap specify transparency through shades of grey with boundaries of black pixels = 100% opaque and white pixels = 100% transparent.
        /// </summary>
        public string ColorBitmap { get; set; }
        /// <summary>
        /// The typical image format for the two SOLIDWORKS bitmaps is 18 x 18 pixels x 256 colors. Using this method, you can specify a bigger bitmap, e.g., 24 x 24 pixels, to get extra detail.<br/>
        /// The pixels in MaskBitmap specify transparency through shades of grey with boundaries of black pixels = 100% opaque and white pixels = 100% transparent.
        /// </summary>
        public string MaskBitmap { get; set; }
    }
}
