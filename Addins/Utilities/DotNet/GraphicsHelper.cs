// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.Runtime.InteropServices;

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

        /// <summary>
        /// get current dpi of the user pc. firt item is the XDpi and the second is the YDpi
        /// </summary>
        /// <returns></returns>
        public static float[] GetUserDpi()
        {
            IntPtr hdc = GdiApi.CreateDC("DISPLAY", null, null, IntPtr.Zero);
            var x = GdiApi.GetDeviceCaps(hdc, 88);
            var VerticalResolution = GdiApi.GetDeviceCaps(hdc, 90);
            GdiApi.DeleteDC(hdc);
            return new float[2] { x, VerticalResolution };
        }

        static class GdiApi
        {

            [DllImport("gdi32.dll")]
            internal static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

            [DllImport("gdi32.dll")]
            internal static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("gdi32.dll")]
        internal static extern bool DeleteDC(IntPtr hdc);
        }
    }
}
