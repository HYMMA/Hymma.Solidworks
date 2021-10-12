using System;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// generate a new id
    /// </summary>
    internal static class Counter
    {
        private static int i = 0;
        private static int p = 0;
        private static int m = -1;
        private static int rowId = -1;
        /// <summary>
        /// get next id
        /// </summary>
        /// <returns></returns>
        internal static int GetNextPmpId()
        {
            return ++i;
        }

        /// <summary>
        /// returns a new mark for selection boxes
        /// </summary>
        internal static int GetNextSelBoxMark()
        {
            return (int)Math.Pow(2, ++m);
        }

        /// <summary>
        /// get the next id for a <see cref="CalloutRow"/>
        /// </summary>
        /// <returns></returns>
        internal static int GetNextCalloutRowId()
        {
            return ++rowId;
        }

        /// <summary>
        /// get the next id for a popup menue item which is an item in the list of items in a right mouse button menue
        /// </summary>
        /// <returns></returns>
        internal static int GetNextPopUpMenuItemId()
        {
            return ++p;
        }

    }
}
