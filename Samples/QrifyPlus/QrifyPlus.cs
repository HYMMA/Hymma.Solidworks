// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins;
using Hymma.Solidworks.Addins.Fluent;
using Microsoft.Win32;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Environment = System.Environment;

namespace QrifyPlus
{
    [Addin("QRifyPlus", AddinIcon = "qrifyPlus", Description = "Generate QR Code and more", LoadAtStartup = true)]
    [Guid("C69637E8-D32E-4C73-A3F6-5DB5DD70E0EF")]
    [ComVisible(true)]
    public class QrifyPlus : AddinMaker
    {
        private PropertyManagerPageX64 pmpFactory;
        private QrifyPlusPmpCallBacks closeCallBackRegistry;

        public QrifyPlus()
        {
            base.OnStart += QrifyPlus_OnStart;
            base.OnExit += QrifyPlus_OnExit;
        }

        private void QrifyPlus_OnExit(object sender, OnConnectToSwEventArgs e)
        {
            // do you magic when addin gets un-loaded
        }

        private void QrifyPlus_OnStart(object sender, OnConnectToSwEventArgs e)
        {
            // do your magic once addin gets loaded

            //this is the proper way to access the solidworks object. prior to this moment Solidworks object is null as add-in is not connected to solidworks yet
            closeCallBackRegistry = new QrifyPlusPmpCallBacks(e.Solidworks);
        }
        public void ShowQrifyPlusPmp()
        {
            var model = Solidworks.ActiveDoc as ModelDoc2;
            model.ClearSelection2(true);
            switch (model.GetType())
            {
                case ((int)swDocumentTypes_e.swDocDRAWING): pmpFactory.Show(); break;
                default:
                    break;
            }
        }
        public int QrifyPlusPmpEnabler()
        {
            if (Solidworks.ActiveDoc == null)
                return 0;

            //activate the commands only if no other property manager page is running
            Solidworks.GetRunningCommandInfo(out int commandId, out string pmpTitle, out bool isUiActive);
            if (string.IsNullOrEmpty(pmpTitle))
                return 1;
            return 0;
        }

        public override AddinUserInterface GetUserInterFace()
        {
            var iconsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"QrifyPlusIcons");
            var builder = this.GetBuilder();
            builder
                .AddCommandTab()                                                //An Addin must have a command tab that hosts the command group which in turn hosts the commands
                    .WithTitle("QRify+")
                    .That()
                    .IsVisibleIn(new[] { swDocumentTypes_e.swDocDRAWING })      //Define which document types this command tab should be accessible from
                    .SetCommandGroup(5)                                         //Add a command group with ID 1. or else if you want. this id and the GUID of this add-in should be unique. once you updated your addin you should change this ID to hold backward compatibility
                        .WithTitle("Qrify+",Constants.SolidworksMenu.View)      //Define a title for command group and place the group under Edit menu in solidworks. for most Ui elements solidworks will not load the ui if they don't have a title
                        .WithIcon(Properties.Resources.qrifyPlus)               //An Icon for the command group
                        .Has()                                                  //change context
                            .Commands(() =>                                     //Add commands to the command group
                            {
                                return new AddinCommand[]
                                {
                                    new AddinCommand("QRify+", "QRify+", "QRify+", Properties.Resources.knight, nameof(ShowQrifyPlusPmp), enableMethod:nameof(QrifyPlusPmpEnabler)),
                                        new AddinCommand("Qrify+ helps","help for qrify+","help toolTip",Properties.Resources.infoPlus,nameof(ShowQrifyPlusHelpDia),enableMethod:nameof(QrifyPlusPmpEnabler)),
                                        new AddinCommand("Qrify+ info","info for qrify+","info toolTip",Properties.Resources.simpson,nameof(ShowQrifyPlusHelpDia),enableMethod:nameof(QrifyPlusPmpEnabler)),
                                        new AddinCommand("box","info","info toolTip",Properties.Resources.box,nameof(ShowQrifyPlusHelpDia),enableMethod:nameof(QrifyPlusPmpEnabler)){BoxId=2},
                                };
                            })
                    .SaveCommandGroup()                                         //Save the command group 
                .SaveCommandTab()                                               //Save the command tab to the builder
                .AddCommandTab()
                    .WithTitle("Qrify+ help")
                    .IsVisibleIn(new[] { swDocumentTypes_e.swDocDRAWING })
                    .SetCommandGroup(7)
                        .WithTitle("Help for Q+", Constants.SolidworksMenu.Help)
                        .WithIcon(Properties.Resources.infoPlus)
                            .Has()
                                .Commands(() =>
                                {
                                    return new AddinCommand[]
                                    {
                                        new AddinCommand("Qrify+ help","help for qrify+","help toolTip",Properties.Resources.infoPlus,nameof(ShowQrifyPlusHelpDia),enableMethod:nameof(QrifyPlusPmpEnabler)),
                                    };
                                })
                    .SaveCommandGroup()
                .SaveCommandTab()
                .AddPropertyManagerPage("QRify+", this.Solidworks)          //Add property manager page to the list of UI that the builder will create
                .AddMenuePopUpItem(new PopUpMenuItem("item 1", "hint", swDocumentTypes_e.swDocDRAWING))
                        .AddGroup("Group Under Property Manager Page")
                            .That()
                            .HasTheseControls(new List<IPmpControl>
                            {
                                new PmpSelectionBox(new[] { swSelectType_e.swSelDRAWINGVIEWS })
                                {
                                    PopUpMenuItems = new List<PopUpMenuItem>()
                                    {
                                       new PopUpMenuItem("popUp menu item", "hint for the pop up menu item", swDocumentTypes_e.swDocDRAWING) ,
                                       new PopUpMenuItem("another item", "hint for this item", swDocumentTypes_e.swDocDRAWING) ,
                                    }
                                }
                            })
                            .SetExpansion(true)
                            .Color(SysColor.ActiveSelectionListBox)
                        .SaveGroup()
                    .AddTab<QrPlusTab>()                                    //Best practice to add tabs with complex Ui setup
                    .AddTab("Settings", Properties.Resources.infoPlus)      //A Property manager page can or cannot have a tab that host the groups. A group hosts the controls such as text box and selection box and ...
                        .AddGroup(caption: "Settings Controls")             //Add a group to property manager page Tab
                            .That()                                         //Just showing off
                            .HasTheseControls(GetSettingsControls)          //add Controls to the property manager page group
                            .SetExpansion(true)                             //Expanded or not expanded on display
                            .OnExpansionChange(null)                        //Event to fire once group expansion changes
                        .SaveGroup()                                        //Save the property manager page group
                    .SaveTab()                                              //Save the property manger page tab
               .OnClosing((r) => closeCallBackRegistry.DuringClose(r))      //Solidworks exposes this API but actually locks the UI and most of the command will have no effect. THIS IS IMPORTANT
               .OnAfterClose(() => closeCallBackRegistry.AfterClose())      //Once the Property Manager Page is closed for good
               .SavePropertyManagerPage(out PropertyManagerPageX64 pmpFactoryX64);  //expose the object that is responsible for showing th property manager page 
            this.pmpFactory = pmpFactoryX64;
            return builder.WithIconsPath(new DirectoryInfo(iconsPath)).Build();                                         //Build the UI
        }

        private object ShowQrifyPlusHelpDia()
        {
            throw new NotImplementedException();
        }

        private List<IPmpControl> GetSettingsControls()
        {
            var controls = new List<IPmpControl>
            {
                new PmpLabel("You can add more controls to this group. Go to https://github.com/HYMMA/Hymma.Solidworks/tree/dev/Addins.Fluent to learn more")
                {
                    TextColor = Color.Black,
                    BackGroundColor = Color.Yellow
                }
            };
            return controls;
        }
    }
}
