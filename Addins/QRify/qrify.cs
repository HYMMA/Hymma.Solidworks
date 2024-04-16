using Hymma.Solidworks.Addins;
using Hymma.Solidworks.Addins.Helpers;
using QRCoder;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace QRify
{
    //It is not mandatory to make this class partial, but in future releases we might use code generators to bypass some of solidworks API restrictions
    //AddinIcon could be a resx file or an Embedded Resource one 
    [Addin(title: "QRify",
        AddinIcon = "qrify.png",
        Description = "Creates a QR",
        LoadAtStartup = true,
        EventSource = "Qrify Addin")]
    [ComVisible(true)]
    [Guid("2EB85AF6-DB51-46FB-B955-D4A7708DA315")]
    public partial class Qrify : AddinMaker
    {
        private PmpFactoryBase pmpFactory;
        public override AddinUserInterface GetUserInterFace()
        {
            var ui = new QrifyUserInterface(this.Solidworks);
            pmpFactory = ui.PmpFactory;
            return ui;
        }

        //you can move this region to Qrify.g.cs
        #region Call back functions
        /// <summary>
        /// This is a call back function from <see cref="QrCommand"/>
        /// </summary>
        /// <returns></returns>
        public object EnablePropertyManagerPage()
        {
            if (Solidworks.ActiveDoc == null || Solidworks.CommandInProgress)
            {
                return 0;
            }
            return 1;

        }

        /// <summary>
        /// This is a call back function from <see cref="QrCommand"/>
        /// </summary>
        /// <returns></returns>
        public void ShowQrifyPropertyManagerPage()
        {
            if (Solidworks.ActiveDoc is DrawingDoc drawing)
            {
                pmpFactory.Show();
            }
        }
        #endregion
    }

    #region Property Manager Page 
    public class QrPropertyManagerPageGroup : PmpGroup
    {
        private PmpTextBox textBox;

        public QrPropertyManagerPageGroup()
        {
            //text box
            textBox = new PmpTextBox("www.hymma.net");

            //button to invoke the QR generation.
            var btn = new PmpBitmapButton(Properties.Resources.qrify, "Generate a qr picture representing above text", BtnSize.thirtyTwo, byte.MaxValue);

            //once clicked on button
            btn.Clicked += Btn_Clicked;

            //Add controls using base class helper method
            AddControls(new List<IPmpControl>
            {
                textBox,
                btn
            });
        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            //generate qr code and save it in clipboard
            SaveQrToClipboard(textBox);
            var btn = sender as PmpBitmapButton;
            btn.ShowBubleTooltip("Success", "Copied into clipboard, use Ctrl+v to paste", Properties.Resources.info, "successImageFileName");
        }

        private void SaveQrToClipboard(PmpTextBox textBox)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(textBox.Value, QRCodeGenerator.ECCLevel.Q);
            var qrImage = ArtQRCodeHelper.GetQRCode(textBox.Value, 5, System.Drawing.Color.Black, System.Drawing.Color.White, System.Drawing.Color.Gray, QRCodeGenerator.ECCLevel.L);
            using (qrImage)
            {
                var src = Imaging.CreateBitmapSourceFromHBitmap(qrImage.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                Clipboard.SetImage(src);
            }
        }
    }

    public class QrPropertyManagerPageTab : PmpTab
    {
        public QrPropertyManagerPageTab() : base("Generate Qr", Properties.Resources.qrify)
        {
            //add the group to the tab
            this.TabGroups = new List<PmpGroup> { new QrPropertyManagerPageGroup() };
        }
    }

    public class QrPropertyManagerPage : PmpUiModel
    {
        private PmpCloseReason closeReson;

        public QrPropertyManagerPage(ISldWorks solidworks) : base(solidworks)
        {
            this.PmpTabs = new List<PmpTab>() { new QrPropertyManagerPageTab() };

            //at this moment solidworks disables most of its API functions. So if you decided to add a sheet to a drawing for example, it wont work
            this.Closing += QrPropertyManagerPage_Closing;

            //this is where you should run any command in solidworks
            this.AfterClose += QrPropertyManagerPage_AfterClose;
        }

        private void QrPropertyManagerPage_Closing(PmpCloseReason obj)
        {
            if (obj == PmpCloseReason.Cancel)
            {
                closeReson = obj;
                Clipboard.Clear();
            }
        }

        private void QrPropertyManagerPage_AfterClose()
        {
            if (closeReson == PmpCloseReason.Okay)
            {
                //Run other commands here
            }
        }
    }

    public class QrPropertyManagerPageFactory : PmpFactoryX64
    {
        public QrPropertyManagerPageFactory(ISldWorks sldWorks) : base(new QrPropertyManagerPage(sldWorks))
        {
        }
    }
    #endregion

    #region Addin button that invokes the property manger page
    public class QrCommand : AddinCommand
    {
        public QrCommand()
        {
            this.CommandTabTextType = ((int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow);
            this.IconBitmap = Properties.Resources.qrify;

            //Restrictions imposed by solidworks API:
            //These two methods must be defined in the addin class (addin class inherits from AddinMaker.cs)
            this.EnableMethode = "EnablePropertyMangagerPage";
            this.CallBackFunction = "ShowQrifyPropertyManagerPage";

            this.Name = "QRify";
            this.HintString = "Get QR code";

            //solidworks uses the ToolTip as the command name. maybe its a bug in their API.
            //workaround is to use Name as the ToolTip
            this.ToolTip = Name;
        }
    }
    public class QrCommandGroup : AddinCommandGroup
    {
        public QrCommandGroup()
        {
            this.Commands = new List<QrCommand>() { new QrCommand() };
            this.Title = "Title for QRify command group";
            this.Description = "QRify command group";
            this.Hint = "Hint for command group";
            this.ToolTip = "ToolTip for command group";
            this.MainIconBitmap = Properties.Resources.qrify;
        }
    }
    public class QrTab : AddinCommandTab
    {
        public QrTab()
        {
            Types = new[] { swDocumentTypes_e.swDocDRAWING };
            TabTitle = "QRify";
            CommandGroup = new QrCommandGroup();
        }
    }

    public class QrifyUserInterface : AddinUserInterface
    {
        public QrPropertyManagerPageFactory PmpFactory { get; }
        public QrifyUserInterface(ISldWorks sldWorks)
        {
            PmpFactory = new QrPropertyManagerPageFactory(sldWorks);
            CommandTabs = new List<AddinCommandTab>() { new QrTab() };
            this.PropertyManagerPages = new List<PmpFactoryX64> { PmpFactory };
        }
    }
    #endregion
}
