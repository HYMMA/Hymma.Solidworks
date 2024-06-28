// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.Windows;
using System.Windows.Forms;

namespace Hymma.Solidworks.Addins.UI.PopUps
{
    /// <summary>
    /// concrete implementation of <see cref="IWin32Window"/> this will act as a proxy for solidworks window, which then hosts <see cref="PopupWpfWindow"/> and <see cref="PopupWinForm"/>
    /// </summary>
    public class Win32Window : IWin32Window
    {
        /// <summary>
        /// Handler winForm solidworks window
        /// </summary>
        public IntPtr Handle { get; }

        /// <summary>
        /// makes the object 
        /// </summary>
        /// <param name="handle">the handler winForm solidworks window frame</param>
        public Win32Window(IntPtr handle)
        {
            Handle = handle;
        }
    }

    /// <summary>
    ///Allows using a WPF windows directly inside solidworks as a modal or modeless window
    /// </summary>
    public class PopupWpfWindow
    {
        /// <summary>
        /// creates the object to interact with the Window 
        /// </summary>
        /// <param name="wpfWindow">the wpf window to hook into solidworks</param>
        /// <param name="parent">this is the handler of the solidworks frame object</param>
        public PopupWpfWindow(Window wpfWindow, IntPtr parent)
        {
            m_ParentWnd = parent;

            this.wpfWindow = wpfWindow;
            m_Owner = new System.Windows.Interop.WindowInteropHelper(this.wpfWindow)
            {
                Owner = parent
            };

            disposed = false;
        }

        /// <summary>
        /// controls the visibility of the window
        /// </summary>
        public bool IsActive
        {
            get => wpfWindow.IsVisible;
            set
            {
                if (value)
                {
                    Show();
                }
                else
                {
                    wpfWindow.Hide();
                }
            }
        }

        private readonly Window wpfWindow;

        private readonly System.Windows.Interop.WindowInteropHelper m_Owner;

        private bool disposed;

        private readonly IntPtr m_ParentWnd;

        /// <summary>
        /// closes the window and disposes of the unmanaged resources
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// closes the window, disposes the unmanaged resources
        /// </summary>
        public void Close()
        {
            if (!disposed)
            {
                disposed = true;
                wpfWindow.Close();
            }
        }

        /// <summary>
        /// shows the windows as a modal window that blocks interaction with the rest of the application
        /// </summary>
        /// <param name="dock"></param>
        /// <returns></returns>
        public bool? ShowDialog(ScreenZones dock = ScreenZones.Center)
        {
            var startupLoc = wpfWindow.WindowStartupLocation;

            wpfWindow.Loaded += (s, e) =>
            {
                PositionWindow(dock);
            };
            var res = wpfWindow.ShowDialog();

            wpfWindow.WindowStartupLocation = startupLoc;

            return res;
        }

        /// <summary>
        /// shows the window as a modeless window that allows interaction with the rest of the application
        /// </summary>
        /// <param name="dock"></param>
        public void Show(ScreenZones dock = ScreenZones.Center)
        {
            var startupLoc = wpfWindow.WindowStartupLocation;
            wpfWindow.Show();

            PositionWindow(dock);
            wpfWindow.BringIntoView();

            wpfWindow.WindowStartupLocation = startupLoc;
        }

        private void PositionWindow(ScreenZones dock)
        {
            var pos = PopupHelper.CalculateLocation(m_ParentWnd, dock, true, wpfWindow.Width, wpfWindow.Height,
                new Thickness(wpfWindow.Padding.Left, wpfWindow.Padding.Right, wpfWindow.Padding.Top, wpfWindow.Padding.Bottom));
            wpfWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            wpfWindow.Left = pos.X;
            wpfWindow.Top = pos.Y;
        }
    }


    /// <summary>
    /// allows using a WinFORM directly inside solidworks as a modal or modeless window
    /// </summary>
    public class PopupWinForm
    {
        private readonly Form winForm;
        private readonly IWin32Window solidworksFrame;
        private bool isDisposed;

        /// <summary>
        /// creates an object and hooks it into solidworks frame object
        /// </summary>
        /// <param name="winForm"></param>
        /// <param name="parent"></param>
        public PopupWinForm(Form winForm, IntPtr parent)
        {
            this.winForm = winForm;
            solidworksFrame = new Win32Window(parent);
            isDisposed = false;
        }

        /// <summary>
        /// controls the visibility of the window
        /// </summary>
        public bool IsActive
        {
            get => winForm.Visible;
            set
            {
                if (value)
                {
                    Show();
                }
                else
                {
                    winForm.Hide();
                }
            }
        }

        /// <summary>
        /// closes the window and disposes of the unmanaged resources
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// closes the window and disposes of the unmanaged resources
        /// </summary>
        public void Close()
        {
            if (isDisposed)
            {
                isDisposed = true;
                winForm.Close();
            }
        }

        /// <summary>
        /// shows the windows as a modal window that blocks interaction with the rest of the application
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        public DialogResult ShowDialog(ScreenZones zone = ScreenZones.Center)
        {
            var startupLoc = winForm.StartPosition;
            PositionWindow(zone);
            var res = winForm.ShowDialog(solidworksFrame);

            winForm.StartPosition = startupLoc;
            return res;
        }

        /// <summary>
        /// shows the window in modeless mode that allows user interact with the rest of the application
        /// </summary>
        /// <param name="zone"></param>
        public void Show(ScreenZones zone = ScreenZones.Center)
        {
            var startupLoc = winForm.StartPosition;
            PositionWindow(zone);
            winForm.Show(solidworksFrame);
            winForm.BringToFront();
            winForm.StartPosition = startupLoc;
        }

        void PositionWindow(ScreenZones zone)
        {
            var pos = PopupHelper.CalculateLocation(solidworksFrame.Handle, zone, false, winForm.Width, winForm.Height,
                new Thickness(winForm.Padding.Left, winForm.Padding.Right, winForm.Padding.Top, winForm.Padding.Bottom));
            winForm.StartPosition = FormStartPosition.Manual;
            winForm.DesktopLocation = pos;
        }
    }
}

