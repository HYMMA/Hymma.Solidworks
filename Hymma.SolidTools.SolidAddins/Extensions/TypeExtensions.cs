using System;
using System.IO;
using System.Reflection;

namespace Hymma.SolidTools.SolidAddins
{
    public static class TypeExtensions
    {
        /// <summary>
        /// returns the attribute in a type
        /// </summary>
        /// <typeparam name="A">the attriubte required</typeparam>
        /// <param name="searchChildren"></param>
        /// <returns></returns>
        public static Attribute TryGetAttribute<A>(this Type type,bool searchChildren) where A:Attribute
        {
            foreach (Attribute attr in type.GetCustomAttributes(searchChildren))
            {
                if (attr is A)
                    return attr;
            }
            return null;
        }

        /// <summary>
        /// get assembly locaiton
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string GetAssemblyDir(this object self)
        {
            Assembly assembly = self.GetType().Assembly;
            return assembly.Location;
        }

        public static T CastTo<T>(this object self)
        {
            return (T)self;
        }
    }
}
