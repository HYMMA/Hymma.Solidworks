// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// extensions for <see cref="MathUtility"/>
    /// </summary>
    public static class MathUtilitiesExtensions
    {
        /// <summary>
        /// changes the unit of length from default in solidworks (meter) to the one specified
        /// </summary>
        /// <param name="mathUtility"></param>
        /// <param name="length">the length to change the unit of</param>
        /// <param name="newUnit">the unit required to change the length to <see cref="swLengthUnit_e"/>
        /// <example>
        /// <code>
        /// ModelDoc2.LengthUnit;
        /// </code>
        /// </example>
        /// </param>
        /// <returns>length in new new unit</returns>
        public static double ConvertLengthUnit(this MathUtility mathUtility, double length, swLengthUnit_e newUnit)
        {
            return Units.ConvertLength(length, newUnit);
        }
    }
}
