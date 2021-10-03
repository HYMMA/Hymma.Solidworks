using Hymma.SolidTools.Addins;
using SolidWorks.Interop.sldworks;

namespace Hymma.SolidTools.Fluent.Addins
{
    ///<inheritdoc/>
    public class AddinModelBuilder : AddinUserInterface, IAddinModelBuilder
    {
        private PmpUiModelFluent pmp;

        ///<inheritdoc/>
        public IFluentCommandTab AddCommandTab()
        {
            return new FluentCommandTab(this);
        }

        ///<inheritdoc/>
        public IPmpUiModelFluent AddPropertyManagerPage(string title, ISldWorks solidworks)
        {
            pmp = new PmpUiModelFluent(solidworks)
            {
                Title = title,
                AddinModel = this
            };
            return pmp;
        }

        /// <summary>
        /// build the user interface object
        /// </summary>
        /// <returns></returns>
        public AddinUserInterface Build() => this;
    }
}