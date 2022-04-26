using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// provides equality comparer for <see cref="Component2"/> objects
    /// </summary>
    public class ComponentEqualityComparer : IEqualityComparer<Component2>
    {
        /// <summary>
        /// determine if two <see cref="Component2"/> are equal or not
        /// </summary>
        /// <param name="x">first component</param>
        /// <param name="y">second component</param>
        /// <returns></returns>
        public bool Equals(Component2 x, Component2 y)
        {
            return (x.GetPathName() == y.GetPathName()
                    && (x.ReferencedConfiguration == y.ReferencedConfiguration)
                    && (x.IsSuppressed() == y.IsSuppressed())
                    && (x.IsEnvelope() == y.IsEnvelope())
                    && (x.ExcludeFromBOM == y.ExcludeFromBOM));
        }

        /// <summary>
        /// returns the hash code for this object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(Component2 obj)
        {
            var code = obj.GetPathName() + obj.ReferencedConfiguration + obj.IsEnvelope().ToString() + obj.IsSuppressed().ToString() + obj.ExcludeFromBOM.ToString();
            return code.GetHashCode();
        }
    }
}
