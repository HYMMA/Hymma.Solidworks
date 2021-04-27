using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
