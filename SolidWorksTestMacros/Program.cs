using Hymma.SolidTools.Core;

namespace SolidWorksTestMacros.csproj
{
    class Program
    {
        static void Main(string[] args)
        {
            var swApp = SolidWorksManager.GetSolidworks();
            var macro = new SolidWorksMacro(swApp);
        }
    }
}
