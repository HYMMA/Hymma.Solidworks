using System.Runtime.InteropServices;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// handles events for a <see cref="PmpSelectionBox"/> event that does not have any event argument
    /// </summary>
    /// <param name="sender">is the selection box that fired the event</param>
    [ComVisible(true)]
    public delegate void SelectionBox_EventHandlerWithoutEventArgs(PmpSelectionBox sender);
}
