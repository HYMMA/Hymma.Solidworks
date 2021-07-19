using SolidWorks.Interop.sldworks;
using System.Drawing;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// picture inside a property manager page
    /// </summary>
    public class PmpBitmap : PmpControl<PropertyManagerPageBitmap>
    {
        /// <summary>
        /// generates a bitmap in the property manager page
        /// </summary>
        /// <param name="colorBitmap">Full path and filename of the bitmap on disk</param>
        /// <param name="maskBitmap">Full path and filename of the alpha mask bitmap on disk</param>
        /// <remarks>The typical image format for the two SOLIDWORKS bitmaps is 18 x 18 pixels x 256 colors. <br/>
        /// </remarks>
        public PmpBitmap(string colorBitmap, string maskBitmap) : base(SolidWorks.Interop.swconst.swPropertyManagerPageControlType_e.swControlType_Bitmap)
        {
            this.SetPictureLabelByName(colorBitmap, maskBitmap);
        }

        /// <summary>
        /// Sets the bitmap for this control. 
        /// </summary>
        /// <param name="colorBitmap">Full path and filename of the bitmap on disk</param>
        /// <param name="maskBitmap">Full path and filename of the alpha mask bitmap on disk</param>
        /// <remarks>The typical image format for the two SOLIDWORKS bitmaps is 18 x 18 pixels x 256 colors. <br/>
        /// Using this method, you can specify a bigger bitmap, e.g., 24 x 24 pixels, to get extra detail. The pixels in MaskBitmap specify transparency through shades of grey with boundaries of black pixels = 100% opaque and white pixels = 100% transparent.
        /// <para>
        /// You can use this method before, during, or after the PropertyManager page is displayed or closed. If you use this method when the PropertyManager page is displayed, use bitmaps that are the same size.
        /// </para>
        /// </remarks>
        public override void SetPictureLabelByName(string colorBitmap, string maskBitmap)
        {
            SolidworksObject.SetBitmapByName(colorBitmap, maskBitmap);
        }
    }
}
