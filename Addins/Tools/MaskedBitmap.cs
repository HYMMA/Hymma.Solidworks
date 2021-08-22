using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// adds an alpha layer to a bitmap file
    /// </summary>
    public sealed class MaskedBitmap
    {
        /// <summary>
        /// full file name of the masked image
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// full file name of the image mask
        /// </summary>
        public string Mask { get; set; }

        /// <summary>
        /// coverts and saves a bitmap to specified location
        /// </summary>
        /// <param name="image">file to get bitmask for</param>
        /// <param name="fullFileName">file name with or without extension, this method will save the image as .png file format only</param>
        /// <param name="allowPartialOpacity">if set true the number assigned to "opacityThreshold" will be considered as entered</param>
        /// <param name="opacityThreshold">a number between 0 and 255 maximum</param>
        /// <param name="invertedMask"></param>
        public static void Save(Bitmap image,ref string fullFileName, bool allowPartialOpacity = false, int opacityThreshold = 255, bool invertedMask = true)
        {
            //check for valid file name . . .
            if (string.IsNullOrEmpty(fullFileName))
                throw new ArgumentNullException("filename assigned to icon was empty");

            //if the string provided does not have .png ...
            if (!fullFileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))

                //add the extension to the file name
                fullFileName += ".png";

            //get maskImage
            if (!File.Exists(fullFileName))
            {

                Bitmap maskImage;

                //if type of image provided is png we simply save it without processing it
                if (image.RawFormat.Equals(ImageFormat.Png))
                {
                    maskImage = image;
                }
                else
                {
                    maskImage = GetMaskedImage(image, allowPartialOpacity, opacityThreshold, invertedMask);
                }

                using (maskImage)
                {
                    maskImage.Save(fullFileName);
                }
            }
        }

        /// <summary>
        /// get the maske for a bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="allowPartialOpacity">if set true the number assigned to "opacityThreshold" will be considered as entered</param>
        /// <param name="opacityThreshold">a number between 0 and 255 maximum</param>
        /// <param name="invertedMask"></param>
        /// <returns></returns>
        public static Bitmap GetImageMask(Bitmap bitmap, bool allowPartialOpacity = false, int opacityThreshold = 255, bool invertedMask = true)
        {
            var maskImage = Create32bppImageAndClearAlpha(bitmap);

            BitmapData bmpData = maskImage.LockBits(new Rectangle(0, 0, maskImage.Width, maskImage.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, maskImage.PixelFormat);

            byte[] maskImageRGBData = new byte[bmpData.Stride * bmpData.Height];

            Marshal.Copy(bmpData.Scan0, maskImageRGBData, 0, maskImageRGBData.Length);

            byte greyLevel;
            for (int i = 0; i + 2 < maskImageRGBData.Length; i += 4)
            {
                //convert to gray scale R:0.30 G=0.59 B 0.11
                greyLevel = (byte)(0.3 * maskImageRGBData[i + 2] + 0.59 * maskImageRGBData[i + 1] + 0.11 * maskImageRGBData[i]);

                if (!allowPartialOpacity)
                {
                    greyLevel = (greyLevel < opacityThreshold) ? byte.MinValue : byte.MaxValue;
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
            return maskImage;
        }

        public static Bitmap GetMaskedImage(Bitmap loadedImage, bool allowPartialOpacity = false, int opacityThreshold = 255, bool invertedMask = true)
        {
            if (loadedImage == null)
                throw new ArgumentNullException(nameof(loadedImage));
            var maskImage = GetImageMask(loadedImage, allowPartialOpacity, opacityThreshold, invertedMask);
            if (loadedImage.Width != maskImage.Width || loadedImage.Height != maskImage.Height)
            {
                throw new ArgumentOutOfRangeException("Error: mask and image must have the same size");
            }
            else
            {
                //allocate the Masked image in ARGB format
                var maskedImage = Create32bppImageAndClearAlpha(loadedImage);

                BitmapData bmpData1 = maskedImage.LockBits(new Rectangle(0, 0, maskedImage.Width, maskedImage.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, maskedImage.PixelFormat);
                byte[] maskedImageRGBAData = new byte[bmpData1.Stride * bmpData1.Height];
                System.Runtime.InteropServices.Marshal.Copy(bmpData1.Scan0, maskedImageRGBAData, 0, maskedImageRGBAData.Length);

                BitmapData bmpData2 = maskImage.LockBits(new Rectangle(0, 0, maskImage.Width, maskImage.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, maskImage.PixelFormat);
                byte[] maskImageRGBAData = new byte[bmpData2.Stride * bmpData2.Height];
                System.Runtime.InteropServices.Marshal.Copy(bmpData2.Scan0, maskImageRGBAData, 0, maskImageRGBAData.Length);

                //copy the mask to the Alpha layer
                for (int i = 0; i + 2 < maskedImageRGBAData.Length; i += 4)
                {
                    maskedImageRGBAData[i + 3] = maskImageRGBAData[i];

                }
                System.Runtime.InteropServices.Marshal.Copy(maskedImageRGBAData, 0, bmpData1.Scan0, maskedImageRGBAData.Length);
                maskedImage.UnlockBits(bmpData1);
                maskImage.UnlockBits(bmpData2);
                return maskedImage;
            }
        }

        //converts a 24bpp bitmap to another that supports alpha layer whith 32bpp 
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
    }
}
