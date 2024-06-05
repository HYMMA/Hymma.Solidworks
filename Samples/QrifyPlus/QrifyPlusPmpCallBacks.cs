// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins;
using SolidWorks.Interop.sldworks;
namespace QrifyPlus
{
    public class QrifyPlusPmpCallBacks
    {
        private PmpCloseReason _reason;

        public QrifyPlusPmpCallBacks(ISldWorks sldWorks)
        {
            this.Solidworks = sldWorks;
        }

        public ISldWorks Solidworks { get; }

        public void AfterClose()
        {
            if (_reason == PmpCloseReason.Okay)
            {
                Solidworks.SendMsgToUser("You're loved.\r\nYou can use CTRL + V to save the QR on your drawing. Alternatively you could contribute to this Addin on Github to remove this message. And insert the image into the drawing");
            }
        }
        public void DuringClose(PmpCloseReason reason)
        {
            //Solidworks does expose this API but blocks all commands. so you wont be able to run you add-in logic at this moment. just register what button user has pressed
            //green check-mark or red cross button and use this in AfterClose
            this._reason = reason;
        }

    }
}
