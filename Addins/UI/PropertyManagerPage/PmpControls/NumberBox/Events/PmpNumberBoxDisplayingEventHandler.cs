using System.Runtime.InteropServices;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// handler for <see cref="PmpNumberBox.Displaying"/>
    /// </summary>
    /// <param name="sender">the number box in the property manager page</param>
    /// <param name="eventArgs">useful commands to use once this event is raised</param>
    [ComVisible(true)]
    public delegate void PmpNumberBoxDisplayingEventHandler(PmpNumberBox sender, PmpNumberBoxDisplayingEventArgs eventArgs);
}
