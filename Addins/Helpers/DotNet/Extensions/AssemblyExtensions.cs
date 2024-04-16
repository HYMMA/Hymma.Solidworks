﻿// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.IO;
using System.Reflection;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// Provides extensions for <see cref="Assembly"/> object
    /// </summary>
    internal static class AssemblyExtensions
    {
        /// <summary>
        /// get the Path to the current Assembly
        /// </summary>
        public static string GetAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
   
    }
}
