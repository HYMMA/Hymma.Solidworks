// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System.Runtime.InteropServices;

namespace Hymma.Solidworks.Addins
{
    /// <summary>
    /// an interface for classes that hide a native solidworks object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWrapSolidworksObject<T>:IReleaseSolidworksObject
    {
        /// <summary>
        /// gets native solidworks object
        /// </summary>
        T SolidworksObject { get; }

    }
    /// <summary>
    /// an interface for classes that can release solidworks objects
    /// </summary>
    public interface IReleaseSolidworksObject
    {
        /// <summary>
        /// calls <see cref="Marshal.ReleaseComObject"/> on the solidworks object
        /// </summary>
        void ReleaseSolidworksObject();
    }
}
