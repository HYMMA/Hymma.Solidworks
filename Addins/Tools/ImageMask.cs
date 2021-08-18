using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// adds an alpha layer to a bitmap file
    /// </summary>
    public static class ImageMask
    {
        /// <summary>
        /// “24 bit” usually means 24 bits total per pixel, with 8 bits per channel for red, green and blue, or 16,777,216 total colours. This is sometimes referred to as 24 bit RGB<br/>
        /// “32 bit” also usually means 32 bits total per pixel, and 8 bits per channel, with an additional 8 bit alpha channel that’s used for transparency. 16,777,216 colours again.<br/> 
        /// This is sometimes referred to as 32 bit RGBA.
        /// </summary>
        /// <param name="tmpImage"></param>
        /// <returns></returns>
        private static Bitmap Create32bppImageAndClearAlpha(Bitmap tmpImage)
        {
            // declare the new image that will be returned by the function
            Bitmap returnedImage = new Bitmap(tmpImage.Width, tmpImage.Height, PixelFormat.Format32bppArgb);

            // create a graphics instance to draw the original image in the new one
            Rectangle rect = new Rectangle(0, 0, tmpImage.Width, tmpImage.Height);
            Graphics g = Graphics.FromImage(returnedImage);

            // create an image attribe to force a clearing of the alpha layer
            ImageAttributes imageAttributes = new ImageAttributes();
            float[][] colorMatrixElements = {
                        new float[] {1,0,0,0,0},
                        new float[] {0,1,0,0,0},
                        new float[] {0,0,1,0,0},
                        new float[] {0,0,0,0,0},
                        new float[] {0,0,0,1,1}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            // draw the original image 
            g.DrawImage(tmpImage, rect, 0, 0, tmpImage.Width, tmpImage.Height, GraphicsUnit.Pixel, imageAttributes);
            g.Dispose();
            return returnedImage;
        }

        /// <summary>
        /// creates a masked bitmap (.bmp) from original bitmap provided
        /// </summary>
        /// <param name="image">original bitmap</param>
        /// <param name="opaque">allows a semi opaq (grey) masked iamge. </param>
        /// <param name="OpacityThreshold">parameter determines the opacity of the masked image. max is 255</param>
        /// <param name="invertedMask">inverts the colors of masked image </param>
        /// <returns>returns masked png format of the image or the image itself if the image parameter was of type png</returns>
        public static Bitmap GetMask(Bitmap image, bool opaque = false, int OpacityThreshold = 128, bool invertedMask = false)
        {
            if (image.RawFormat.Equals(ImageFormat.Png))
                return image;

            Bitmap maskImage = Create32bppImageAndClearAlpha(image);

            BitmapData bmpData = maskImage.LockBits(new Rectangle(0, 0, maskImage.Width, maskImage.Height), ImageLockMode.ReadWrite, maskImage.PixelFormat);

            // Declare an array to hold the bytes of the bitmap.
            byte[] maskImageRGBData = new byte[Math.Abs(bmpData.Stride) * bmpData.Height];

            // Get the address of the first line. and copy into array
            Marshal.Copy(bmpData.Scan0, maskImageRGBData, 0, maskImageRGBData.Length);

            byte greyLevel;
            for (int i = 0; i + 2 < maskImageRGBData.Length; i += 4)
            {
                //convert to gray scale R:0.30 G=0.59 B 0.11
                greyLevel = (byte)(0.3 * maskImageRGBData[i + 2] + 0.59 * maskImageRGBData[i + 1] + 0.11 * maskImageRGBData[i]);

                if (opaque)
                {
                    //check max on opacityThreshold
                    if (OpacityThreshold > 255)
                        throw new ArgumentOutOfRangeException(nameof(OpacityThreshold));
                    greyLevel = (greyLevel < OpacityThreshold) ? byte.MinValue : byte.MaxValue;
                }
                if (invertedMask)
                {
                    greyLevel = (byte)(255 - (int)greyLevel);
                }

                maskImageRGBData[i] = greyLevel;
                maskImageRGBData[i + 1] = greyLevel;
                maskImageRGBData[i + 2] = greyLevel;

            }
            Marshal.Copy(maskImageRGBData, 0, bmpData.Scan0, maskImageRGBData.Length);
            maskImage.UnlockBits(bmpData);
            return GetMaskedImage(image, maskImage);
            //return maskImage;
        }

        private static Bitmap GetMaskedImage(Bitmap loadedImage, Bitmap maskImage)
        {
            if (loadedImage == null && maskImage == null)
                return null;
         
            //allocate the Masked image in ARGB format
            var maskedImage = Create32bppImageAndClearAlpha(loadedImage);

            BitmapData bmpData1 = maskedImage.LockBits(new Rectangle(0, 0, maskedImage.Width, maskedImage.Height), ImageLockMode.ReadWrite, maskedImage.PixelFormat);
            byte[] maskedImageRGBAData = new byte[bmpData1.Stride * bmpData1.Height];
            Marshal.Copy(bmpData1.Scan0, maskedImageRGBAData, 0, maskedImageRGBAData.Length);

            BitmapData bmpData2 = maskImage.LockBits(new Rectangle(0, 0, maskImage.Width, maskImage.Height), ImageLockMode.ReadOnly, maskImage.PixelFormat);
            byte[] maskImageRGBAData = new byte[bmpData2.Stride * bmpData2.Height];
            Marshal.Copy(bmpData2.Scan0, maskImageRGBAData, 0, maskImageRGBAData.Length);

            //copy the mask to the Alpha layer
            for (int i = 0; i + 2 < maskedImageRGBAData.Length; i += 4)
            {
                maskedImageRGBAData[i + 3] = maskImageRGBAData[i];
            }
            Marshal.Copy(maskedImageRGBAData, 0, bmpData1.Scan0, maskedImageRGBAData.Length);
            maskedImage.UnlockBits(bmpData1);
            maskImage.UnlockBits(bmpData2);

            return maskedImage;
        }
    }
}
