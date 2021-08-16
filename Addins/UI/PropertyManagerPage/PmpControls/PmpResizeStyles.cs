namespace Hymma.SolidTools.Addins
{

    /// <summary>
    /// PropertyManager page control resize options. Bitmask. 
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>
    /// <term>LockLeft </term>
    /// <description>the control is locked in place relative to the left edge of the PropertyManager page. <br/>
    /// When the page width is changed, the control stays in place and its width does not change.</description>
    /// </item>
    /// <item>
    /// <term>LockRight</term>
    /// <description>the control is locked in place relative to the right edge of the PropertyManager page.<br/>
    /// When the page width is changed, the control shifts to the right, but its width does not change.</description>
    /// </item>
    /// <item>
    /// <term>LockLeft and LockRight specified</term>
    /// <description>the left edge of the control stays in place relative to the left edge and the right edge of the control stays in place relative to the right edge of the PropertyManager page,<br/>
    /// giving the effect that the control grows and shrinks with the PropertyManager page.</description>
    /// </item>
    /// </list></remarks>
    public enum PmpResizeStyles
    {
        /// <summary>
        /// the control is locked in place relative to the left edge of the PropertyManager page. <br/>
        /// When the page width is changed, the control stays in place and its width does not change.
        /// </summary>
        LockLeft = 1,

        /// <summary>
        /// the control is locked in place relative to the right edge of the PropertyManager page.<br/>
        /// When the page width is changed, the control shifts to the right, but its width does not change.
        /// </summary>
        LockRight = 2
    }
}
