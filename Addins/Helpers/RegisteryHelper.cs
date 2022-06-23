using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.Solidworks.Addins.Helpers
{
    internal static class RegisteryHelper
    {

        internal static void Register(Type t)
        {
            var addinAttribute = t.TryGetAttribute<AddinAttribute>(false) as AddinAttribute;

            if (addinAttribute == null)
                return;

            try
            {
                Logger.Source = "Solidworks Addin";
                Log("trying to register");

                string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + t.GUID.ToString() + "}";
                RegistryKey addinKey = Registry.LocalMachine.CreateSubKey(keyname);
                addinKey.SetValue(null, 0);

                addinKey.SetValue("Description", addinAttribute.Description);
                addinKey.SetValue("Title", addinAttribute.Title);

                keyname = "Software\\SolidWorks\\AddInsStartup\\{" + t.GUID.ToString() + "}";
                RegistryKey addinStartUpKey = Registry.CurrentUser.CreateSubKey(keyname);
                addinStartUpKey.SetValue(null, Convert.ToInt32(addinAttribute.LoadAtStartup), RegistryValueKind.DWord);

                #region Extract icon during registration
                _addinTitle = addinAttribute.Title;
                addinKey.SetValue("Icon Path", GetIconPath(t, addinAttribute));

                Log("Registration was successful!");
                #endregion
            }
            catch (System.NullReferenceException e)
            {
                Log($"Error! There was a problem registering this library: addinModel is null. \n\"" + e.Message + "\"");
            }
            catch (System.Exception e)
            {
                Log(e);
            }
        }

        internal static void Unregister(Type type)
        {
            Logger.Source = "Solidworks Addin";
            var swAttr = t.TryGetAttribute<AddinAttribute>(false) as AddinAttribute;
            if (swAttr == null)
                try
                {
                    Log("trying to unregister");
                    string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + t.GUID.ToString() + "}";
                    Registry.LocalMachine.DeleteSubKey(keyname);

                    keyname = "Software\\SolidWorks\\AddInsStartup\\{" + t.GUID.ToString() + "}";
                    Registry.CurrentUser.DeleteSubKey(keyname);

                    Log("Unregister successful");
                    //UnRegisterLogger(swAttr.Title);

                }
                catch (System.NullReferenceException nl)
                {
                    Log(nl.Message);

                    //TODO:log this
                    Console.WriteLine("Error! There was a problem unregistering this library: " + nl.Message);
                }
                catch (System.Exception e)
                {
                    //TODO:log this
                    Log(e.Message);
                    Console.WriteLine("Error! There was a problem unregistering this library: " + e.Message);
                }
        }

        /// <summary>
        /// generates an addin icon (.png) format and saves it on assembly folder
        /// </summary>
        /// <returns>returns address</returns>
        private static string GetIconPath(Type type, AddinAttribute addinAttribute)
        {
            //get addin icon
            var icon = AddinIcons.GetInstance(addinAttribute).GetAddinIcon(type);

            //if could not get the icon from the addin attribute
            if (icon == null)

                //create a new empty one
                icon = new Bitmap(16, 16);
            
            var dirInfo=AddinIcons.CreateIconsDir(addinAttribute.Title);
            string addinIconAddress = Path.Combine(dirInfo.FullName, type.Name + ".png");

            //resize, save and dispose of
            using (icon)
            {
                using (var resized = new Bitmap(icon, 16, 16))
                {
                    resized.Save(addinIconAddress);
                }
            }
            return addinIconAddress;
        }
    }
}
