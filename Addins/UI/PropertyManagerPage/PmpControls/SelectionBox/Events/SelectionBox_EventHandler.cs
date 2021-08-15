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
    public delegate void SelectionBox_EventHandler<EventArgs>(PmpSelectionBox sender, EventArgs eventArgs);

    /// <summary>
    /// handles events for a <see cref="PmpSelectionBox"/> event that does not have any event argument
    /// </summary>
    /// <param name="sender">is the selection box that fired the event</param>
    [ComVisible(true)]
    public delegate void SelectionBox_EventHandler(PmpSelectionBox sender);

}
