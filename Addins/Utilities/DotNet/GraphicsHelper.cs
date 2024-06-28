// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;

namespace Hymma.Solidworks.Addins.Utilities.DotNet
{
    internal static class GraphicsHelper
    {
        //A static class with static status is a bad idea specially because solidworks loads everything in one app domain. that means all addins will get this exact same result that this class returns. It's only okay here because DPI scale cannot be changed unless user signs-out form computer. In other words it does not change during the app usages.
        internal static double XDpiScale { get; set; } 
        internal static double YDpiScale { get; set; } 
        /// <summary>
        /// gets the scale of dpi 
        /// </summary>
        ///<remarks>when user changes the scale factor in windows it affects the DPI value. the default is 96 at 100%. this scale should be denominator</remarks>
        internal static void SaveDpiScaleInMemory()
        {
            using (var graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
            {
                const int DPI = 96;

                XDpiScale = graphics.DpiX / DPI;
                YDpiScale = graphics.DpiY / DPI;
            }
        }
    }
}
