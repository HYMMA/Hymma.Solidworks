using System.Runtime.InteropServices;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// an event handler for <see cref="PmpSelectionBox"/> 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    [ComVisible(true)]
    public delegate void PmpSelectionBoxListChangedEventHandler(PmpSelectionBox sender, PmpSelectionBoxListChangedEventArgs e);
}
