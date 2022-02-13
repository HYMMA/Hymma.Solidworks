namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// event handler for <see cref="PmpListBox.Displaying"/>
    /// </summary>
    /// <param name="sender">the listbox that raised the event</param>
    /// <param name="eventArgs">useful arguments passed by SOLIDWORKS</param>
    public delegate void PmpListboxDisplayingEventHandler(PmpListBox sender, PmpListboxDisplayingEventArgs eventArgs);
}
