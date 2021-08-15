using System.Runtime.InteropServices;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// an event handler for <see cref="PmpSelectionBox"/> where th event returns some event arguments
    /// </summary>
    /// <typeparam name="EventArgs"></typeparam>
    /// <param name="sender">the controller</param>
    /// <param name="eventArgs">event arguments provided to you by SOLIDWORKS when this event happens</param>
    /// <returns></returns>
    [ComVisible(true)]
    public delegate bool SelectionBox_EventHandlerWithEventArgs<EventArgs>(PmpSelectionBox sender, EventArgs eventArgs);
}
