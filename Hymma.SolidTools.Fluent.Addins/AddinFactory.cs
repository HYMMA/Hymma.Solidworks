using Hymma.SolidTools.Addins;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class clientCode
    {
        #region clinet code
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
                          controls.Add(new PmpCheckBox(true));
                          return controls;
                      })
                      .AndOnExpansionChange(state =>
                      {
                          if (!state)
                              throw new ArgumentException("group was un-checked");
                      })
                      .SaveGroup()
                  .SavePropertyManagerPage(out PropertyManagerPageX64 propertyManagerPageX64);

            var tab = builder.AddCommandTab().
        }
        #endregion
    }
}
