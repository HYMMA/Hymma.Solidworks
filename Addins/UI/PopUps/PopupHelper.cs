// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins.Utilities.DotNet;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Hymma.Solidworks.Addins.UI.PopUps
{
    /// <summary>
    /// Utilities for "IXPopupWindow{TWindow}>"/>
    /// </summary>
    public static class PopupHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        /// <summary>
        /// Returns top-left location of the popup
        /// </summary>
        /// <param name="parentWnd">Parent window</param>
        /// <param name="zone"></param>
        /// <param name="scaleDpi">True to scale according to the screen resolution</param>
        /// <param name="width">Width of the popup</param>
        /// <param name="height">Height of the popup</param>
        /// <param name="padding">Padding of the popup</param>
        /// <returns>Top-left location</returns>
        public static Point CalculateLocation(IntPtr parentWnd, ScreenZones zone, bool scaleDpi, double width, double height, Thickness padding)
        {
            GetWindowRect(parentWnd, out var rect);

            double scaleX;
            double scaleY;

            if (scaleDpi)
            {
                scaleX = GraphicsHelper.XDpiScale;
                scaleY = GraphicsHelper.YDpiScale;
            }
            else
            {
                scaleX = 1;
                scaleY = 1;
            }

            var left = rect.Left / scaleX;
            var top = rect.Top / scaleY;

            var wndWidth = (rect.Right - rect.Left) / scaleX;
            var wndHeight = (rect.Bottom - rect.Top) / scaleY;

            if (double.IsNaN(height))
            {
                height = 0;
            }
            if (double.IsNaN(width))
            {
                width = 0;
            }

            switch (zone)
            {
                case ScreenZones.Center:
                    return new Point((int)(left + wndWidth - width) / 2, (int)(top + wndHeight - height) / 2);

                case ScreenZones.TopRight:
                    return new Point((int)(left + wndWidth - width - padding.Right), (int)(top + padding.Top));

                case ScreenZones.TopLeft:
                    return new Point((int)(left + padding.Left), (int)(top + padding.Top));

                case ScreenZones.BottomRight:
                    return new Point((int)(left + wndWidth - width - padding.Right), (int)(top + wndHeight - height - padding.Bottom));

                case ScreenZones.BottomLeft:
                    return new Point((int)(left + padding.Left), (int)(top + wndHeight - height - padding.Bottom));

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
