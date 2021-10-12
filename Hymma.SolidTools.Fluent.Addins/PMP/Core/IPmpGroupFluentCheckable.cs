namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// an interface to add property manager page group to a <see cref="IPmpUiModelFluent"/>
    /// </summary>
    public interface IPmpGroupFluentCheckable : IPmpGroupFluentBase<IPmpGroupFluentCheckable>
    {
        /// <summary>
        /// save all the changes 
        /// </summary>
        /// <returns></returns>
        IPmpUiModelFluent SaveGroup();

        /// <summary>
        /// defines whether a chackable group appears in its checked state by default
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        IPmpGroupFluentCheckable Checked(bool status = true);
    }
}