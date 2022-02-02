using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// event handerl for <see cref="PmpListBox.RightMouseBtnUp"/>
    /// </summary>
    /// <param name="sender">the list box controller that raised this event</param>
    /// <param name="point">where user released the right mouse button</param>
    public delegate void PmpListboxRmbEventHandler(PmpListBox sender, Tuple<double, double, double> point);
}
