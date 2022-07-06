// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.Reflection;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// provides extension methods for a <see cref="Type"/>
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// returns the attribute in a type
        /// </summary>
        /// <typeparam name="T">the attribute required</typeparam>
        /// <param name="type"></param>
        /// <param name="searchChildren"></param>
        /// <returns></returns>
        internal static T TryGetAttribute<T>(this Type type, bool searchChildren = false) where T : Attribute
        {
            foreach (var attr in type.GetCustomAttributes(searchChildren))
            {
                if (attr is T)
                    return attr as T;
            }
            return null;
        }

        /// <summary>
        /// cast <see cref="object"/> to <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        internal static T CastTo<T>(this object self)
        {
            return (T)self;
        }
    }
}
