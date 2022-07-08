using Hymma.Solidworks.Addins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hymma.Solidworks.Addins.Fluent;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
namespace QrifyPlus
{
    [Addin("QRify+",AddinIcon ="qrify",Description ="Generate QR Code and more",LoadAtStartup =true)]
    public class QrifyPlus : AddinMaker
    {
        public override AddinUserInterface GetUserInterFace()
        {
            var builder = new AddinModelBuilder();
            builder
                .AddCommandTab()
                    .WithTitle("QRify+")
                    .That()
                    .IsVisibleIn(new[] {swDocumentTypes_e.swDocDRAWING })
                .SaveCommandTab()
                    .AddPropertyManagerPage("QRify",this.Solidworks)
                        .AddTab("Generate QR",)
                        .AddGroup("Generate QR")
                        .
                        .
        }
    }
}
