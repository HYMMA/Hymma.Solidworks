// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using System;
using System.Windows;
using System.Windows.Forms;
namespace Hymma.Solidworks.Addins.UI.PopUps
{
    /// <summary>
    /// Ui Related Extensions to <see cref="ISldWorks"/>
    /// </summary>
    public static class ISldWorksExtensions
    {
        /// <summary>
        /// registers a win-form into this solidworks session
        /// </summary>
        /// <param name="sldWorks"></param>
        /// <param name="form">the win-form object to register into solidworks</param>
        /// <returns>A wrapper object that allows you to show or show-as-dialogue the win-form</returns>
        public static PopupWinForm HookWinForm(this ISldWorks sldWorks, Form form)
        {
            var parent = (IntPtr)sldWorks.IFrameObject().GetHWndx64();
            return new PopupWinForm(form, parent);
        }

        /// <summary>
        /// registers a wpf window into this session of solidworks
        /// </summary>
        /// <param name="sldWorks"></param>
        /// <param name="window">the wpf window</param>
        /// <returns>A wrapper object that allows you to show or show-as-dialogue the window</returns>
        public static PopupWpfWindow HookWpfWindow(this ISldWorks sldWorks, Window window)
        {
            var parent = (IntPtr)sldWorks.IFrameObject().GetHWndx64();
            return new PopupWpfWindow(window, parent);
        }
    }
}
