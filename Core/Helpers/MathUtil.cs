using System;

namespace Hymma.SolidTools
{
    /// <summary>
    /// usefull mathematics helpers
    /// </summary>
    public static class MathUtil
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
    }
}
