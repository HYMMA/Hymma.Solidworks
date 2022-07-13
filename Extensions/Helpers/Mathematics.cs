// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// provides numerous helper classes for unit conversion, mathematical calculaitons etc
    /// </summary>
    /// <summary>
    /// usefull mathematics helpers
    /// </summary>
    public static class Mathematics
    {
        /// <summary>
        /// determine if two numbers are in the same range of each other<br/>
        /// specifically useful when you want to know if two numbers are equal if you disregard tiny differences in values
        /// </summary>
        /// <param name="num1"></param>
        /// <param name="num2"></param>
        /// <param name="tolerance">the amount you want to compare the tow numbers against</param>
        /// <remarks>this method should help with the fact that double values lose percision in arithmatic operations</remarks>
        /// <returns></returns>
        public static bool AlmostEqual(double num1, double num2, double tolerance = double.Epsilon)
        {
            return Math.Abs(num1 - num2) < tolerance;
        }

        /// <summary>
        /// determines if two vectors are in the same range of each other <br/>
        /// sepcifically usefull when you want to know if two vectors are equal if you disregard tiny differences in values
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <remarks>this method should help with the fact that double values lose percision in arithmatic operations</remarks>
        /// <param name="tolerance">all members of the vecotrs should be within the range of this tolerance</param>
        /// <returns>true if all members of each vector (array) are equal to a certain tolerance and false otherwise</returns>
        public static bool AlmostEqual(double[] vector1, double[] vector2, double tolerance = double.Epsilon)
        {
            if (vector2.Length != vector1.Length)
                return false;

            for (int i = 0; i < vector1.Length; i++)
            {
                if (!AlmostEqual(vector1[i], vector2[i], tolerance))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// calculate the dot product of two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>V1.V2 or -1 on error</returns>
        public static double VectorsDot(double[] v1, double[] v2)
        {
            if (v1.Length != v2.Length)
                return -1;
            double dot = 0;
            for (int i = 0; i < v2.Length; i++)
            {
                dot += v1[i] * v2[i];
            }
            return dot;
        }

        /// <summary>
        /// calculate the cross product of two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>V1 x V2 or {0,0,0} on error</returns>
        public static double[] VectorsCross(double[] v1, double[] v2)
        {
            if (v1.Length != v2.Length)
                return new double[3] { 0, 0, 0 };

            double[] cross = new double[3];
            cross[0] = (v1[1] * v2[2]) - (v1[2] * v2[1]);
            cross[1] = (v1[2] * v2[0]) - (v1[0] * v2[2]);
            cross[2] = (v1[0] * v2[1]) - (v1[1] * v2[0]);
            return cross;
        }

    }

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
        /// <item>mm</item><description>millimeters</description>
        /// <item>cm</item><description>centimeters</description>
        /// <item>m</item><description>meters</description>
        /// <item>in</item><description>inches</description>
        /// <item>ft</item><description>feet</description>
        /// <item>ft-in</item><description>inches</description>
        /// <item>angstrom</item><description>angstrom</description>
        /// <item>nm</item><description>nano meters</description>
        /// <item>micro-m</item><description>micro meters</description>
        /// <item>mil</item><description>inches</description>
        /// <item>uin</item><description>meters</description>
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
    }
}
