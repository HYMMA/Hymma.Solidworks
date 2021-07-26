namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// generate a new id
    /// </summary>
    internal static class PmpId
    {
        private static int i = 0;

        /// <summary>
        /// get next id
        /// </summary>
        /// <returns></returns>
        internal static int GetNext()
        {
            return i++;
        }
    }
}
