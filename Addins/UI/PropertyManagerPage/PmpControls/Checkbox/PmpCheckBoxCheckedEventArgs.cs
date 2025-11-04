// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// event handler for <see cref="PmpCheckBox.Checked"/>
    /// </summary>
    /// <param name="pmpCheckBox">the controller</param>
    /// <param name="isChecked">the state of the checkbox passed in by SOLIDWORKS</param>
    //public delegate void PmpCheckBoxCheckedEventHandler(PmpCheckBox pmpCheckBox, bool isChecked);

    /// <summary>
    /// event arguments for <see cref="PmpCheckBox.Checked"/>
    /// </summary>
    public class PmpCheckBoxCheckedEventArgs : EventArgs
    {
        /// <summary>
        /// the state of the checkbox passed in by SOLIDWORKS
        /// </summary>
        public bool IsChecked { get; }
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="isChecked">the state of the checkbox passed in by SOLIDWORKS</param>
        public PmpCheckBoxCheckedEventArgs(bool isChecked)
        {
            IsChecked = isChecked;
        }
    }
}
