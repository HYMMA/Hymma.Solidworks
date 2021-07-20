#region lincese
//this is forked from AngleSix.SolidWorksApi.IconGenerator
//https://github.com/angelsix/solidworks-api/tree/develop/Tools/CommandManager%20Icon%20Generator

//MIT License

//Copyright (c) 2017

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//All files inside the References folder are property of Dassault Systemes 
//SolidWorks Corp and may only be used in unmodified form in conjunction with 
//SolidDNA.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
#endregion

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static Hymma.SolidTools.Addins.Logger;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// genrates solidworks ready icons
    /// </summary>
    public static class IconGenerator
    {
        /// <summary>
        /// generate 6-off SolidWorks toolbar sprites in 20, 32, 40, 64, 96, 128 pixels
        /// </summary>
        /// <param name="icons">bitmap files to combine together</param>
        /// <param name="filenamePrepend">Prepends this word to the new file, it's a suffix</param>
        /// <returns>address to the strip file</returns>
        public static string[] GetCommandGroupIconStrips(Bitmap[]
            icons, string filenamePrepend)
        {
            // The filename to prepend to the output files
            // 
            //   NOTE: 
            //
            //   We expect a list of images in, and a name to prepend the filename as the last argument
            //
            //   From that we will combine them into lists and resize them 
            //   from the top size down to the smallest size
            //

            // All output sizes
            var possibleSizes = new[] { 20, 32, 40, 64, 96, 128 };

            // define an image list
            Bitmap[] images = icons;

            // If we have no images then throw exception
            if (images.Length < 1)
            {

                Log("Error! list of images were less than 1");
                throw new ArgumentOutOfRangeException(images.ToString());
            }
            //variable to hold address to the strips files
            var stripes = new string[possibleSizes.Length];

            // Now create an image from each of the images, for each file size
            for (int i = 0; i < possibleSizes.Length; i++)
            {
                var size = possibleSizes[i];

                // Combine all bitmaps
                Log("combining all bitmaps");
                var combinedImage = CombineBitmap(images, size);

                Log("attempting to save combinedImage");
                using (combinedImage)
                {
                    try
                    {

                        var stripe = $"{filenamePrepend}{size}.png";
                        Log($"stripe file name is {stripe}");

                        stripes[i] = (Path.Combine(GetIconFolder(), stripe));
                        Log($"stripe path is {stripes[i]}");

                        combinedImage.Save(stripes[i]);
                        Log($"saved {stripe} which has index of {i}");
                    }
                    catch (Exception e)
                    {
                        Log($"Error! {e.Message}");
                        throw;
                    }
                }
            };

            Log($"returning all stripes of qty {stripes.Length}");
            return stripes;
        }

        /// <summary>
        /// generates an addin icon (.png) format and saves it on assembly folder
        /// </summary>
        /// <param name="icon">the icon to transform</param>
        /// <param name="filename">name of file without extension</param>
        /// <returns></returns>
        public static string GetAddinIcon(Bitmap icon, string filename)
        {
            var addinIcon = Resize(icon, 16, 16);
            string addinIconAddress = Path.Combine(GetIconFolder(), filename + ".png");
            addinIcon.Save(addinIconAddress);
            return addinIconAddress;
        }

        /// <summary>
        /// edits and saves a bitmap for use in <see cref="PmpBitmapButton"/> 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="images">an array of files of resized images</param>
        /// <param name="maskedImages">an array of masked images</param>
        /// <returns>An array of arrays where first array is the resized images and the second array is their masked images </returns>
        public static void GetBitmapButtonIcons(Bitmap bitmap, out string[] images, out string[] maskedImages)
        {
            //possible sizes for a button bitmap in solidworks
            var possibleSizes = new[] { 20, 32, 40, 64, 96, 128 };

            //empty array to hold address of final bitmaps
            maskedImages = images = new string[6];
            
            //iterate through possible sizes and process bitmap against that size
            for (int i = 0; i < possibleSizes.Length; i++)
            {
                var size = possibleSizes[i];
                var resized = Resize(bitmap, size, size);
                SaveMaskedImage(resized, GetIconFolder(), Guid.NewGuid().ToString(), out images[i], out maskedImages[i]);
            }
        }

        /// <summary>
        /// coverts and saves a bitmap for use in a <see cref="PmpBitmap"/>
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="image">full file name of the image on disk</param>
        /// <param name="maskeImage">full file name of the masked image on disk</param>
        public static void GetBitmapIcon(Bitmap bitmap, out string image, out string maskeImage)
        {
            //get bitmap size
            Bitmap resized = bitmap;
            if (bitmap.Width != bitmap.Height)
            {
                var size = Math.Min(bitmap.Width, bitmap.Height);
                resized = Resize(bitmap, size, size);
            }
            SaveMaskedImage(resized, GetIconFolder(), Guid.NewGuid().ToString(), out image, out maskeImage);
        }

        /// <summary>
        /// coverts and saves a bitmap to specified location
        /// </summary>
        /// <param name="bitmap">file to get bitmask for</param>
        /// <param name="directory">directory address</param>
        /// <param name="filename">without extension</param>
        /// <param name="image"></param>
        /// <param name="mask"></param>
        /// <returns>mask image file or "" if bitmap provided is of type png</returns>
        private static void SaveMaskedImage(Bitmap bitmap, string directory, string filename, out string image, out string mask)
        {
            //png files dont support bitmask
            if (bitmap.RawFormat.Equals(ImageFormat.Png))
            {
                image = Path.Combine(directory, filename, ".png");
                //accroding to solidworks api for png files we should return empty string as masked images
                mask = "";
            }
            else
            {
                image = Path.Combine(directory, filename, ".bmp");
                mask= ImageMask.GetImageMask(bitmap, directory, filename);
            }
            bitmap.Save(image);
        }

        /// <summary>
        /// Combines images into a sprite horizontally
        /// </summary>
        /// <param name="bitmaps">The bitmaps to combine</param>
        /// <param name="iconSize">The sprite size</param>
        /// <returns></returns>
        private static Bitmap CombineBitmap(Bitmap[] bitmaps, int iconSize)
        {
            // Read all images into memory
            Bitmap finalImage = null;
            Bitmap[] images = new Bitmap[bitmaps.Length];
            try
            {

                Log("getting the bitmap sizes");
                // Get size
                int width = iconSize * bitmaps.Length;
                int height = iconSize;

                // Create a bitmap to hold the combined image
                finalImage = new Bitmap(width, height);

                // Get a graphics object from the image so we can draw on it
                using (var g = Graphics.FromImage(finalImage))
                {

                    Log("setting bitmap background color to transparent");
                    // Set background color
                    g.Clear(Color.Transparent);

                    // Go through each image and draw it on the final image
                    Log("going through each image and drawing it on the final image");
                    int offset = 0;
                    for (int i = 0; i < bitmaps.Length; i++)
                    {
                        var file = bitmaps[i];
                        Log($"file is {file}");
                        // Read this image
                        var bitmap = new Bitmap(file);
                        images[i] = bitmap;

                        // Scale it to the sprite size
                        var scaleFactor = (float)iconSize / Math.Max(bitmap.Width, bitmap.Height);


                        Log("attempting to drawing it on canvas");
                        // Draw it onto the new image
                        g.DrawImage(bitmap, new Rectangle(offset, 0, (int)(scaleFactor * bitmap.Width), (int)(scaleFactor * bitmap.Height)));

                        // Move offset to next position
                        offset += iconSize;

                    };
                }


                Log($"returning final image {finalImage}");
                // Return the final image
                return finalImage;
            }
            catch (Exception)
            {
                Log("Error! there was error in combining bitmaps");
                // Cleanup
                finalImage?.Dispose();
                throw;
            }
            finally
            {
                // Cleanup
                for (int i = 0; i < images.Length; i++)
                {
                    if (images[i] != null)
                        images[i].Dispose();
                }
            }
        }

        /// <summary>
        /// this is a folder where the icons will get saved to
        /// </summary>
        /// <returns></returns>
        private static string GetIconFolder()
        {
            //directory should be a folder where user has access to at all times
            //because we make icons for commands everytime solidworks starts
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            directory = Path.Combine(directory, "HYMMA.SolidTools.Addins");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            return directory;
        }

        /// <summary>
        /// resizes a bitmap and returns it with the new size
        /// </summary>
        /// <param name="bitmap">the bitmap to resize</param>
        /// <param name="width">new width in pixles</param>
        /// <param name="height">new height in pixles</param>
        /// <returns>new <see cref="Bitmap"/> with sizes specified</returns>
        private static Bitmap Resize(Bitmap bitmap, int width, int height)
        {
            var newIcon = new Bitmap(width, height);
            try
            {
                using (var g = Graphics.FromImage(newIcon))
                {
                    g.Clear(Color.Transparent);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(bitmap, new Rectangle(0, 0, width, height));
                }
            }
            catch (Exception)
            {
                Log("Couldnt resize bitmap");
                throw;
            }
            return newIcon;
        }
    }
}
