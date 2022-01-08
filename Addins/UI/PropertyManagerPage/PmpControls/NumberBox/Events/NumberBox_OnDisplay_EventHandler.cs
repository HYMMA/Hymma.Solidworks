using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.Solidworks.Addins
{

    /// <summary>
    /// handler for <see cref="PmpNumberBox.OnDisplay"/>
    /// </summary>
    /// <param name="sender">the number box in the property manager page</param>
    /// <param name="eventArgs">useful commands to use once this event is raised</param>
    [ComVisible(true)]
    public delegate void NumberBox_OnDisplay_EventHandler(PmpNumberBox sender, NumberBox_Ondisplay_EventArgs eventArgs);
}
