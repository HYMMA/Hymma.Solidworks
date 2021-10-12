using SolidWorks.Interop.sldworks;
using System;
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
        private byte _opacity;
        private string _filename;
        private ControlResizeStyles _resizeStyles;

        /// <summary>
        /// generates a bitmap in the property manager page
        /// </summary>
        /// <param name="bitmap">bitmap to edit and set in the property manager page</param>
        /// <param name="fileName">resultant bitmap file name on disk without extensions or directory</param>
        /// <param name="resizeStyles">resize option as defined in <see cref="ControlResizeStyles"/></param>
        /// <param name="opacity">define opacity of the image. 255 is th emax possible value, less values result in more transparent pictures</param>
        /// <remarks>The typical image format for the two SOLIDWORKS bitmaps is 18 x 18 pixels x 256 colors. <br/>
        /// </remarks>
        public PmpBitmap(Bitmap bitmap, string fileName, ControlResizeStyles resizeStyles=ControlResizeStyles.LockLeft, byte opacity = 255) : base(SolidWorks.Interop.swconst.swPropertyManagerPageControlType_e.swControlType_Bitmap)
        {
            OnRegister += PmpBitmap_OnRegister;
            _bitmap = bitmap;
            _opacity = opacity;
            _filename = string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));
            _resizeStyles = resizeStyles;
            OnDisplay += PmpBitmap_OnDisplay;
        }

        private void PmpBitmap_OnDisplay(IPmpControl sender, OnDisplay_EventArgs eventArgs)
        {
            eventArgs.OptionsForResize = (int)_resizeStyles;
        }

        private void PmpBitmap_OnRegister()
        {
            UpdatePicture(_bitmap, _filename,_opacity);
        }

        /// <summary>
        /// Sets the bitmap for this control. 
        /// </summary>
        /// <param name="bitmap">bitmap to edit and set in the property manager page</param>
        /// <param name="fileName">resultant bitmap file name on disk without extensions or directory</param>
        /// <param name="opacity">define opacity of the image. less values result in more transparent pictures</param>
        /// <remarks>The typical image format for the two SOLIDWORKS bitmaps is 18 x 18 pixels x 256 colors. <br/>
        /// Using this method, you can specify different sizes e.g. 24 x 90. 
        /// <para>
        /// You can use this method during the PropertyManager page is displayed or closed. If you use this method when the PropertyManager page is displayed, use bitmaps that are the same size.
        /// </para>
        /// </remarks>
        public void UpdatePicture(Bitmap bitmap, string fileName, byte opacity)
        {
            if (SolidworksObject == null || string.IsNullOrEmpty(fileName))
                return;

            var fullFileName = Path.Combine(IconGenerator.GetDefaultIconFolder(), fileName);
            MaskedBitmap.SaveAsPng(bitmap,bitmap.Size, ref fullFileName, true, opacity);
            SolidworksObject.SetBitmapByName(fullFileName, "");
        }


    }
}
