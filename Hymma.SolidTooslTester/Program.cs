using Hymma.SolidTools.Addins;
using System;
using System.Drawing;
using System.Reflection;
using System.Resources;
namespace Hymma.SolidTooslTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var bitmap = GetBitmap(typeof(Hymma.Addin.SolidWorks.Hymma));
        }
        public static Bitmap GetBitmap(Type t)
        {
            var attribute = t.GetCustomAttribute(typeof(AddinAttribute)) as AddinAttribute;
            if (attribute == null) return null;
            var asm = t.Assembly;
            string[] resNames = asm.GetManifestResourceNames();
            foreach (var resName in resNames)
            {
                var rm = new ResourceManager(resName, asm);

                // Get the fully qualified resource type name
                // Resources are suffixed with .resource
                var resName2 = resName.Substring(0, resName.IndexOf(".resource"));
                var type = asm.GetType(resName2, true);

                var resources = type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (PropertyInfo res in resources)
                {
                    // collect string type resources
                    if (res.PropertyType == typeof(Bitmap) && res.Name == attribute.AddinIcon)
                        return res.GetValue(null, null) as Bitmap;
                }

            }
            return null;
        }

    }
}
