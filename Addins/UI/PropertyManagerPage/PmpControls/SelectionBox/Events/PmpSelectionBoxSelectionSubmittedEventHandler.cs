using System.Runtime.InteropServices;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// and event handelr for <see cref="PmpSelectionBox.SelectionSubmitted"/>
    /// </summary>
    /// <param name="sender">the controller that fired the event</param>
    /// <param name="eventArgs">useful event arguments provided to  you by SOLIDWORKS</param>
    /// <returns>ture to selection to happen and false to disregard all selections</returns>
    [ComVisible(true)]
    public delegate bool PmpSelectionBoxSelectionSubmittedEventHandler(PmpSelectionBox sender, PmpSelectionBoxSelectionSubmittedEventArgs eventArgs);
}
