using System;

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// an interface to add property manager page group to a <see cref="IPmpTabFluent"/>
    /// </summary>
    public interface IPmpTabGroupFluentCheckable : IPmpGroupFluentBase<IPmpTabGroupFluentCheckable>
    {
        /// <summary>
        /// save all the changes 
        /// </summary>
        /// <returns><see cref="IPmpTabFluent"/></returns>
        IPmpTabFluent SaveGroup();

        /// <summary>
        /// defines whether a chackable group appears in its checked state by default
        /// </summary>
        /// <param name="status"></param>
        /// <returns><see cref="IPmpTabGroupFluentCheckable"/></returns>
        IPmpTabGroupFluentCheckable Checked(bool status = true);

        /// <summary>
        /// fires when user check/un-check the gourp check box
        /// </summary>
        /// <param name="doThis"></param>
        /// <returns><see cref="PmpGroupCheckable"/></returns>
        IPmpTabGroupFluentCheckable WhenChecked(EventHandler<bool> doThis);
     
        /// <summary>
        /// fires when this group is displayed on the screen
        /// </summary>
        /// <param name="doThis"></param>
        /// <returns></returns>
        IPmpTabGroupFluentCheckable WhenDisplayed(EventHandler doThis);
    }
}