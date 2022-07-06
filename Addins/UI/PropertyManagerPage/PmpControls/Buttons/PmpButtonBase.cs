// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;
using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// represents a base class for all buttons in a property manager page 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PmpButtonBase<T> : PmpControl<T>
    {
        #region constructor
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="caption"></param>
        /// <param name="tip"></param>
        public PmpButtonBase(swPropertyManagerPageControlType_e type, string caption, string tip) : base(type, caption, tip)
        {
        }
        #endregion

        #region call backs
        internal void ClickedCallback() => Clicked?.Invoke(this, EventArgs.Empty);
        #endregion

        #region events
        /// <summary>
        /// invoked when this button is clicked
        /// </summary>
        public event EventHandler Clicked;
        #endregion
    }
}
