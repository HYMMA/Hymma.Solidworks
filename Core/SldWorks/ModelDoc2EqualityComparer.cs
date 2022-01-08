using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// provides equality comparision for <see cref="ModelDoc2"/>
    /// </summary>
    public class ModelDoc2EqualityComparer : IEqualityComparer<ModelDoc2>
    {
        /// <summary>
        /// determin if two <see cref="ModelDoc2"/> are equla or not
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(ModelDoc2 x, ModelDoc2 y)
        {
            return (x.GetPathName() == y.GetPathName()) && (x.ConfigurationManager.ActiveConfiguration.Name == y.ConfigurationManager.ActiveConfiguration.Name);
        }

        /// <summary>
        /// return a hashcode for this <see cref="ModelDoc2"/> object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(ModelDoc2 obj)
        {
            var code = obj.GetPathName() + obj.ConfigurationManager.ActiveConfiguration.Name;
            return code.GetHashCode();
        }
    }
    
}
