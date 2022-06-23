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

        private static AddinIcons _instance;
        private static AddinAttribute _attribute;
        private static DirectoryInfo _iconsDirInfo;
        #endregion

        #region singleton pattern

        private AddinIcons() { }
        private static object _lock = new object();
        internal static AddinIcons GetInstance(AddinAttribute attribute)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new AddinIcons();
                    _attribute = attribute;
                    //create the icons directory
                    _instance.CreateIconsDir(attribute.Title);
                }
                return _instance;
            }
        }
        #endregion

        #region private methods

        private void CreateIconsDir(string dirName)
        {
            //directory should be assy folder where user has access to at all times
            //because we make icons for commands every time solidworks starts
            string localApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            try
            {
                var IconsDir = Path.Combine(localApp, dirName);

                //if directory exists, recreate it
                if (Directory.Exists(IconsDir))
                    Directory.Delete(IconsDir);
                _iconsDirInfo = Directory.CreateDirectory(IconsDir);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private List<string> GetAssemblyEmbeddedResourceNames(Assembly assy, out string resx)
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

        private Bitmap GetResxBitmap(Type t, string imageName, string resxName)
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

        private Bitmap GetEmbeddedBitmap(Type t, string resouceName)
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
        #endregion

        #region internal properties and methods

        /// <summary>
        /// this is assy folder where the icons will get saved to
        /// </summary>
        /// <returns></returns>
        internal DirectoryInfo IconsDir => _iconsDirInfo;

        /// <summary>
        /// get solidworks icon
        /// </summary>
        /// <param name="type">a type in the assembly where the Embedded Resources or .resx files are stored</param>
        /// <returns></returns>
        internal Bitmap GetAddinIcon(Type type)
        {
            var fileName = _attribute.Title;
            //get assembly
            var assy = Assembly.GetAssembly(type);
            Bitmap result;

            //get fileName of all Embedded Resources
            var embeddedResourceNames = GetAssemblyEmbeddedResourceNames(assy, out string resx);

            result = GetResxBitmap(type, fileName, resx);

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
                        result = GetEmbeddedBitmap(type, resourceName);
                    }
                }
            }

            return result;
        }
        #endregion

    }
}
