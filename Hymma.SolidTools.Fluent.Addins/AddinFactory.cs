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
        /// Access main object to make a property manager page 
        /// </summary>
        /// <returns></returns>
        public IAddinModelBuilder GetPmpBuilder()
        {
            return new AddinModelBuilder();
        }
    }

    public class clientCode
    {
        #region clinet code
        public clientCode(ISldWorks solidworks)
        {
          var  _addin = new AddinFactory().GetPmpBuilder()
                .AddPropertyManagerPage("title of the pmp UI",solidworks)
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
            /*.AddGroup()
                .That()
                .IsClickable()
                .And()
                .IsExpanded()
                .WhenExpanded(Action<bool>)
                    .AddControl(swPropertyManagerPageControlType_e.swControlType_Selectionbox)
                    .withHeight
            .Apply()
        .OnHelp(Action<bool>)


                .With()//returns new PropertyManagerBuilderX64:IPmpBuilder
                .Controls(list of IPMpControls)*/
        }
        #endregion
    }
}
