// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// generates solidworks ready icons
    /// </summary>
    public static class AddinIcons
    {
        #region fields
        static DirectoryInfo _iconsDirInfo;
        //static Logger log = Logger.GetInstance(Properties.Resources.LogSource);
        static string _iconFullFileName;
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
                    //delete it recursively
                    dirInfo.Delete(true);

                    //log.Warning($"addin folder {dirName} was created on {dirInfo.CreationTime} so it was deleted to be re-created again");
                }

                //this method does nothing if it already exists
                dirInfo.Create();
                return dirInfo;
            }
            catch (Exception e)
            {
                //log.Error(e);
                throw e;
            }
        }

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
                result = Image.FromStream(s) as Bitmap;
            }
            return result;
        }

        static Bitmap GetAddinIcon(Type type)
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
                return _iconsDirInfo;
            }
        }

        /// <summary>
        /// Extracts icon from the assembly of a type and saves to the folder of the assembly
        /// </summary>
        /// <remarks>this is NOT the preferred method due to a SOLIDWORKS api bug.If used the installer should register absolute path of the 16x16 pixel image into 'HKLM:\Software\Solidworks\Addins\YOUR ADDIN GUID\Icon Path'.</remarks>
        static public void TrySaveAddinIconsInAssemblyFolder(Type type)
        {
            try
            {
                //var attr = type.TryGetAttribute<AddinAttribute>();
                var assy = Assembly.GetAssembly(type);

                //extract file name from the dll file
                var assyFullName = assy.GetModules()[0].FullyQualifiedName;
                var assyPath = assyFullName.TrimEnd('.', 'd', 'l', 'l');
                //var fileName = Path.GetFileNameWithoutExtension(assyFullName);

                //valid addin icon sizes as requested by SolidWORKS docs
                var sizes = new int[7] { 20, 16, 32, 40, 64, 96, 128 };

                //create valid file names
                string[] iconFileNames = new string[7];
                var sb = new StringBuilder();
                for (int i = 0; i < 7; i++)
                {
                    //extract the icon from the addin attribute
                    //if null create a blank icon, which will be displayed black in the add-in list
                    Bitmap icon = GetAddinIcon(type) ?? new Bitmap(16, 16);
                    var fullFileName = sb.Append(assyPath).Append('_').Append(sizes[i]).ToString();

                    //this has a using statement so a new icon should be passed in each call
                    MaskedBitmap.SaveAsPng(icon, new Size(sizes[i], sizes[i]), ref fullFileName);

                    sb.Clear();
                }

                //SaveIcons(icon, sizes, iconFileNames);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Extracts icon from the assembly of a type and saves to %LOCALAPPDATA%\<see cref="AddinAttribute.Title"/>.png
        /// </summary>
        /// <param name="type">A type that has <see cref="AddinAttribute"/></param>
        /// <param name="iconFullFileName"></param>
        static internal void SaveAddinIconInLocalAppData(Type type, out string iconFullFileName)
        {
            //if this has been generated already 
            if (!string.IsNullOrEmpty(_iconFullFileName))
            {
                iconFullFileName = _iconFullFileName;
                return;
            }

            var attr = type.TryGetAttribute<AddinAttribute>();
            _iconsDirInfo = CreateIconsDirInLocalAppFolder(attr.Title);
            iconFullFileName = Path.Combine(_iconsDirInfo.FullName, attr.Title + ".png");
            _iconFullFileName = iconFullFileName;
            var icon = GetAddinIcon(type);

            //remove old one if existed
            if (File.Exists(iconFullFileName))
                File.Delete(iconFullFileName);

            //if could not get the icon from the addin attribute
            if (icon == null)

            {
                //log.Warning("Could not find the icon so we created a blank one");
                //create assy new empty one
                icon = new Bitmap(16, 16);
            }

            //resize, save and dispose
            MaskedBitmap.SaveAsPng(icon, new Size(16, 16), ref iconFullFileName);
            //SaveIcons(icon, new int[] { 16 }, new[] { iconFullFileName });
        }
        #endregion
    }
}
