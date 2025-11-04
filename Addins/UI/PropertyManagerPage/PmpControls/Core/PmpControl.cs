// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;
using System.Runtime.InteropServices;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// a SOLIDWORKS property manager page control
    /// </summary>
    public class PmpControl<T> : IPmpControl, IWrapSolidworksObject<T>
    {
        #region constructor
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="type">type of this controller as per <see cref="swPropertyManagerPageControlType_e"/></param>
        /// <param name="caption">caption for this control</param>
        /// <param name="tip">tip for this control</param>
        public PmpControl(swPropertyManagerPageControlType_e type, string caption = "", string tip = "")
            : base(type, caption, tip)
        {
            Registering += PmpControl_Registering;
        }

        private void PmpControl_Registering(object sender, System.EventArgs e)
        {
            SolidworksObject = (T)Control;
        }

        /// <summary>
        /// release solidworks object
        /// </summary>
        public void ReleaseSolidworksObject()
        {
            Marshal.ReleaseComObject(SolidworksObject);
        }
        #endregion
        ///<inheritdoc/>
        public T SolidworksObject { get; internal set; }
    }
}
