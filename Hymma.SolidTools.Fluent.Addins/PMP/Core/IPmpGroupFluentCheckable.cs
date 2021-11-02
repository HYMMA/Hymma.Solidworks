using Hymma.SolidTools.Addins;
using System;

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

        /// <summary>
        /// fires when user check/un-check the gourp check box
        /// </summary>
        /// <param name="doThis"></param>
        /// <returns><see cref="PmpGroupCheckable"/></returns>
        IPmpGroupFluentCheckable WhenChecked(EventHandler<bool> doThis);

        /// <summary>
        /// fires when this group is displayed on the screen
        /// </summary>
        /// <param name="doThis"></param>
        /// <returns></returns>
        IPmpGroupFluentCheckable WhenDisplayed(EventHandler doThis);
    }
}