using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// an interface for classes that hide a native solidworks object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWrapSolidworksObject<T>
    {
        /// <summary>
        /// gets native solidworks object
        /// </summary>
        T SolidworksObject { get;  }
    }
}
