// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using static Hymma.Solidworks.Addins.Logger;
namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// generates solidworks ready icons
    /// </summary>
    internal static class AddinIcons
    {
        #region fields
        static DirectoryInfo _iconsDirInfo;
        static private string _iconFullFileName;
        static string logPath;
        #endregion

        #region private methods
        static DirectoryInfo CreateIconsDirInLocalAppFolder(string dirName)
        {
            //directory should be assy folder where user has access to at all times
            //because we make icons for commands every time solidworks starts
            string localApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            try
            {
                //get icons directory
                var dirPath = Path.Combine(localApp, dirName);

                var dirInfo = new DirectoryInfo(dirPath);

                //if directory exists and was created more than one day ago
                //we want to make sure that these images exist or solidarity will not load the addin
                //we set the time frame per day because it slows downs the startup of solidworks quite significantly
                if (dirInfo.Exists
                    &&
                    dirInfo.CreationTime < (DateTime.Now - TimeSpan.FromDays(1)))
                {
                    log($"dirInfo.CreationTime is {dirInfo.CreationTime} which is less than criterion");
                    log($"deleting {dirInfo.FullName} . . .");
                    //delete it recursively
                    dirInfo.Delete(true);

                }

                //this method does nothing if it already exists
                log($"creating {dirInfo.FullName} . . .");
                dirInfo.Create();
                return dirInfo;
            }
            catch (Exception e)
            {
                log("Error! " + e.Message);
                throw e;

                //TODO
                //Logger.Log(e);
            }
        }

        static List<string> GetAssemblyEmbeddedResourceNames(Assembly assy, out string resx)
        {
            var list = new List<string>();
            resx = "";

            //get all resource names
            var names = assy.GetManifestResourceNames();
            log("iterating manifest resource names");
            //iterate all resource names
            foreach (var name in names)
            {
                log($"_ {name}");
                //if name is assy name of assy resource in the binary resource file generate via resgen.exe
                if (name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
                {

                    //remove extension
                    resx = Path.GetFileNameWithoutExtension(name);
                    log($"_     Which ends with .resources so the resx name is {resx}");
                }
                else
                {
                    log("_      Which is an Embedded Resource");

                    //all other names are Embedded Resource
                    list.Add(name);
                }
            }
            return list;
        }

        static Bitmap GetResxBitmap(Type t, string imageName, string resxName)
        {
            log("Getting bitmap from resx");
            var a = Assembly.GetAssembly(t);
            var r = new ResourceManager(resxName, a);
            log("Getting resource set");
            ResourceSet set = r.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

            log("Iterating through resource set");
            foreach (System.Collections.DictionaryEntry entry in set)
            {
                log($"_  entry key is: {entry.Key.ToString()} and image name is {imageName}");
                if (string.Equals(entry.Key.ToString(), imageName, StringComparison.OrdinalIgnoreCase))
                {
                    log($"_     it is a match and entry.Value is Bitmap bi?  {entry.Value is Bitmap bi}");
                    return entry.Value as Bitmap;
                }
            }
            log($"{imageName} not found, returning null");
            return null;
        }

        static Bitmap GetEmbeddedBitmap(Type type, string resouceName)
        {
            log("Getting Embedded Bitmap");
            //define variable
            Bitmap result = null;

            //get assembly
            var assy = Assembly.GetAssembly(type);
            if (assy == null)
                return null;

            //get manifest stream
            //this method is the proper way to use with items whose build action is set to Embedded Resource
            var s = assy.GetManifestResourceStream(type, resouceName);

            if (s == null)
            {
                log("assembly manifest resource stream was null");
                return null;
            }

            //get bitmap from the resource
            using (s)
            {
                result = Image.FromStream(s) as Bitmap;
            }
            log($"image was extracted and cast to Bitmap, Is it null? {result == null}");
            return result;
        }

        static Bitmap GetAddinIcon(Type type)
        {
            var attr = type.TryGetAttribute<AddinAttribute>();
            if (attr == null)
            {
                log($"attribute is null, returning null");
                return null;
            }


            //get assembly
            var assy = Assembly.GetAssembly(type);
            Bitmap result;

            //get fileName of all Embedded Resources
            var embeddedResourceNames = GetAssemblyEmbeddedResourceNames(assy, out string resx);

            result = GetResxBitmap(type, attr.AddinIcon, resx);

            //in case result was null check the embedded resources
            if (result == null)
            {
                log("bitmap from resx was null, iterating embedded resource names");
                foreach (var item in embeddedResourceNames)
                {
                    log($"_  {item}");
                    if (item.EndsWith(attr.AddinIcon, StringComparison.OrdinalIgnoreCase))
                    {
                        log($"_ Which ends with {attr.AddinIcon}");
                        // Visual Studio always prefixes resource names with the project’s default namespace,
                        //plus the names of any subfolders in which the file is contained
                        var count = item.IndexOf('.') + 1;
                        log($"- index of . is {item.IndexOf('.')}");
                        
                        var resourceName = item.Remove(0, count);
                        log($"- removing the dot results in {resourceName}");
                        result = GetEmbeddedBitmap(type, resourceName);
                    }
                }
            }

            return result;
        }
        #endregion
        
        #region internal properties and methods

        /// <summary>
        /// this is the folder where the icons will get saved to
        /// </summary>
        /// <returns></returns>
        static internal DirectoryInfo IconsDir
        {
            get
            {
                log($"retrieving IconsDir => {_iconsDirInfo.FullName}");
                return _iconsDirInfo;
            }
        }

        /// <summary>
        /// Extracts icon from the assembly of a type and saves to %LOCALAPPDATA%\<see cref="AddinAttribute.Title"/>.png
        /// </summary>
        /// <param name="type">A type that has <see cref="AddinAttribute"/></param>
        /// <param name="iconFullFileName"></param>
        static internal void SaveAddinIcon(Type type, out string iconFullFileName)
        {
            var desk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            logPath = Path.Combine(desk, "AddinIcons.txt");
            //if this has been generated already 
            if (!string.IsNullOrEmpty(_iconFullFileName))
            {
                iconFullFileName = _iconFullFileName;
                log("returning early as iconFullFileName is already set");
                return;
            }

            var attr = type.TryGetAttribute<AddinAttribute>();
            log($"attribute is null {attr == null} and title is {attr.Title}");
            _iconsDirInfo = CreateIconsDirInLocalAppFolder(attr.Title);
            log($"iconsDirInfo.FullName is {_iconsDirInfo.FullName}");
            iconFullFileName = Path.Combine(_iconsDirInfo.FullName, attr.Title + ".png");
            _iconFullFileName = iconFullFileName;
            var icon = GetAddinIcon(type);

            //remove old one if existed
            if (File.Exists(iconFullFileName))
                File.Delete(iconFullFileName);

            //if could not get the icon from the addin attribute
            if (icon == null)

                //create assy new empty one
                icon = new Bitmap(16, 16);

            //resize, save and dispose of
            using (icon)
            {
                using (var resized = new Bitmap(icon, 16, 16))
                {
                    resized.Save(iconFullFileName);
                }
            }
        }
        #endregion
    }
}
