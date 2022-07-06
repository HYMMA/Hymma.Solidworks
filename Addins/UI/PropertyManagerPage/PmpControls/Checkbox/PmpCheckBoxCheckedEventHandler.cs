// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// event handler for <see cref="PmpCheckBox.Checked"/>
    /// </summary>
    /// <param name="pmpCheckBox">the controller</param>
    /// <param name="isChecked">the state of the checkbox passed in by SOLIDWORKS</param>
    public delegate void PmpCheckBoxCheckedEventHandler(PmpCheckBox pmpCheckBox, bool isChecked);
}
