using Hymma.SolidTools.Addins;

namespace Hymma.SolidTools.Fluent.Addins
{
    public interface IAddinModelBuilder : IFluent
    {
        AddinModel BuildPropertyManagerPage(out PropertyManagerPageX64 pmp);
    }
}