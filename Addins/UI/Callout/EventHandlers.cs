using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTools.Addins
{
    public class EventHandlers
    {
        /// <summary>
        /// event handler for when a new line of text is added to this callout
        /// </summary>
        /// <param name="sender">is the <see cref="SwCallout"/></param>
        /// <param name="eventArgs"> has properties useful for when an event is raised <see cref="CallOutEventArgs"/></param>
        /// <returns></returns>

        public delegate bool CalloutEventHandler_OnAddValue(SwCallout sender, CallOutEventArgs eventArgs);
    }
}
