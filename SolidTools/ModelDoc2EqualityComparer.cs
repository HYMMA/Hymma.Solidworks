using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace Hymma.SolidTools
{
    public class ModelDoc2EqualityComparer : IEqualityComparer<ModelDoc2>
    {
        public bool Equals(ModelDoc2 x, ModelDoc2 y)
        {
            return (x.GetPathName() == y.GetPathName()) && (x.ConfigurationManager.ActiveConfiguration.Name == y.ConfigurationManager.ActiveConfiguration.Name);
        }

        public int GetHashCode(ModelDoc2 obj)
        {
            var code = obj.GetPathName() + obj.ConfigurationManager.ActiveConfiguration.Name;
            return code.GetHashCode();
        }
    }
    
}
