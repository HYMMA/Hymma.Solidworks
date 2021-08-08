using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// generate a new id
    /// </summary>
    internal static class PmpConstants
    {
        private static int i = 0;
        private static int m = -1;
        /// <summary>
        /// get next id
        /// </summary>
        /// <returns></returns>
        internal static int GetNextId()
        {
            return ++i;
        }

        /// <summary>
        /// returns a new mark for selection boxes
        /// </summary>
        internal static int GetNextMark()
        {
            return (int)Math.Pow(2, ++m);
        }
    }
}
