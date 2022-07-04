using Hymma.Solidworks.Addins;
using QRCoder;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;

namespace QRify
{
    [Addin(title: "Qrify", AddinIcon = "qrify.png", Description = "Creates a QR code as a picture", LoadAtStartup = true)]
    [Guid("BB7927D1-9EAD-489F-B6EF-D48BB9091182")]
    [ComVisible(true)]
    public class Qrify : AddinMaker
    {
        public override AddinUserInterface GetUserInterFace()
        {
            return new QrifyUserInterface(this.Solidworks);
        }

        private object EnablePropertyMangagerPage()
        {
            if (Solidworks.ActiveDoc == null || Solidworks.CommandInProgress)
            {
                return 0;
            }
            return 1;

        }

        public void ShowQrifyPropertyManagerPage()
        {
            if (Solidworks.ActiveDoc is DrawingDoc drawing)
            {
                pmpFactory.Show();
            }
        }
    }


    /// <summary>
    /// This will add a command to the addin tab, once user clicked on it the property manger page should pop up
    /// </summary>
    public class QrCommand : AddinCommand
    {
        private ISldWorks sldworks;
        private PmpFactoryBase pmpFactory;

        public QrCommand(ISldWorks sldWorks, PmpFactoryBase pmpFactory)
        {
            // this property must be set to a value otherwise solidworks wont show the command
            this.IconBitmap = Properties.Resources.qrify;

            //this has to be a string and it has to be name of a command in this assembly, solidworks restrictions
            this.CallBackFunction = nameof(ShowQrifyPropertyManagerPage);

            
            this.EnableMethode = nameof(EnablePropertyMangagerPage);
            this.sldworks = sldWorks;
            this.pmpFactory = pmpFactory;
        }

        private object EnablePropertyMangagerPage()
        {
            if (sldworks.ActiveDoc == null || sldworks.CommandInProgress)
            {
                return 0;
            }
            return 1;

        }

        public void ShowQrifyPropertyManagerPage()
        {
            if (sldworks.ActiveDoc is DrawingDoc drawing)
            {
                pmpFactory.Show();
            }
        }
    }

    public class QrPropertyManagerPageTab : PmpTab
    {
        public QrPropertyManagerPageTab() : base("Generate Qr", Properties.Resources.qrify)
        {
            var controls = new List<IPmpControl>();
            var textBox = new PmpTextBox("Text to convert to QR");
            var btn = new PmpBitmapButton(Properties.Resources.qrify, "Generate a qr picture representing above text", BtnSize.forty, byte.MaxValue);
            btn.Clicked += (s, e) =>
            {
                textBox.TextColor = Color.Green;
                SaveQrToClipboard(textBox);
                textBox.Value = "Press Ctrl+V to paste the code";
            };
            textBox.Displaying += (s, e) => textBox.TextColor = Color.Black;
            controls.Add(textBox);
            controls.Add(btn);
            var group = new PmpGroup("", controls);
            TabGroups = new List<PmpGroup> { group };
        }

        private void SaveQrToClipboard(PmpTextBox textBox)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(textBox.Value, QRCodeGenerator.ECCLevel.Q);
            AsciiQRCode qrCode = new AsciiQRCode(qrCodeData);
            var code = qrCode.GetGraphic(1);
            Clipboard.SetText(code);
        }
    }

    public class QrPropertyManagerPage : PmpUiModel
    {
        public QrPropertyManagerPage(ISldWorks solidworks) : base(solidworks)
        {
            this.PmpTabs = new List<PmpTab>() { new QrPropertyManagerPageTab() };
        }
    }

    public class QrPropertyManagerPageFactory : PmpFactoryX64
    {
        public QrPropertyManagerPageFactory(ISldWorks sldWorks) : base(new QrPropertyManagerPage(sldWorks))
        {

        }
    }

    public class QrTab : AddinCommandTab
    {
        public QrTab(ISldWorks sldWorks, QrPropertyManagerPageFactory pmpFactory)
        {
            Types = new[] { swDocumentTypes_e.swDocDRAWING };
            TabTitle = "Qrify";
            var commands = new[] { new QrCommand(sldWorks, pmpFactory) };
            CommandGroup = new AddinCommandGroup(0, commands, "QRify this", "Create a QR code", "Create a QR code", "Create a QR code", Properties.Resources.qrify);
        }
    }

    public class QrifyUserInterface : AddinUserInterface
    {
        public QrifyUserInterface(ISldWorks sldWorks)
        {
            var pmpFactory = new QrPropertyManagerPageFactory(sldWorks);
            CommandTabs = new List<AddinCommandTab>() { new QrTab(sldWorks, pmpFactory) };
            this.PropertyManagerPages = new List<PmpFactoryBase> { pmpFactory };
        }
    }
}
