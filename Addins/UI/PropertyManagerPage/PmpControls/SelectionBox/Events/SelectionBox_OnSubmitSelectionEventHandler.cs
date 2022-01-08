using System.Runtime.InteropServices;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// and event handelr for <see cref="PmpSelectionBox.OnSubmitSelection"/>
    /// </summary>
    /// <param name="sender">the controller that fired the event</param>
    /// <param name="eventArgs">useful event arguments provided to  you by SOLIDWORKS</param>
    /// <returns>ture to selection to happen and false to disregard all selections</returns>
    [ComVisible(true)]
    public delegate bool SelectionBox_OnSubmitSelectionEventHandler(PmpSelectionBox sender, SelectionBox_OnSubmitSelection_EventArgs eventArgs);
}
