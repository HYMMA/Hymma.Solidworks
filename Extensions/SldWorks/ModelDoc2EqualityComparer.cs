// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using System.Collections.Generic;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// provides equality comparison for <see cref="ModelDoc2"/>
    /// </summary>
    public class ModelDoc2EqualityComparer : IEqualityComparer<ModelDoc2>
    {
        /// <summary>
        /// determine if two <see cref="ModelDoc2"/> are equal or not
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(ModelDoc2 x, ModelDoc2 y)
        {
            //Check whether the compared objects reference the same data.
            if (ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (x is null || y is null)
                return false;
            
            return (x.GetPathName() == y.GetPathName()) && (x.ConfigurationManager.ActiveConfiguration.Name == y.ConfigurationManager.ActiveConfiguration.Name);
        }

        /// <summary>
        /// return a hash code for this <see cref="ModelDoc2"/> object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(ModelDoc2 obj)
        {
            if (obj==null)
                return 0;
            
             return obj.GetPathName().GetHashCode() ^ obj.ConfigurationManager.ActiveConfiguration.Name.GetHashCode();
        }
    }
    
}
