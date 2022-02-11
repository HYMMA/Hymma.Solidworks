using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// event handler for <see cref="PmpListBox.RightClicked"/>
    /// </summary>
    /// <param name="sender">the list box controller that raised this event</param>
    /// <param name="point">where user released the right mouse button</param>
    public delegate void PmpListboxRightClickedEventHandler(PmpListBox sender, Tuple<double, double, double> point);
}
