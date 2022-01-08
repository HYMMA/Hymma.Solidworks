namespace Hymma.Solidworks.Fluent.Addins
{
    ///<inheritdoc/>
    public interface IPmpTabGroupFluent : IPmpGroupFluentBase<IPmpTabGroupFluent>
    {
        /// <summary>
        /// save all the changes 
        /// </summary>
        /// <returns></returns>
        IPmpTabFluent SaveGroup();
    }
}
