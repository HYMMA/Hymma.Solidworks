using Hymma.SolidTools.Addins;
using SolidWorks.Interop.sldworks;

namespace Hymma.SolidTools.Fluent.Addins
{
    ///<inheritdoc/>
    public class AddinModelBuilder : AddinModel, IAddinModelBuilder
    {
        ///<inheritdoc/>
        public IPmpUi AddPropertyManagerPage(string title, ISldWorks solidworks)
        {
            var pmp = new PmpUi(solidworks)
            {
                Title = title,
                AddinModel = this
            };
            return pmp;
        }

        ///<inheritdoc/>
        public IAddinModelBuilder BuildPropertyManagerPage(out PropertyManagerPageX64 pmp)
        {
            throw new System.NotImplementedException();
        }
    }
}