using Hymma.SolidTools.Addins;
using SolidWorks.Interop.sldworks;

namespace Hymma.SolidTools.Fluent.Addins
{
    ///<inheritdoc/>
    public class AddinModelBuilder : AddinUserInterface, IAddinModelBuilder
    {
        private PmpUi pmp;

        ///<inheritdoc/>
        public IFluentCommandTab AddCommandTab()
        {
            return new FluentCommandTab(this);
        }

        ///<inheritdoc/>
        public IPmpUi AddPropertyManagerPage(string title, ISldWorks solidworks)
        {
            pmp = new PmpUi(solidworks)
            {
                Title = title,
                AddinModel = this
            };
            return pmp;
        }
    }
}