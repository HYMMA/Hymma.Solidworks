namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// event handerl for <see cref="PmpListBox"/>
    /// </summary>
    /// <typeparam name="EventArgs"></typeparam>
    /// <param name="sender">the list box controller that raised this event</param>
    /// <param name="eventArgs">useful arguments passed on by SOLIDWORKS for you</param>
    public delegate void Listbox_EventHandler<EventArgs>(PmpListBox sender, EventArgs eventArgs);
}
