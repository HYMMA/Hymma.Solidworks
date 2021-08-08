using Hymma.SolidTools.Addins;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// Creates Multiple object required to make a solidworks addin using fluent pattern
    /// </summary>
    public class AddinFactory : IFluent
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <remarks> a factory to make addin ui
        /// </remarks>
        public AddinFactory()
        {
            addinModelBuilder = new AddinModelBuilder();
        }

        private IAddinModelBuilder addinModelBuilder;
        /// <summary>
        /// Access main object to make a an addin ui 
        /// </summary>
        /// <returns></returns>
        public IAddinModelBuilder GetUiBuilder()
        {
            if (addinModelBuilder == null)
                return new AddinModelBuilder();
            return addinModelBuilder;
        }
    }

    internal class clientCode
    {
        #region clinet code samples
        public clientCode(ISldWorks solidworks)
        {
            var builder = new AddinFactory().GetUiBuilder();
            var addinUiModel = builder
                  .AddPropertyManagerPage("title of the pmp UI", solidworks)
                  .AfterClose(() => { })
                  .WhileClosing(PMPCloseReason =>
                  {
                      if (PMPCloseReason == PMPCloseReason.UnknownReason)
                      {
                          throw new Exception();
                      }
                  })
                  .AddGroup("Caption")
                      .That()
                      .IsExpanded()
                      .And()
                      .HasTheseControls(() =>
                      {
                          var controls = new List<IPmpControl>();
                          controls.Add(new PmpCheckBox("checkbox"));
                          return controls;
                      })
                      .AndOnExpansionChange(state =>
                      {
                          if (!state)
                              throw new ArgumentException("group was un-checked");
                      })
                      .SaveGroup()
                  .SavePropertyManagerPage(out PropertyManagerPageX64 propertyManagerPageX64);

            var tab = builder.AddCommandTab()
                    .WithTitle("title of command tab")
                    .That()
                    .IsVisibleIn(new[] { swDocumentTypes_e.swDocASSEMBLY, swDocumentTypes_e.swDocDRAWING, swDocumentTypes_e.swDocIMPORTED_PART })
                    .AddCommandGroup(1)
                        .Has()
                        .Commands(() =>
                        {
                            var c1 = new AddinCommand();
                            var c2 = new AddinCommand("c2", "hint for c2", "tooltip for c2", new System.Drawing.Bitmap(128, 128), "callBackFunction");
                            return new[] { c1, c2 };
                        })
                    .WithHint("hint for group")
                    .WithDescription("description")
                    .SaveCommnadGroup();
        }
        #endregion
    }
}
