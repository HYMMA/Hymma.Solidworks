// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

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
            //Check whether the compared objects reference the same data.
            if (ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (x is null || y is null)
                return false;

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
            if (obj==null)
                return 0;
            
            return obj.GetPathName().GetHashCode() ^
                        obj.ReferencedConfiguration.GetHashCode() ^
                        obj.IsEnvelope().ToString().GetHashCode() ^
                        obj.IsSuppressed().ToString().GetHashCode() ^
                        obj.ExcludeFromBOM.ToString().GetHashCode();
        }
    }
}
