using Hymma.SolidTools.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTools.Addins.UI.PropertyManagerPage.PmpControls.Message
{
    public class PmpMessage : PmpGroup
    { 
         

        public PmpMessage(string message, SysColor backgroundColor)
        {
            this.Message = message;
            BackgroundColor = backgroundColor;
        }

        public string Message { get; }
    }
}
