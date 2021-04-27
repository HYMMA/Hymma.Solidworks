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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Hymma.SolidTools.SolidAddins.Tools
{
    public static class ToolbarIcons
    {
        /// <summary>
        /// generate 6-off SolidWorks toolbar sprites in 20, 32, 40, 64, 96, 128 pixels
        /// </summary>
        /// <param name="icons">full file name of the icon file</param>
        /// <param name="filenamePrepend">Prepends this word to the new file, it's a suffix</param>
        /// <returns>address to the strip file</returns>
        public static IEnumerable<string> GetIcons(string[] icons, string filenamePrepend)
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
            var possibleSizes = new List<int>(new[] { 20, 32, 40, 64, 96, 128 });

            // define an image list
            var images = new List<string>();

            //check iconfiles
            foreach (var item in icons)
            {
                string icon = item;
                // Make sure the file exists
                if (!File.Exists(icon))
                {
                    // Try and find it with .png to the name
                    if (!icon.Contains('.'))
                        icon += ".png";

                    //check file extension
                    var fi = new FileInfo(icon);
                    if (!fi.Extension.Equals("png", StringComparison.OrdinalIgnoreCase))
                        throw new FormatException(icon);

                    // Check if it exists again
                    if (!File.Exists(icon))
                    {
                        // Let user know file not found
                        throw new FileNotFoundException(icon);
                    }
                    else
                        // Add this to the list and carry on
                        images.Add(icon);
                }
                else
                {
                    // Add this to the list and carry on
                    images.Add(icon);
                }
            }

            // If we have no images then throw exception
            if (images.Count < 1)
            {
                throw new ArgumentOutOfRangeException(images.ToString());
            }
            //variable to hold address to the strips files
            var stripes = new List<string>();

            // Now create an image from each of the images, for each file size
            possibleSizes.ForEach(size =>
            {
                var assyDir = GetAssemblyDirectory();
                // Combine all bitmaps
                using (var combinedImage = CombineBitmap(images, size))
                {
                    var stripe = $"{filenamePrepend}{size}.png";
                    combinedImage.Save(stripe);
                    stripes.Add(Path.Combine(assyDir, stripe));
                }
            });
            return stripes;
        }

        /// <summary>
        /// get the path to the current exe
        /// </summary>
        private static string GetAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// Combines images into a sprite horizontally
        /// </summary>
        /// <param name="files">The files to combine</param>
        /// <param name="iconSize">The sprite size</param>
        /// <returns></returns>
        private static Bitmap CombineBitmap(List<string> files, int iconSize)
        {
            // Read all images into memory
            Bitmap finalImage = null;
            var images = new List<Bitmap>();

            try
            {
                // Get size
                int width = iconSize * files.Count;
                int height = iconSize;

                // Create a bitmap to hold the combined image
                finalImage = new Bitmap(width, height);

                // Get a graphics object from the image so we can draw on it
                using (var g = Graphics.FromImage(finalImage))
                {
                    // Set background color
                    g.Clear(Color.Transparent);

                    // Go through each image and draw it on the final image
                    int offset = 0;
                    files.ForEach(file =>
                    {
                        // Read this image
                        var bitmap = new Bitmap(file);
                        images.Add(bitmap);

                        // Scale it to the sprite size
                        var scaleFactor = (float)iconSize / Math.Max(bitmap.Width, bitmap.Height);

                        // Draw it onto the new image
                        g.DrawImage(bitmap, new Rectangle(offset, 0, (int)(scaleFactor * bitmap.Width), (int)(scaleFactor * bitmap.Height)));

                        // Move offset to next position
                        offset += iconSize;

                    });
                }

                // Return the final image
                return finalImage;
            }
            catch (Exception)
            {
                // Cleanup
                finalImage?.Dispose();
                throw;
            }
            finally
            {
                // Cleanup
                images.ForEach(image => image?.Dispose());
            }
        }
    }
}
