using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// generates solidworks ready icons
    /// </summary>
    internal static class Icons
    {
        /// <summary>
        /// get solidworks icon
        /// </summary>
        /// <param name="t"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        internal static Bitmap GetAddinIcon(Type t, string fileName)
        {
            //get assembly
            var a = Assembly.GetAssembly(t);
            Bitmap result;

            //get fileName of all Embedded Resources
            var embeddedResourceNames = GetAssemblyEmbeddedResourceNames(a, out string resx);

            result = GetResxBitmap(t, fileName, resx);

            //in case result was null check the embedded resources
            if (result == null)
            {
                foreach (var item in embeddedResourceNames)
                {
                    if (item.EndsWith(fileName, StringComparison.OrdinalIgnoreCase))
                    {
                        // Visual Studio always prefixes resource names with the project’s default namespace,
                        //plus the names of any subfolders in which the file is contained
                        var count = item.IndexOf('.') + 1;
                        var resourceName = item.Remove(0, count);
                        result = GetEmbeddedBitmap(t, resourceName);
                    }
                }
            }

            return result;
        }

        private static List<string> GetAssemblyEmbeddedResourceNames(Assembly assy, out string resx)
        {
            var list = new List<string>();
            resx = "";

            //get all resource names
            var names = assy.GetManifestResourceNames();

            //iterate all resource names
            foreach (var name in names)
            {
                //if name is a name of a resource in the binary resource file generate via resgen.exe
                if (name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))

                    //remove extension
                    resx = Path.GetFileNameWithoutExtension(name);
                else

                    //all other names are Embedded Resource
                    list.Add(name);
            }
            return list;
        }

        private static Bitmap GetResxBitmap(Type t, string imageName, string resxName)
        {
            var a = Assembly.GetAssembly(t);
            var r = new ResourceManager(resxName, a);
            ResourceSet set = r.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (System.Collections.DictionaryEntry entry in set)
            {
                if (string.Equals(entry.Key.ToString(), imageName, StringComparison.OrdinalIgnoreCase))
                {
                    return entry.Value as Bitmap;
                }
            }
            return null;
        }

        private static Bitmap GetEmbeddedBitmap(Type t, string resouceName)
        {
            //define variable
            Bitmap result = null;

            //get assembly
            var a = Assembly.GetAssembly(t);
            if (a == null)
                return null;

            //get manifest stream
            //this method is the proper way to use with items whose build action is set to Embedded Resource
            var s = a.GetManifestResourceStream(t, resouceName);

            if (s == null)
            {
                return null;
            }

            //get bitmap from the resource
            using (s)
            {
                result = Image.FromStream(s) as Bitmap;
            }
            return result;
        }
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
