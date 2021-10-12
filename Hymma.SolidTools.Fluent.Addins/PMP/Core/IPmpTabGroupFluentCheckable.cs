namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// an interface to add property manager page group to a <see cref="IPmpTabFluent"/>
    /// </summary>
    public interface IPmpTabGroupFluentCheckable : IPmpGroupFluentBase<IPmpTabGroupFluentCheckable>
    {
        /// <summary>
        /// save all the changes 
        /// </summary>
        /// <returns></returns>
        IPmpTabFluent SaveGroup();

        /// <summary>
        /// defines whether a chackable group appears in its checked state by default
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        IPmpTabGroupFluentCheckable Checked(bool status = true);
    }
}