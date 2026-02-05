// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins;
using Hymma.Solidworks.Addins.Helpers;
using Hymma.Solidworks.Addins.ContextMenus;
using Hymma.Solidworks.Extensions;
using QRCoder;
using QRify.Logging;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Environment = System.Environment;

namespace QRify
{
    internal static class Gdi32
    {
        [DllImport("gdi32.dll")]
        internal static extern bool DeleteObject(IntPtr hObject);

#if DEBUG
        [DllImport("user32.dll")]
        internal static extern int GetGuiResources(IntPtr hProcess, int uiFlags);
#endif
    }

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
        public Qrify()
        {
            base.OnStart += Qrify_OnStart;
            base.OnUiReady += Qrify_OnUiReady;
            base.OnExit += Qrify_OnExit;
        }

        private void Qrify_OnStart(object sender, OnConnectToSwEventArgs e)
        {
            this.SolidWorks = e.Solidworks;
        }

        private void Qrify_OnUiReady(object sender, OnConnectToSwEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("QRify: OnUiReady fired");
            RegisterContextMenus();
        }

        private void Qrify_OnExit(object sender, OnConnectToSwEventArgs e)
        {
            _contextMenus?.Dispose();
            _contextMenus = null;
        }

        private PropertyManagerPageBase pmpFactory;
        private ContextMenuRegistrar _contextMenus;

        public ISldWorks SolidWorks { get; private set; }

        public override AddinUserInterface GetUserInterFace()
        {
            var ui = new QrifyUserInterface(this.SolidWorks);
            pmpFactory = ui.PmpFactory;
            return ui;
        }

        private void RegisterContextMenus()
        {
            System.Diagnostics.Debug.WriteLine("QRify: RegisterContextMenus start");
            _contextMenus = ContextMenuRegistrar.Create(this);

            // Command group for graphics area selections (faces, edges, vertices)
            var graphicsGroup = new AddinCommandGroup
            {
                Title = "QRify Graphics",
                Commands = new[]
                {
                    new AddinCommand(
                        "Show Geometry Info",
                        "Show geometry type and details",
                        "Show geometry type and details",
                        Properties.Resources.info,  // info icon for graphics
                        nameof(ShowSelectionInfoFromContextMenu),
                        0,
                        (int)swCommandItemType_e.swMenuItem,
                        1,
                        nameof(EnableSelectionInfoFromContextMenu))
                }
            };

            // Register for faces, edges, vertices in graphics area
            _contextMenus.Register(graphicsGroup,
                ContextMenuTarget.Faces,
                ContextMenuTarget.Edges,
                ContextMenuTarget.Vertices);

            // Command group for feature tree selections
            var featureGroup = new AddinCommandGroup
            {
                Title = "QRify Features",
                Commands = new[]
                {
                    new AddinCommand(
                        "Show Feature Info",
                        "Show feature type and name",
                        "Show feature type and name",
                        Properties.Resources.qrify,  // qrify icon for features
                        nameof(ShowSelectionInfoFromContextMenu),
                        0,
                        (int)swCommandItemType_e.swMenuItem,
                        2,
                        nameof(EnableSelectionInfoFromContextMenu))
                }
            };

            // Register for features in the design tree
            _contextMenus.Register(featureGroup,
                ContextMenuTarget.Features,
                ContextMenuTarget.FeatureTreeItems,
                ContextMenuTarget.SketchProfiles);

            // Command group for sketch elements
            var sketchGroup = new AddinCommandGroup
            {
                Title = "QRify Sketch",
                Commands = new[]
                {
                    new AddinCommand(
                        "Show Sketch Info",
                        "Show sketch element details",
                        "Show sketch element details",
                        Properties.Resources.hymma,  // hymma icon for sketches
                        nameof(ShowSelectionInfoFromContextMenu),
                        0,
                        (int)swCommandItemType_e.swMenuItem,
                        3,
                        nameof(EnableSelectionInfoFromContextMenu))
                }
            };

            // Register for sketch segments and points
            _contextMenus.Register(sketchGroup,
                ContextMenuTarget.SketchSegments,
                ContextMenuTarget.SketchPoints);
        }

        private static string GetSelectionName(object selection)
        {
            switch (selection)
            {
                case Feature feature:
                    return $"\"{feature.Name}\"";
                case Sketch sketch:
                    return "\"Sketch\"";
                case SketchSegment sketchSegment:
                    return $"\"{sketchSegment.GetType()}\"";
                case SketchPoint sketchPoint:
                    return $"({sketchPoint.X:0.###}, {sketchPoint.Y:0.###}, {sketchPoint.Z:0.###})";
                default:
                    return string.Empty;
            }
        }

        public void ShowSelectionInfoFromContextMenu()
        {
            var modelDoc = SolidWorks?.ActiveDoc as ModelDoc2;
            var selectionMgr = modelDoc?.SelectionManager as SelectionMgr;
            if (selectionMgr == null)
                return;

            var type = (swSelectType_e)selectionMgr.GetSelectedObjectType3(1, -1);
            var selected = selectionMgr.GetSelectedObject6(1, -1);
            var name = GetSelectionName(selected);
            SolidWorks.SendMsgToUser($"Selection: {type} {name}".Trim());
        }

        public int EnableSelectionInfoFromContextMenu()
        {
            return SolidWorks?.ActiveDoc != null ? 1 : 0;
        }


        //you can move this region to Qrify.g.cs
        #region Call back functions
        /// <summary>
        /// This is a call back function from <see cref="QrCommand"/>
        /// </summary>
        /// <returns></returns>
        public object EnablePropertyManagerPage()
        {
            if (SolidWorks.ActiveDoc == null || SolidWorks.CommandInProgress)
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
            if (SolidWorks.ActiveDoc is DrawingDoc drawing)
            {
                pmpFactory.Show();
            }
        }

        /// <summary>
        /// Enables the preview extraction command for part and assembly documents that have been saved.
        /// </summary>
        public int EnablePreviewExtraction()
        {
            // Diagnostic: always enable so we can verify the callback is invoked.
            return SolidWorks.CommandInProgress ? 0 : 1;
        }

        /// <summary>
        /// Enables the rendered preview extraction command for part and assembly documents.
        /// </summary>
        public int EnableRenderedPreviewExtraction()
        {
            return SolidWorks.CommandInProgress ? 0 : 1;
        }

        /// <summary>
        /// Extracts the preview bitmap for the active part/assembly and saves it to LocalAppData.
        /// </summary>
        public void ExtractActiveModelPreviewToClipboard()
        {
#if DEBUG
            var diagEnabled = string.Equals(Environment.GetEnvironmentVariable("QRIFY_PREVIEW_DIAG"), "1", StringComparison.Ordinal);
            var gdiBefore = diagEnabled ? GetGdiObjectCount() : 0;
            var handlesBefore = diagEnabled ? Process.GetCurrentProcess().HandleCount : 0;
#endif
            SolidWorks.SendMsgToUser("QRify: preview command invoked.");
            var modelDoc = SolidWorks.ActiveDoc as ModelDoc2;
            if (modelDoc == null)
                return;

            var isPart = SolidWorks.ActiveDoc is PartDoc;
            var isAssembly = SolidWorks.ActiveDoc is AssemblyDoc;
            if (!isPart && !isAssembly)
            {
                SolidWorks.SendMsgToUser("Preview extraction only works for part and assembly documents.");
                return;
            }

            var path = modelDoc.GetPathName();
            if (string.IsNullOrWhiteSpace(path))
            {
                SolidWorks.SendMsgToUser("Please save the document before extracting its preview.");
                return;
            }

            var configurationName = modelDoc.ConfigurationManager?.ActiveConfiguration?.Name ?? string.Empty;

            try
            {
                using (var previewBitmap = SolidWorks.ExtractPreviewImage(path, configurationName))
                {
                    var savedPath = SavePreviewToLocalAppData(path, configurationName, previewBitmap);
                    SolidWorks.SendMsgToUser($"Preview saved to: {savedPath}");
                }
            }
            catch (Exception ex)
            {
                SolidWorks.SendMsgToUser($"Could not extract preview: {ex.Message}");
            }
#if DEBUG
            if (diagEnabled)
            {
                var gdiAfter = GetGdiObjectCount();
                var handlesAfter = Process.GetCurrentProcess().HandleCount;
                SolidWorks.SendMsgToUser($"QRify diag: GDI {gdiBefore}->{gdiAfter}, Handles {handlesBefore}->{handlesAfter}");
            }
#endif
        }

        /// <summary>
        /// Renders the active view and saves it to LocalAppData with real model colors.
        /// </summary>
        public void ExtractActiveModelRenderedPreviewToClipboard()
        {
            SolidWorks.SendMsgToUser("QRify: rendered preview command invoked.");
            var modelDoc = SolidWorks.ActiveDoc as ModelDoc2;
            if (modelDoc == null)
                return;

            var isPart = SolidWorks.ActiveDoc is PartDoc;
            var isAssembly = SolidWorks.ActiveDoc is AssemblyDoc;
            if (!isPart && !isAssembly)
            {
                SolidWorks.SendMsgToUser("Rendered preview only works for part and assembly documents.");
                return;
            }

            var path = modelDoc.GetPathName();
            if (string.IsNullOrWhiteSpace(path))
            {
                SolidWorks.SendMsgToUser("Please save the document before extracting its rendered preview.");
                return;
            }

            var configurationName = modelDoc.ConfigurationManager?.ActiveConfiguration?.Name ?? string.Empty;

            try
            {
                using (var renderedBitmap = modelDoc.RenderActiveViewToBitmap())
                {
                    var savedPath = SavePreviewToLocalAppData(path, configurationName, renderedBitmap);
                    SolidWorks.SendMsgToUser($"Rendered preview saved to: {savedPath}");
                }
            }
            catch (Exception ex)
            {
                SolidWorks.SendMsgToUser($"Could not render preview: {ex.Message}");
            }
        }

        private static string SavePreviewToLocalAppData(string modelPath, string configurationName, Bitmap previewBitmap)
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var previewsDir = Path.Combine(localAppData, "QrifyPreviews");
            Directory.CreateDirectory(previewsDir);

            var modelName = Path.GetFileNameWithoutExtension(modelPath);
            var safeConfig = string.IsNullOrWhiteSpace(configurationName)
                ? "default"
                : MakeSafeFileName(configurationName);

            var fileName = $"{modelName}_{safeConfig}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var fullPath = Path.Combine(previewsDir, fileName);

            previewBitmap.Save(fullPath, ImageFormat.Png);
            return fullPath;
        }

        private static string MakeSafeFileName(string value)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var chars = value.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (Array.IndexOf(invalidChars, chars[i]) >= 0)
                    chars[i] = '_';
            }

            return new string(chars);
        }

#if DEBUG
        private static int GetGdiObjectCount()
        {
            using (var process = Process.GetCurrentProcess())
            {
                return Gdi32.GetGuiResources(process.Handle, 0);
            }
        }
#endif
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
                var hBitmap = qrImage.GetHbitmap();
                try
                {
                    var src = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    src.Freeze();
                    Clipboard.SetImage(src);
                }
                finally
                {
                    Gdi32.DeleteObject(hBitmap);
                }
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

        private void QrPropertyManagerPage_Closing(object sender,PmpCloseEventArgs e)
        {
            if (e.Reason == PmpCloseReason.Cancel)
            {
                closeReason = e.Reason;
                Clipboard.Clear();
            }
        }

        private void QrPropertyManagerPage_AfterClose(object sender,PmpCloseEventArgs e)
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
            this.UserId = 1;

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
            this.UserId = 2;

            this.EnableMethod = "EnablePropertyMangagerPage";
            this.CallBackFunction = "ShowQrifyPropertyManagerPage";

            ToolTip = "Help for Qrify";
            HintString = "How to get help!";
            Name = "Qrify Helps";
        }
    }
    public class QrCommandExtractPreview : AddinCommand
    {
        public QrCommandExtractPreview()
        {
            CommandTabTextType = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow;
            IconBitmap = Properties.Resources.info;
            UserId = 3;

            EnableMethod = nameof(Qrify.EnablePreviewExtraction);
            CallBackFunction = nameof(Qrify.ExtractActiveModelPreviewToClipboard);

            Name = "Copy Preview";
            HintString = "Copy the active part/assembly preview to the clipboard";
            ToolTip = Name;
        }
    }
    public class QrCommandExtractRenderedPreview : AddinCommand
    {
        public QrCommandExtractRenderedPreview()
        {
            CommandTabTextType = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow;
            IconBitmap = Properties.Resources.info;
            UserId = 4;

            EnableMethod = nameof(Qrify.EnableRenderedPreviewExtraction);
            CallBackFunction = nameof(Qrify.ExtractActiveModelRenderedPreviewToClipboard);

            Name = "Copy Rendered Preview";
            HintString = "Render the active view and save it to LocalAppData";
            ToolTip = Name;
        }
    }
    public class QrCommandGroup : AddinCommandGroup
    {
        public QrCommandGroup() : base(2,
                                       new List<AddinCommand>() { new QrCommand(), new QrCommandHelp(), new QrCommandExtractPreview(), new QrCommandExtractRenderedPreview() },
                                       "Title for Qrify command group",
                                       "Description for Qrify",
                                       "make QR codes",
                                       "Tooltip",
                                       Properties.Resources.qrify)
        {
            // Force SOLIDWORKS to rebuild the tab/commands instead of keeping cached ones.
            IgnorePrevious = true;
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
            DocTypes = new[] { swDocumentTypes_e.swDocDRAWING, swDocumentTypes_e.swDocPART, swDocumentTypes_e.swDocASSEMBLY };
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
