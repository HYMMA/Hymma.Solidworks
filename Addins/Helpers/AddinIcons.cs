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
    internal class AddinIcons
    {
        #region fields

        static AddinIcons _instance;
        static DirectoryInfo _iconsDirInfo;
        private string _iconFullFileName;
        #endregion

        #region constructor

        AddinIcons() { }
        static readonly object _lock = new object();
        internal static AddinIcons Instance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new AddinIcons();
                }
                return _instance;
            }
        }
        #endregion

        #region private methods
        DirectoryInfo CreateIconsDirInLocalAppFolder(string dirName)
        {
            //directory should be assy folder where user has access to at all times
            //because we make icons for commands every time solidworks starts
            string localApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            try
            {
                //get icons directory
                var dirPath = Path.Combine(localApp, dirName);

                var dirInfo = new DirectoryInfo(dirPath);

                //if directory exists and was created more than two minutes ago
                if (dirInfo.Exists
                    &&
                    dirInfo.CreationTime < (DateTime.Now - TimeSpan.FromMinutes(2)))
                {
                    //delete it recursively
                    dirInfo.Delete(true);
                }

                //this method does nothing if it already exists
                dirInfo.Create();
                return dirInfo;
            }
            catch (Exception e)
            {
                throw e;

                //TODO
                //Logger.Log(e);
            }
        }

        List<string> GetAssemblyEmbeddedResourceNames(Assembly assy, out string resx)
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

                    //remove extension
                    resx = Path.GetFileNameWithoutExtension(name);
                else

                    //all other names are Embedded Resource
                    list.Add(name);
            }
            return list;
        }

        Bitmap GetResxBitmap(Type t, string imageName, string resxName)
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

        Bitmap GetEmbeddedBitmap(Type type, string resouceName)
        {
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
                return null;
            }

            //get bitmap from the resource
            using (s)
            {
                result = Image.FromStream(s) as Bitmap;
            }
            return result;
        }

        Bitmap GetAddinIcon(Type type)
        {
            var attr = type.TryGetAttribute<AddinAttribute>();
            if (attr == null)
                return null;


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
        internal DirectoryInfo IconsDir => _iconsDirInfo;

        
        /// <summary>
        /// Extracts icon from the assembly of a type and saves to %LOCALAPPDATA%\<see cref="AddinAttribute.Title"/>.png
        /// </summary>
        /// <param name="type">A type that has <see cref="AddinAttribute"/></param>
        /// <param name="iconFullFileName"></param>
        internal void SaveAddinIcon(Type type, out string iconFullFileName)
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
