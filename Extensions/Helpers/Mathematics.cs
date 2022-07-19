// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;

namespace Hymma.Solidworks.Extensions
{
    /// <summary>
    /// provides numerous helper classes for unit conversion, mathematical calculations etc
    /// </summary>
    /// <summary>
    /// useful mathematics helpers
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
        /// <remarks>this method should help with the fact that double values lose percision in arithmetic operations</remarks>
        /// <returns></returns>
        public static bool AlmostEqual(double num1, double num2, double tolerance = double.Epsilon)
        {
            return Math.Abs(num1 - num2) < tolerance;
        }
        /// <summary>
        ///This method should help with the fact that double values lose percision in arithmetic operations. Determine if two numbers are in the same range of each other
        ///specifically useful when you want to know if two numbers are equal if you disregard
        ///tiny differences in values
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="digits">the difference between the two numbers will be rounded to this decimal point</param>
        /// <remarks>Two numbers are almost equal to each other when their value is assessed against another number. As our environment is manufacturing and the most accurate devices (wire cutters) have a percision of 1E-5 meters we should round differences of the two numbers to that percision</remarks>
        public static bool AlmostEqual(double v1, double v2, int digits = 5)
        {
            var diff = Math.Abs(v1 - v2);

            //based on wire cutting which has 1E-5(m) percision
            return Math.Round(diff, (5)) == 0;
        }

        /// <summary>
        /// determines if two vectors are in the same range of each other <br/>
        /// specifically useful when you want to know if two vectors are equal if you disregard tiny differences in values
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <param name="digits">the difference between the two numbers will be rounded to this decimal point</param>
        /// <remarks>this method should help with the fact that double values lose percision in arithmetic operations</remarks>
        /// <returns>true if all members of each vector (array) are equal to a certain tolerance and false otherwise</returns>
        public static bool AlmostEqual(double[] vector1, double[] vector2, int digits = 5)
        {
            if (vector2.Length != vector1.Length)
                return false;

            for (int i = 0; i < vector1.Length; i++)
            {
                if (!AlmostEqual(vector1[i], vector2[i], digits))
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
}