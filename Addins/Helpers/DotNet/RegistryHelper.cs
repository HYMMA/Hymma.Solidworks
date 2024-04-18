// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Microsoft.Win32;
using System;

namespace Hymma.Solidworks.Addins.Helpers
{
    internal static class RegisterHelper
    {
        /// <summary>
        /// registers <see cref="Type"/> provided to RegisteryHelper so solidworks can find it
        /// </summary>
        /// <param name="type">type of class that inherits from  <see cref="AddinMaker"/></param>
        public static void TryRegisterSolidworksAddin(Type type)
        {
            //As we are using EvenLog at this stage. we cannot log to it because a source in EventLog is not available immediately after a it is registered.
            try
            {
                //wix.4.0.5 heat harvester will read these data and generate proper registry components, on development machines these registry values will be set during compile time
                var addinAttribute = type.TryGetAttribute<AddinAttribute>(false);
                string key = "SOFTWARE\\SolidWorks\\Addins\\{" + type.GUID.ToString() + "}";
                RegistryKey addinKey = Registry.LocalMachine.CreateSubKey(key);
                addinKey.SetValue(null, 0);

                addinKey.SetValue("Description", addinAttribute.Description);
                addinKey.SetValue("Title", addinAttribute.Title);

                key = "Software\\SolidWorks\\AddInsStartup\\{" + type.GUID.ToString() + "}";
                RegistryKey addinStartUpKey = Registry.CurrentUser.CreateSubKey(key);
                addinStartUpKey.SetValue(null, Convert.ToInt32(addinAttribute.LoadAtStartup), RegistryValueKind.DWord);

                //this value should be set during install. 
                //addin icons work only if set in this registry path
                //AddinIcons.SaveAddinIconInLocalAppData(type, out string fullFileName);
                //addinKey.SetValue("Icon Path", fullFileName);
            }
            catch (Exception )
            {
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
