// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// helpers for unit conversion and calculations
    /// </summary>
    public static class Units
    {
        /// <summary>
        /// changes the unit of length from (meter) to the one specified
        /// </summary>
        /// <param name="length">the length to change the unit of</param>
        /// <param name="newUnit">the unit required to change the length to 
        /// <list type="bullet">
        /// <item>use mm for </item><description>millimeters</description>
        /// <item>use cm for </item><description>centimeters</description>
        /// <item>use m  for </item><description>meters</description>
        /// <item>use in for </item><description>inches</description>
        /// <item>use ft for </item><description>feet</description>
        /// <item>use ft-in for </item><description>inches</description>
        /// <item>use angstrom for </item><description>angstrom</description>
        /// <item>use nm for </item><description>nano meters</description>
        /// <item>use micro-m for </item><description>micro meters</description>
        /// <item>use mil for </item><description>inches</description>
        /// <item>use uin for </item><description>meters</description>
        /// </list>
        /// </param>
        /// <returns></returns>
        public static double ConvertLength(double length, string newUnit)
        {
            switch (newUnit.ToLower())
            {
                case "mm":
                    return length * 1000;
                case "cm":
                    return length * 100;
                case "m":
                    return length;
                case "in":
                    return length * 1000 / 25.4;
                case "ft":
                    return length * 3.28084;
                case "ft-in":
                    return length * 1000 / 25.4;
                case "angstrom":
                    return length * 1E-10;
                case "nm":
                    return length * 1E-9;
                case "micro-m":
                    return length * 1E-6;
                case "mil":
                    return length / 25.4;
                case "uin":
                    return length;
                default:
                    return length;
            }
        }

        /// <summary>
        /// changes the unit of length from default in solidworks (meter) to the one specified
        /// </summary>
        /// <param name="length">the length to change the unit of</param>
        /// <param name="newUnit">the unit required to change the length to <see cref="swLengthUnit_e"/>
        /// <example>
        /// <code>
        /// ModelDoc2.LengthUnit;
        /// </code>
        /// </example>
        /// </param>
        /// <returns>length in new new unit</returns>
        public static double ConvertLength(double length, swLengthUnit_e newUnit)
        {
            switch (newUnit)
            {
                case swLengthUnit_e.swMM:
                    return length * 1000;
                case swLengthUnit_e.swCM:
                    return length * 100;
                case swLengthUnit_e.swMETER:
                    return length;
                case swLengthUnit_e.swINCHES:
                    return length * 1000 / 25.4;
                case swLengthUnit_e.swFEET:
                    return length * 3.28084;
                case swLengthUnit_e.swFEETINCHES:
                    return length * 1000 / 25.4;
                case swLengthUnit_e.swANGSTROM:
                    return length * 1E-10;
                case swLengthUnit_e.swNANOMETER:
                    return length * 1E-9;
                case swLengthUnit_e.swMICRON:
                    return length * 1E-6;
                case swLengthUnit_e.swMIL:
                    return length / 25.4;
                case swLengthUnit_e.swUIN:
                    return length;
                default:
                    return length;
            }
        }
    }
}
