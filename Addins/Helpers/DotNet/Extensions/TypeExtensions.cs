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
        /// <typeparam name="A">the attribute required</typeparam>
        /// <param name="type"></param>
        /// <param name="searchChildren"></param>
        /// <returns></returns>
        internal static Attribute TryGetAttribute<A>(this Type type,bool searchChildren) where A:Attribute
        {
            foreach (Attribute attr in type.GetCustomAttributes(searchChildren))
            {
                if (attr is A)
                    return attr;
            }
            return null;
        }

        /// <summary>
        /// get assembly location
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        internal static string GetAssemblyDir(this object self)
        {
            Assembly assembly = self.GetType().Assembly;
            return assembly.Location;
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
