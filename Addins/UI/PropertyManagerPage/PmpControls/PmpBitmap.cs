using SolidWorks.Interop.sldworks;
using System.Drawing;
using System.IO;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// picture inside a property manager page
    /// </summary>
    public class PmpBitmap : PmpControl<PropertyManagerPageBitmap>
    {
        private Bitmap _bitmap;
        private string _filename;

        /// <summary>
        /// generates a bitmap in the property manager page
        /// </summary>
        /// <param name="bitmap">bitmap to edit and set in the property manager page</param>
        /// <param name="fileName">resultant bitmap file name on disk without extensions or directory</param>
        /// <remarks>The typical image format for the two SOLIDWORKS bitmaps is 18 x 18 pixels x 256 colors. <br/>
        /// </remarks>
        public PmpBitmap(Bitmap bitmap, string fileName) : base(SolidWorks.Interop.swconst.swPropertyManagerPageControlType_e.swControlType_Bitmap)
        {
            OnRegister += PmpBitmap_OnRegister;
            _bitmap = bitmap;
            _filename = string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
        }

        private void PmpBitmap_OnRegister()
        {
            SetBitmap(_bitmap, _filename);
        }

        /// <summary>
        /// Sets the bitmap for this control. 
        /// </summary>
        /// <param name="bitmap">bitmap to edit and set in the property manager page</param>
        /// <param name="fileName">resultant bitmap file name on disk without extensions or directory</param>
        /// <remarks>The typical image format for the two SOLIDWORKS bitmaps is 18 x 18 pixels x 256 colors. <br/>
        /// Using this method, you can specify different sizes e.g. 24 x 90. If bitmap size is not square e.g (24 x 24 or 18 x 18) this method will resize it to suit
        /// <para>
        /// You can use this method before, during, or after the PropertyManager page is displayed or closed. If you use this method when the PropertyManager page is displayed, use bitmaps that are the same size.
        /// </para>
        /// </remarks>
        public override void SetBitmap(Bitmap bitmap, string fileName)
        {
            IconGenerator.GetPmpBitmapIcon(bitmap, fileName, out string image, out string maskImage);
            if (SolidworksObject != null)
                SolidworksObject.SetBitmapByName(image, maskImage);
        }
    }
}
