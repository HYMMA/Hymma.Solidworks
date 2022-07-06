// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System.Runtime.InteropServices;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// event handler for a <see cref="IPmpControl.Displaying"/> event
    /// </summary>
    /// <param name="sender">the </param>
    /// <param name="eventArgs"></param>
    [ComVisible(true)]
    public delegate void PmpControlDisplayingEventHandler(IPmpControl sender, PmpControlDisplayingEventArgs eventArgs);

}
