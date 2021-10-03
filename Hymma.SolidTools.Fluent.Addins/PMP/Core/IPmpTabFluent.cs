using System;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// creat a tab for property manager page
    /// </summary>
    public interface IPmpTabFluent : IFluent
    {
        /// <summary>
        /// add a group to this tab
        /// </summary>
        /// <returns></returns>
        IPmpTabGroupFluent AddGroup(string caption);

        /// <summary>
        /// event to fire once this tab is displayed
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IPmpTabFluent WhenDisplayed(Action action);

        /// <summary>
        /// saves the tab
        /// </summary>
        /// <returns></returns>
        IPmpUiModelFluent SaveTab();
    }
}
