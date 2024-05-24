// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// generates SolidWORKS ready icons
    /// </summary>
    public static class AddinIcons
    {
        static List<string> GetAssemblyEmbeddedResourceNames(Assembly assy, out string resx)
        {
            var list = new List<string>();
            resx = "";

            //get all resource names
            var names = assy.GetManifestResourceNames();
            //iterate all resource names
            foreach (var name in names)
            {
                //if name is assy name of assy resource in the binary resource file generate via resgen.exe
                if (name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
                {
                    //remove extension
                    resx = Path.GetFileNameWithoutExtension(name);
                }
                else
                {

                    //all other names are Embedded Resource
                    list.Add(name);
                }
            }
            return list;
        }

        static Bitmap GetResxBitmap(Type t, string imageName, string resxName)
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
            //log.Warning($"image {imageName} did not exist in {resxName}");
            return null;
        }

        static Bitmap GetEmbeddedBitmap(Type type, string resourceName)
        {
            //define variable
            Bitmap result = null;
            //get assembly
            var assy = Assembly.GetAssembly(type);
            if (assy == null)
            {
                //log.Error(new Exception("Could not get the Assembly of the type specified"));
                return null;
            }

            //get manifest stream
            //this method is the proper way to use with items whose build action is set to Embedded Resource
            var s = assy.GetManifestResourceStream(type, resourceName);

            if (s == null)
            {
                //log.Error(new Exception($"Could not get the manifest resource stream in {resourceName}"));
                return null;
            }

            //get bitmap from the resource
            using (s)
            {
                result = System.Drawing.Image.FromStream(s) as Bitmap;
            }
            return result;
        }

        /// <summary>
        /// extracts image by its name as identified in <see cref="AddinAttribute.AddinIcon"/> 
        /// </summary>
        /// <param name="type">the type the has the attribute, the main addin class</param>
        /// <returns>a bitmap object</returns>
        ///<remarks>this method is public for testing only</remarks>
        public static Bitmap GetAddinIcon(Type type)
        {
            var attr = type.TryGetAttribute<AddinAttribute>();
            //get assembly
            var assy = Assembly.GetAssembly(type);
            Bitmap result;

            //get fileName of all Embedded Resources
            var embeddedResourceNames = GetAssemblyEmbeddedResourceNames(assy, out string resx);

            result = GetResxBitmap(type, attr.AddinIcon, resx);

            //in case result was null check the embedded resources
            if (result == null)
            {
                foreach (var item in embeddedResourceNames)
                {
                    if (item.EndsWith(attr.AddinIcon, StringComparison.OrdinalIgnoreCase))
                    {
                        // Visual Studio always prefixes resource names with the project’s default namespace,
                        //plus the names of any subfolders in which the file is contained
                        var count = item.IndexOf('.') + 1;

                        var resourceName = item.Remove(0, count);
                        result = GetEmbeddedBitmap(type, resourceName);
                        //log.Info($"found the icon in embedded resources");
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Solidworks Addin icons has to be in 16x16 anything else fails. this method converts a random image file into a size recognizable by solidworks
        /// </summary>
        /// <param name="image">the image obj to convert to png format in 16x16</param>
        /// <param name="directory">the directory to save the file</param>
        /// <param name="file">the name of the file. the extension does not matter.</param>
        ///<returns>full file name of the modified image</returns>
        public static string SaveAsStandardSize(Bitmap image, string directory, string file)
        {
            if (image == null || string.IsNullOrEmpty(directory))
                throw new ArgumentNullException();

            if (file.ToCharArray().Any(c => (Path.GetInvalidPathChars().Any(i => i.Equals(c)))))
                throw new Exception("file contains invalid chars");

            Directory.CreateDirectory(directory);
            //var fileName = Path.GetFileNameWithoutExtension(file);
            var fileNamePng = Path.ChangeExtension(file, "png");
            var fullFileName = Path.Combine(directory, fileNamePng);
            using (image)
            {
                //MaskedBitmap.SaveAsPng(image, new Size(16, 16),ref fullFileName, false, 255, false);
                using (var newSize = new Bitmap(image, new Size(16, 16)))
                {
                    newSize.Save(fullFileName, ImageFormat.Png);
                }
            }

            return fullFileName;
        }
    }
}
