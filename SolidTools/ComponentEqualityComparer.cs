using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace Hymma.SolidTools
{
    public class ComponentEqualityComparer : IEqualityComparer<Component2>
    {
        public bool Equals(Component2 x, Component2 y)
        {
            return (x.GetPathName() == y.GetPathName()
                    && (x.ReferencedConfiguration == y.ReferencedConfiguration)
                    && (x.IsSuppressed() == y.IsSuppressed())
                    && (x.IsEnvelope() == y.IsEnvelope())
                    && (x.ExcludeFromBOM == y.ExcludeFromBOM));
        }

        public int GetHashCode(Component2 obj)
        {
            var code = obj.GetPathName() + obj.ReferencedConfiguration + obj.IsEnvelope().ToString() + obj.IsSuppressed().ToString() + obj.ExcludeFromBOM.ToString();
            return code.GetHashCode();
        }
    }
}
