using Hymma.Solidworks.Addins;
using SolidWorks.Interop.sldworks;
using System.Drawing;

namespace Hymma.Solidworks.Fluent.Addins
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

        /// <summary>
        /// Add a tab to this property manager page
        /// </summary>
        /// <param name="caption">caption for the property manager page tab </param>
        /// <param name="icon">an icon that will be placed on the left side of the caption</param>
        /// <returns>an object of <see cref="PmpTabFluent"/> </returns>
        public IPmpTabFluent CreatePropertyManagerPageTab(string caption, Bitmap icon = null)
        {
            return new PmpTabFluent(caption, icon);
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