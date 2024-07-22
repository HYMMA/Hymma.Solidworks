// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins;
using Hymma.Solidworks.Addins.Helpers;
using QRCoder;
using QRify.Logging;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Environment = System.Environment;

namespace QRify
{
    //It is not mandatory to make this class partial, but in future releases we might use code generators to bypass some of SolidWORKS API restrictions
    //AddinIcon could be a resx file or an Embedded Resource one 
    [Addin(title: "QRify",
        AddinIcon = "HforHymma.jpg",
        Description = "Creates a QR",
        LoadAtStartup = true)]
    [ComVisible(true)]
    [Guid("2EB85AF6-DB51-46FB-B955-D4A7708DA315")]
    public partial class Qrify : AddinMaker
    {
        private PropertyManagerPageBase pmpFactory;
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
            btn.ShowBubbleTooltip("Success", "Copied into clipboard, use Ctrl+v to paste", Properties.Resources.info, "successImageFileName");
        }

        private void SaveQrToClipboard(PmpTextBox textBox)
        {

            var value = textBox.Value;
            var logger = new QRifyLogger();
            logger.Info(value);
            var qrImage = ArtQRCodeHelper.GetQRCode(value, 5, System.Drawing.Color.Black, System.Drawing.Color.White, System.Drawing.Color.Gray, QRCodeGenerator.ECCLevel.L);
            using (qrImage)
            {
                var src = Imaging.CreateBitmapSourceFromHBitmap(qrImage.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                Clipboard.SetImage(src);
                //src = null;
            }
        }
    }

    public class QrPropertyManagerPageSettingGroup : PmpGroup
    {
        public QrPropertyManagerPageSettingGroup()
        {
            var lblSunken = new PmpLabel("A label with sunk text", LabelStyles.Sunken);
            var lblRight = new PmpLabel("A label with right aligned text", LabelStyles.RightText);
            var btn = new PmpButton("Button", "tip for this button");
            var img = new PmpBitmap(Properties.Resources.info, "info button", ControlResizeStyles.LockLeft);
            var bitMapbtn = new PmpBitmapButton(Properties.Resources.info, "tip for the bitmap button", BtnSize.thirtyTwo, byte.MaxValue) { Top = 400};
            var imgLbl = new PmpLabel("Info") { Top = 400 ,Left = 30};
            var list = new PmpListBox(new[] { "item 1", "item 2", "item 3", "item 4" }, "caption for list", "tip for list", 0, ListboxStyles.AllowMultiSelect);

            AddControls(new List<IPmpControl>() { lblSunken, lblRight, btn, img,  bitMapbtn, imgLbl,list });
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
    public class QrPropertyManagerPageSettingsTab : PmpTab
    {
        public QrPropertyManagerPageSettingsTab() : base("Settings")
        {
            //add the group to the tab
            this.TabGroups = new List<PmpGroup> { new QrPropertyManagerPageSettingGroup() };
        }
    }

    public class QrPropertyManagerPageUiModel : PmpUiModel
    {
        private PmpCloseReason closeReason;

        public QrPropertyManagerPageUiModel(ISldWorks solidworks) : base(solidworks, "Qrify")
        {
            this.PmpTabs = new List<PmpTab>() { new QrPropertyManagerPageTab(), new QrPropertyManagerPageSettingsTab() };

            //at this moment SolidWORKS disables most of its API functions. So if you decided to add a sheet to a drawing for example, it wont work
            this.Closing += QrPropertyManagerPage_Closing;

            //this is where you should run any command in SolidWORKS
            this.AfterClose += QrPropertyManagerPage_AfterClose;
        }

        private void QrPropertyManagerPage_Closing(PmpUiModel sender,PmpCloseReason obj)
        {
            if (obj == PmpCloseReason.Cancel)
            {
                closeReason = obj;
                Clipboard.Clear();
            }
        }

        private void QrPropertyManagerPage_AfterClose(PmpUiModel sender)
        {
            if (closeReason == PmpCloseReason.Okay)
            {
                //Run other commands here
            }
        }
    }

    public class QrPropertyManagerPage : PropertyManagerPageX64
    {
        public QrPropertyManagerPage(ISldWorks sldWorks) : base(new QrPropertyManagerPageUiModel(sldWorks))
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

            //Restrictions imposed by SolidWORKS API:
            //These two methods must be defined in the addin class (addin class inherits from AddinMaker.cs)
            this.EnableMethod = "EnablePropertyMangagerPage";
            this.CallBackFunction = "ShowQrifyPropertyManagerPage";

            this.Name = "QRify";
            this.HintString = "Get QR code";

            //SolidWORKS uses the ToolTip as the command name. maybe its a bug in their API.
            //workaround is to use Name as the ToolTip
            this.ToolTip = Name;
        }
    }
    public class QrCommandHelp : AddinCommand
    {
        public QrCommandHelp()
        {
            this.CommandTabTextType = ((int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow);
            this.IconBitmap = Properties.Resources.info;

            this.EnableMethod = "EnablePropertyMangagerPage";
            this.CallBackFunction = "ShowQrifyPropertyManagerPage";

            ToolTip = "Help for Qrify";
            HintString = "How to get help!";
            Name = "Qrify Helps";
        }
    }
    public class QrCommandGroup : AddinCommandGroup
    {
        public QrCommandGroup() : base(0,
                                       new List<AddinCommand>() { new QrCommand(), new QrCommandHelp() },
                                       "Title for Qrify command group",
                                       "Description for Qrify",
                                       "make QR codes",
                                       "Tooltip",
                                       Properties.Resources.qrify)
        {
            //this.Commands = new List<QrCommand>() { new QrCommand() };
            //this.Title = "Title for QRify command group";
            //this.Description = "QRify command group";
            //this.Hint = "Hint for command group";
            //this.ToolTip = "ToolTip for command group";
            //this.MainIconBitmap = Properties.Resources.qrify;
        }
    }
    public class QrTab : AddinCommandTab
    {
        public QrTab()
        {
            DocTypes = new[] { swDocumentTypes_e.swDocDRAWING };
            Title = "QRify";
            CommandGroup = new QrCommandGroup();
        }
    }
    public class QrifyUserInterface : AddinUserInterface
    {
        public QrPropertyManagerPage PmpFactory { get; }
        public QrifyUserInterface(ISldWorks sldWorks)
        {

            PmpFactory = new QrPropertyManagerPage(sldWorks);
            CommandTabs = new List<AddinCommandTab>() { new QrTab() };
            var localappdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            IconsRootDir = new DirectoryInfo(Path.Combine(localappdata, "QrifyIcons"));
            this.PropertyManagerPages = new List<PropertyManagerPageX64> { PmpFactory };
        }
    }
    #endregion
}
