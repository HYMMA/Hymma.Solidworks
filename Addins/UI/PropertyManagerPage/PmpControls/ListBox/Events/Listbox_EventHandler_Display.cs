namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// event handler for <see cref="PmpListBox.OnDisplay"/>
    /// </summary>
    /// <param name="sender">the listbox that raised the event</param>
    /// <param name="eventArgs">useful arguments passed by SOLIDWORKS</param>
    public delegate void Listbox_EventHandler_Display(PmpListBox sender, Listbox_OnDisplay_EventArgs eventArgs);
}
