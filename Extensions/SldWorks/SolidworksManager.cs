// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using System;
using System.Runtime.InteropServices;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// Handles <see cref="ISldWorks"/>
    /// </summary>
    public static class SolidworksManager
    {
        private const string ProgId = "SldWorks.Application";

        /// <summary>
        /// Connects to a running SolidWorks instance, or creates a new one if none is running.
        /// </summary>
        /// <returns>An instance of <see cref="ISldWorks"/></returns>
        /// <exception cref="InvalidOperationException">Thrown when unable to connect to or start SolidWorks.</exception>
        public static ISldWorks GetOrStartSolidworksApp()
        {
            try
            {
                return (ISldWorks)Marshal.GetActiveObject(ProgId);
            }
            catch (COMException)
            {
                return CreateNewInstance();
            }
        }

        /// <summary>
        /// Connects to an already running SolidWorks instance.
        /// </summary>
        /// <returns>An instance of <see cref="ISldWorks"/>, or null if no instance is running.</returns>
        public static ISldWorks GetActiveSolidworksApp()
        {
            try
            {
                return (ISldWorks)Marshal.GetActiveObject(ProgId);
            }
            catch (COMException)
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a new SolidWorks instance via COM activation.
        /// </summary>
        /// <returns>A new instance of <see cref="ISldWorks"/></returns>
        /// <exception cref="InvalidOperationException">Thrown when unable to create a SolidWorks instance.</exception>
        public static ISldWorks StartNewSolidworksApp()
        {
            return CreateNewInstance();
        }

        private static ISldWorks CreateNewInstance()
        {
            var swType = Type.GetTypeFromProgID(ProgId);
            if (swType == null)
                throw new InvalidOperationException("SolidWorks is not installed or not registered on this machine.");

            var instance = Activator.CreateInstance(swType) as ISldWorks;
            if (instance == null)
                throw new InvalidOperationException("Failed to create a new SolidWorks instance.");

            return instance;
        }
    }
}
