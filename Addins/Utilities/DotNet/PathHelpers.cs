// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.Solidworks.Addins.Utilities.DotNet
{
    /// <summary>
    /// helpers methods for <see cref="Path"/>
    /// </summary>
    public static class PathHelpers
    {
        /// <summary>
        /// removes invalid file name characters from file name
        /// </summary>
        /// <param name="input"></param>
        /// <returns>string without invalid file name chars</returns>
        public static string RemoveInvalidFileNameChars(string input)
        {
            var sb = new StringBuilder(input);
            var invalidFilaNameChars = Path.GetInvalidFileNameChars();
            for (int i = 0; i < input.Length; i++)
            {
                if (invalidFilaNameChars.Contains(input[i]))
                    sb.Remove(i, 1);
            }
            return sb.ToString();
        }
    }
}
