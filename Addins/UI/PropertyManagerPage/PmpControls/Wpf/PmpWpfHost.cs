﻿// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Windows.Forms.Integration;
using Xarial.XCad.SolidWorks.Utils;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a windows winForm host that solidworks uses to show win forms or wpf
    /// </summary>
    /// <remarks>your addin must add a reference to WindowsFormsIntegration</remarks>
    public class PmpWpfHost : PmpControl<IPropertyManagerPageWindowFromHandle>, IEquatable<PmpWpfHost>, IDisposable
    {
        #region constructors
        /// <summary>
        /// default constructor 
        /// </summary>
        /// <param name="elementHost">solidworks uses <see cref="System.Windows.Forms.Integration.ElementHost"/> to hook into a windows winForm</param>
        /// <param name="wpfControl">wpf controller</param>
        /// <param name="height">height of this control in property manager page if set to zero the control will not appear</param>
        public PmpWpfHost(ElementHost elementHost, System.Windows.Controls.UserControl wpfControl, int height) : base(swPropertyManagerPageControlType_e.swControlType_WindowFromHandle)
        {
            this.ElementHost = elementHost;
            this.WindowsControl = wpfControl;
            _keystrokePropagator = new WpfControlKeystrokePropagator(wpfControl);
            Displaying += PmpWpfHost_OnDisplay;
            Registering += () => SolidworksObject.Height = height;
        }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="winFormOrWpfControl">a wpf controller</param>
        /// <param name="height">height of this control in property manager page if set to zero the control will not appear</param>
        public PmpWpfHost(System.Windows.Controls.UserControl winFormOrWpfControl, int height) : this(new ElementHost(), winFormOrWpfControl, height)
        {

        }
        #endregion

        #region call backs
        private void PmpWpfHost_OnDisplay(object sender, PmpControlDisplayingEventArgs e)
        {
            //this should be called every time pmp is displayed and on the pmp registration
            if (ElementHost == null || !WindowsControl.HasContent)
                return;

            ElementHost.Child = WindowsControl;
            SolidworksObject?.SetWindowHandlex64(ElementHost.Handle.ToInt64());
        }

        #endregion

        #region public properties

        /// <summary>
        /// A host for <see cref="WindowsControl"/>
        /// </summary>
        public ElementHost ElementHost { get; }

        /// <summary>
        /// a windows winForm or wpf controller
        /// </summary>
        public System.Windows.Controls.UserControl WindowsControl { get; }

        private WpfControlKeystrokePropagator _keystrokePropagator;
        #endregion

        #region methods
        /// <summary>
        /// makes sure each SwWindowHandler has its unique ElementHost
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(PmpWpfHost other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) ? true : ElementHost == other.ElementHost;
        }

        ///<inheritdoc/>
        public override bool Equals(object obj) => Equals(obj as PmpWpfHost);

        ///<inheritdoc/>
        public override int GetHashCode() => ElementHost.GetHashCode();

        /// <summary>
        /// properly disposes of this object
        /// </summary>
        public void Dispose()
        {
            _keystrokePropagator.Dispose();
            ElementHost.Dispose();
        }

        ///<inheritdoc/>
        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
                WindowsControl.IsEnabled = value;
            }
        }
        #endregion
    }
}
