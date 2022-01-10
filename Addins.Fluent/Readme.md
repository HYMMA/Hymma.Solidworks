# Overview
Making native-looking SOLIDWORKS Addins is time consuming and has a steep learning curve. This package was originally made for internal use in HYMMA and now is available to the public. It allows creating a complex UI in a [property manager page](http://help.solidworks.com/2020/english/SolidWorks/sldworks/r_pm_overview.htm) in SOLIDWORKS with fluent API.

# Key Features
- Fluent API 
- Supports Bitmaps
- No need to create [sprites](https://en.wikipedia.org/wiki/Sprite_(computer_graphics)) for Commands 
- Each of [Property manager page controllers](http://help.solidworks.com/2017/english/api/sldworksapiprogguide/Overview/Using_PropertyManagerPage2_and_the_Related_Objects.htm?id=fd93f031fb6c4a9c935310a569d9ce45#Pg0) have separate class that supports events so you don't need to worry about [IPropertyManagerPage2Handler9](https://help.solidworks.com/2018/English/api/swpublishedapi/SolidWorks.Interop.swpublished~SolidWorks.Interop.swpublished.IPropertyManagerPage2Handler9.html).  

# How to start
Since this package is built on top of [Hymma.Solidworks.Addins](https://www.nuget.org/packages/Hymma.Solidworks.Addins/) you need to inherit from ```AddinMaker.cs``` and override the ```GetUserInterFace()``` as indicated below.

```Csharp
using Hymma.Solidworks.Addins;
using Hymma.Solidworks.Addins.Fluent;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Hymma.Addin.SolidWorks
{
    [Guid("XXXXXX-XXX-XXX-XXX-XXXXXx")]
    [ComVisible(true)]
    [Addin(Title = "HYMMA", Description = "Addins by HYMMA", LoadAtStartup = true, AddinIcon = "NameOfBitmapRecourseFile")]
    public class HymmaAddin : AddinMaker
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public HymmaAddin() : base(typeof(HymmaAddin))
        {

        }
        private PmpFactoryX64 _pmpForExportDxfAssemblies;
        private PmpFactoryX64 _pmpForExportDxfParts;
        private PmpFactoryX64 _pmpForExportDxfDrawings;

           /// <summary>
        /// get the user interface for this addin
        /// </summary>
        /// <returns></returns>
        public override AddinUserInterface GetUserInterFace()
        {
            var uiBuilder = new AddinFactory().GetUiBuilder();

            #region CommandTab
            uiBuilder.AddCommandTab()
                    .WithTitle("HYMMA")
                    .That()
                    .IsVisibleIn(new[] { swDocumentTypes_e.swDocASSEMBLY, swDocumentTypes_e.swDocDRAWING, swDocumentTypes_e.swDocPART })
                        .SetCommandGroup(16)
                            .WithTitle("HYMMA")
                            .WithIcon(Properties.Resources.folded)
                            .WithDescription("Command Description")
                            .WithHint("Addins by HYMMA").WithToolTip("HYMMA")
                            .Has()
                            .Commands(() =>
                            {
                                return new List<AddinCommand>()
                                {
                                    new AddinCommand("name", "hint", "tootipTitle", Properties.Resources.CommandIcon,
                                    nameof(ShowPmp),
                                    enableMethode:nameof(PmpEnabler)),
                                };
                            })
                        .SaveCommandGroup()
                    .SaveCommandTab();
            #endregion

            #region property manager page in part documents
            uiBuilder.AddPropertyManagerPage("title", Solidworks)
                .WhileClosing((r) => Properties.YourAddin.Default.Save())
                .AddTab("tab title in PMP", Properties.Resources.YourAddin)
                    .AddGroup("command group title")
                        .IsExpanded()
                        .HasTheseControls(IEnumerable<IPmpControl>)
                    .SaveGroup()
                    .AddGroup("Another group title")
                        .IsExpanded(false)
                        .HasTheseControls(IEnumerable<IPmpControl>)
                    .SaveGroup()
                 .SaveTab()

                    //this allows reusing the same tab in different documents
                 .AddTab<InhertiedFromPmpTab>()
                 .AddTab<InhertiedFromPmpTab>()
                .SavePropertyManagerPage(out PmpFactoryX64 pmpFactoryParts);
            _pmpForExportDxfParts = pmpFactoryParts;
            #endregion

            #region property manager page in Assemblies
            uiBuilder.AddPropertyManagerPage("title", Solidworks)
                        .WhileClosing((r) => Properties.YourAddin.Default.Save())
                    .AddTab("tab title in pmp", Properties.Resources.YourAddin)
                        .AddGroup("Select Components")
                            .IsExpanded()
                            .HasTheseControls(IEnumerable<IPmpControl>)
                        .SaveGroup()
                    .SaveTab()

                    //this allows reusing the same tab in different documents
                 .AddTab<InhertiedFromPmpTab>()
                .SavePropertyManagerPage(out PmpFactoryX64 pmpFactoryAssy);
            this._pmpForExportDxfAssemblies = pmpFactoryAssy;
            #endregion

            #region Export dxf pmp drawings
            uiBuilder.AddPropertyManagerPage("title", Solidworks)
                    .WhileClosing((r) => Properties.YourAddin.Default.Save())
                        .AddTab("tab title", Properties.Resources.YourAddin)
                            .AddGroup("group in the tab")
                                .HasTheseControls(IEnumerable<IPmpControl>)
                            .SaveGroup()
                            .AddGroup("Bill of Materials (BOMs)")
                                .HasTheseControls(IEnumerable<IPmpControl>)
                            .SaveGroup()
                        .SaveTab()
                        //this allows reusing the same tab in different documents
                 .AddTab<InhertiedFromPmpTab>()
                .SavePropertyManagerPage(out PmpFactoryX64 pmpFactoryDrawing);
            _pmpForExportDxfDrawings = pmpFactoryDrawing;
            #endregion

            return uiBuilder.Build();
        }

        /// <summary>
        /// determines when a property manager page is enabled
        /// </summary>
        /// <returns></returns>
        public int PmpEnabler()
        {
            if (Solidworks.ActiveDoc == null)
                return 0;

            //activate the commands only if no other property manager page is running
            Solidworks.GetRunningCommandInfo(out int commandId, out string pmpTitle, out bool isUiActive);
            if (string.IsNullOrEmpty(pmpTitle))
                return 1;
            return 0;
        }

        /// <summary>
        /// determines which document should be shown with which property manager page
        /// </summary>
        public void ShowPmp()
        {
            var model = Solidworks.ActiveDoc as ModelDoc2;
            switch (model.GetType())
            {
                case ((int)swDocumentTypes_e.swDocASSEMBLY): _pmpForExportDxfAssemblies.Show(); break;
                case ((int)swDocumentTypes_e.swDocPART): _pmpForExportDxfParts.Show(); break;
                case ((int)swDocumentTypes_e.swDocDRAWING): _pmpForExportDxfDrawings.Show(); break;
                default:
                    break;
            }
        }
    }
}
```
## How to download
This package is signed for COM interop and is [published to Nuget](https://www.nuget.org/packages/Hymma.Solidworks.Addins.Fluent/).
```
Install-Package Hymma.Solidworks.Addins.Fluent -Version 18.0.0
```
## Versioning
The first two digits of the version indicates version of SOLIDWORKS this package is compatible with. For example, all version 18.x.x are compatible with SOLIDWORKS 2018 and above.

