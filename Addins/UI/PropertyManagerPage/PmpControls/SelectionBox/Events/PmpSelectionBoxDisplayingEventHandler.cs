using System.Runtime.InteropServices;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// an event handler for <see cref="PmpSelectionBox"/> where th event returns some event arguments
    /// </summary>
    /// <param name="sender">the controller</param>
    /// <param name="e">event arguments provided to you by SOLIDWORKS when this event happens</param>
    /// <returns></returns>
    [ComVisible(true)]
    public delegate void PmpSelectionBoxDisplayingEventHandler(PmpSelectionBox sender, PmpSelectionBoxDisplayingEventArgs e);


    /// <summary>
    /// an event handler for <see cref="PmpSelectionBox"/> 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    [ComVisible(true)]
    public delegate void PmpSelectionBoxListChangedEventHandler(PmpSelectionBox sender, PmpSelectionBoxListChangedEventArgs e);
}
