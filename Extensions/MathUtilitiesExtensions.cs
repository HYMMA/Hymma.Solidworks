using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Hymma.SolidTools.Extensions
{
    public static class MathUtilitiesExtensions
    {
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
        public static double ConvertLengthUnit(this MathUtility mathUtility, double length, int newUnit)
        {
            switch (newUnit)
            {
                case (int)swLengthUnit_e.swMM:
                    return length * 1000;
                case (int)swLengthUnit_e.swCM:
                    return length * 100;
                case (int)swLengthUnit_e.swMETER:
                    return length;
                case (int)swLengthUnit_e.swINCHES:
                    return length * 1000 / 25.4;
                case (int)swLengthUnit_e.swFEET:
                    return length * 3.28084;
                case (int)swLengthUnit_e.swFEETINCHES:
                    return length * 1000 / 25.4;
                case (int)swLengthUnit_e.swANGSTROM:
                    return length * 1E-10;
                case (int)swLengthUnit_e.swNANOMETER:
                    return length * 1E-9;
                case (int)swLengthUnit_e.swMICRON:
                    return length * 1E-6;
                case (int)swLengthUnit_e.swMIL:
                    return length / 25.4;
                case (int)swLengthUnit_e.swUIN:
                    return length;
                default:
                    return length;
            }
        }

    }
}
