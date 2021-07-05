using Hymma.SolidTools.Addins;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTools.Fluent.Addins
{
    public class AddinPmpFactory : IFluent
    {

        public PmpUi AddPropertyManagerPage(string title)
        {
            var pmp = new PmpUi();
            pmp.Title = title;
            return pmp;
        }

        public PmpUi OnHelp()
        {
            throw new NotImplementedException();
        }

    }
    public class clientCode
    {
        PmpUi _pmp;
        #region clinet code
        public clientCode()
        {

            _pmp = new AddinModel()
                .AddPropertyManagerPage("title of the pmp UI")
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
                    .
                                                        
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
