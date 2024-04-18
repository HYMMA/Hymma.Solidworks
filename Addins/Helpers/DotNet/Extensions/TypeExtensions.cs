// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// provides extension methods for a <see cref="Type"/>
    /// </summary>
    internal static class TypeExtensions
    {
        //static Logger log = Logger.GetInstance(Properties.Resources.LogSource);
        /// <summary>
        /// returns the attribute in a type
        /// </summary>
        /// <typeparam name="T">the attribute required</typeparam>
        /// <param name="type"></param>
        /// <param name="searchAncesstors"></param>
        /// <returns></returns>
        internal static T TryGetAttribute<T>(this Type type, bool searchAncesstors = false) where T : Attribute
        {
            foreach (var attr in type.GetCustomAttributes(searchAncesstors))
            {
                if (attr is T)
                {
                    //log.Info($"found attribute {attr} in type {type}");
                    return attr as T;
                }
            }
            var e = new ArgumentNullException($"Could not find attribute in type {type}");
            //log.Error(e);
            throw e;
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
