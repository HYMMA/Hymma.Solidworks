// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Microsoft.Win32;
using System;

namespace Hymma.Solidworks.Addins.Helpers
{
    internal static class RegisteryHelper
    {
        /// <summary>
        /// registers <see cref="Type"/> provided to RegisteryHelper so solidworks can find it
        /// </summary>
        /// <param name="type">type of class that inherits from  <see cref="AddinMaker"/></param>
        public static void RegisterSolidworksAddin(Type type)
        {
            Logger.log("registering solidworks addin");
            try
            {
                var addinAttribute = type.TryGetAttribute<AddinAttribute>(false);
                string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + type.GUID.ToString() + "}";
                RegistryKey addinKey = Registry.LocalMachine.CreateSubKey(keyname);
                addinKey.SetValue(null, 0);

                addinKey.SetValue("Description", addinAttribute.Description);
                addinKey.SetValue("Title", addinAttribute.Title);

                keyname = "Software\\SolidWorks\\AddInsStartup\\{" + type.GUID.ToString() + "}";
                RegistryKey addinStartUpKey = Registry.CurrentUser.CreateSubKey(keyname);
                addinStartUpKey.SetValue(null, Convert.ToInt32(addinAttribute.LoadAtStartup), RegistryValueKind.DWord);

                AddinIcons.SaveAddinIcon(type, out string iconPath);
                addinKey.SetValue("Icon Path", iconPath);
            }
            catch (Exception e)
            {
                Logger.log("Error!  \r\n" +e.Message);
            }
        }

        /// <summary>
        /// unregisters the addin once removed or when the project is cleaned
        /// </summary>
        /// <param name="type"></param>
        public static void UnregisterSolidworksAddin(Type type)
        {
            try
            {
                string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + type.GUID.ToString() + "}";
                Registry.LocalMachine.DeleteSubKey(keyname);

                keyname = "Software\\SolidWorks\\AddInsStartup\\{" + type.GUID.ToString() + "}";
                Registry.CurrentUser.DeleteSubKey(keyname);
            }
            catch (Exception)
            {
            }
        }
    }
}
