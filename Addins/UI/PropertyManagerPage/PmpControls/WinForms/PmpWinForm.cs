// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using System;

namespace Hymma.Solidworks.Addins.UI
{
    /// <summary>
    /// allows creating a winform in a propeprty manager page
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PmpWinForm<T> : PmpControl<IPropertyManagerPageWindowFromHandle> where T : System.Windows.Forms.Form, new()
    {
        private T _userControl;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="height">if set to zero the winForm will not be shown</param>
        /// <param name="caption">caption for the winForm</param>
        /// <param name="tip">a tip for the controller</param>
        public PmpWinForm(int height, string caption = "", string tip = "") : base(SolidWorks.Interop.swconst.swPropertyManagerPageControlType_e.swControlType_WindowFromHandle, caption, tip)
        {
            Registering += () => SolidworksObject.Height = height;
            Displaying += (s, d) =>
            {
                //user needs to create the dotnet control at every display
                _userControl = Activator.CreateInstance(typeof(T)) as T;

                _userControl.Enabled = Enabled;

                //this is suggested by solidworks website
                _userControl.TopLevel = false;

                //again from solidworks website
                _userControl.Show();
                SolidworksObject?.SetWindowHandlex64(_userControl.Handle.ToInt64());
            };
        }

        ///<inheritdoc/>
        public override bool Enabled
        {
            get => base.Enabled;
            set
            {
                base.Enabled = value;
                if (_userControl!=null)
                _userControl.Enabled = value;
            }
        }
    }
}
