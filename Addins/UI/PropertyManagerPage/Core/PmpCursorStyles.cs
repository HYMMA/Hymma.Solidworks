namespace Hymma.Solidworks.Addins
{
    /// <summary>
    ///  PropertyManager page's cursor after a user makes a selection in the SOLIDWORKS graphics area. 
    /// </summary>
    public enum PmpCursorStyles
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Okay and close the PropertyManager page
        /// </summary>
        Okay = 1,

        /// <summary>
        /// Move to the next selection box on the PropertyManager page
        /// </summary>
        Advance = 2
    }
}
