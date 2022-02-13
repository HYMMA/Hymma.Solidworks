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

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// genrates solidworks ready icons
    /// </summary>
    public static class IconGenerator
    {
        ///// <summary>
        ///// generate 6-off SolidWorks toolbar sprites in 20, 32, 40, 64, 96, 128 pixels
        ///// </summary>
        ///// <param name="icons">bitmap files to combine together</param>
        ///// <param name="filenamePrepend">Prepends this word to the new file, it's a suffix</param>
        ///// <returns>address to the strip file</returns>
        //public static string[] GetCommandGroupIconStrips(Bitmap[]
        //    icons, string filenamePrepend)
        //{
        //    // All output sizes
        //    var possibleSizes = new[] { 20, 32, 40, 64, 96, 128 };

        //    // define an image list
        //    Bitmap[] images = icons;

        //    // If we have no images then throw exception
        //    if (images.Length < 1)
        //    {

        //        Log("Error! list of images were less than 1");
        //        throw new ArgumentOutOfRangeException(images.ToString());
        //    }
        //    //variable to hold address to the strips files
        //    var stripes = new string[possibleSizes.Length];

        //    // Now create an image from each of the images, for each file size
        //    for (int i = 0; i < possibleSizes.Length; i++)
        //    {
        //        var size = possibleSizes[i];

        //        // Combine all bitmaps
        //        Log("combining all bitmaps");
        //        var combinedImage = CombineBitmap(images, size);

        //        Log("attempting to save combinedImage");
        //        using (combinedImage)
        //        {
        //            try
        //            {

        //                var stripe = $"{filenamePrepend}{size}.png";
        //                Log($"stripe file name is {stripe}");

        //                stripes[i] = (Path.Combine(GetIconDir(), stripe));
        //                Log($"stripe path is {stripes[i]}");

        //                combinedImage.Save(stripes[i]);
        //                Log($"saved {stripe} which has index of {i}");
        //            }
        //            catch (Exception e)
        //            {
        //                Log($"Error! {e.Message}");
        //                throw;
        //            }
        //        }
        //    };

        //    Log($"returning all stripes of qty {stripes.Length}");
        //    return stripes;
        //}

   

        ///// <summary>
        ///// Combines images into a sprite horizontally
        ///// </summary>
        ///// <param name="bitmaps">The bitmaps to combine</param>
        ///// <param name="iconSize">The sprite size</param>
        ///// <returns></returns>
        //private static Bitmap CombineBitmap(Bitmap[] bitmaps, int iconSize)
        //{
        //    // Read all images into memory
        //    Bitmap finalImage = null;
        //    Bitmap[] images = new Bitmap[bitmaps.Length];
        //    try
        //    {

        //        Log("getting the bitmap sizes");
        //        // Get size
        //        int width = iconSize * bitmaps.Length;
        //        int height = iconSize;

        //        // Create a bitmap to hold the combined image
        //        finalImage = new Bitmap(width, height);

        //        // Get a graphics object from the image so we can draw on it
        //        using (var g = Graphics.FromImage(finalImage))
        //        {

        //            Log("setting bitmap background color to transparent");
        //            // Set background color
        //            g.Clear(Color.Transparent);

        //            // Go through each image and draw it on the final image
        //            Log("going through each image and drawing it on the final image");
        //            int offset = 0;
        //            for (int i = 0; i < bitmaps.Length; i++)
        //            {
        //                var file = bitmaps[i];
        //                Log($"file is {file}");
        //                // Read this image
        //                var bitmap = new Bitmap(file);
        //                images[i] = bitmap;

        //                // Scale it to the sprite size
        //                var scaleFactor = (float)iconSize / Math.Max(bitmap.Width, bitmap.Height);


        //                Log("attempting to drawing it on canvas");
        //                // Draw it onto the new image
        //                g.DrawImage(bitmap, new Rectangle(offset, 0, (int)(scaleFactor * bitmap.Width), (int)(scaleFactor * bitmap.Height)));

        //                // Move offset to next position
        //                offset += iconSize;

        //            };
        //        }


        //        Log($"returning final image {finalImage}");
        //        // Return the final image
        //        return finalImage;
        //    }
        //    catch (Exception)
        //    {
        //        Log("Error! there was error in combining bitmaps");
        //        // Cleanup
        //        finalImage?.Dispose();
        //        throw;
        //    }
        //    finally
        //    {
        //        // Cleanup
        //        for (int i = 0; i < images.Length; i++)
        //        {
        //            if (images[i] != null)
        //                images[i].Dispose();
        //        }
        //    }
        //}

        ///// <summary>
        ///// this is a folder where the icons will get saved to
        ///// </summary>
        ///// <returns></returns>
        //public static string GetIconDir()
        //{
        //    //directory should be a folder where user has access to at all times
        //    //because we make icons for commands everytime solidworks starts
        //    string localApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        //    try
        //    {
        //        return Directory.CreateDirectory(Path.Combine(localApp, AddinTitle)).FullName;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
    }
}
