using SolidWorks.Interop.sldworks;

namespace Hymma.Solidworks.Addins
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
        T SolidworksObject { get; }
    }
}
