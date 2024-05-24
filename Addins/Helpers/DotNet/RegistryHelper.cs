// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;

namespace Hymma.Solidworks.Addins.Helpers
{
    /// <summary>
    /// utility class to write to registry
    /// </summary>
    public static class RegisterHelper
    {
        /// <summary>
        /// registers <see cref="Type"/> provided to registry helper so solidworks can find it
        /// </summary>
        /// <param name="type">type of class that inherits from  <see cref="AddinMaker"/></param>
        public static void TryRegisterSolidworksAddin(Type type)
        {
            //As we are using EvenLog at this stage. we cannot log to it because a source in EventLog is not available immediately after a it is registered.
            try
            {
                //wix.4.0.5 heat harvester will read these data and generate proper registry components,
                //on development machines these registry values will be set during compile time via regasm.exe, visual studio will take care of that
                var addinAttribute = type.TryGetAttribute<AddinAttribute>(false);
                string key = "SOFTWARE\\SolidWorks\\Addins\\{" + type.GUID.ToString() + "}";
                RegistryKey addinKey = Registry.LocalMachine.CreateSubKey(key);
                addinKey.SetValue(null, 0);

                addinKey.SetValue("Description", addinAttribute.Description);
                addinKey.SetValue("Title", addinAttribute.Title);

                key = "Software\\SolidWorks\\AddInsStartup\\{" + type.GUID.ToString() + "}";
                RegistryKey addinStartUpKey = Registry.CurrentUser.CreateSubKey(key);
                addinStartUpKey.SetValue(null, Convert.ToInt32(addinAttribute.LoadAtStartup), RegistryValueKind.DWord);

                var icon = AddinIcons.GetAddinIcon(type);
                using (icon)
                {
                    var localAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    var path = Path.Combine(localAppDataFolder, addinAttribute.Title+"_Addin_Icon");
                    var addinIconFileName = AddinIcons.SaveAsStandardSize(icon, path, addinAttribute.Title);
                    addinKey.SetValue("Icon Path", addinIconFileName);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// unregisters the addin once removed or when the project is cleaned
        /// </summary>
        /// <param name="type"></param>
        public static void TryUnregisterSolidworksAddin(Type type)
        {
            try
            {
                string key = "SOFTWARE\\SolidWorks\\Addins\\{" + type.GUID.ToString() + "}";
                Registry.LocalMachine.DeleteSubKey(key);

                key = "Software\\SolidWorks\\AddInsStartup\\{" + type.GUID.ToString() + "}";
                Registry.CurrentUser.DeleteSubKey(key);
            }
            catch (Exception)
            {
            }
        }
    }
}
