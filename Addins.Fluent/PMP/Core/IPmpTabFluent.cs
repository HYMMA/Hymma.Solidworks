// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;

namespace Hymma.Solidworks.Addins.Fluent
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
        /// Adds a checkable group to this tab
        /// </summary>
        /// <param name="caption">caption/title for this group</param>
        /// <returns></returns>
        IPmpTabGroupFluentCheckable AddCheckableGroup(string caption);


        /// <summary>
        /// event to fire once this tab is displayed
        /// </summary>
        /// <param name="doThis"></param>
        /// <returns></returns>
        IPmpTabFluent OnDisplaying(Action doThis);

        /// <summary>
        /// define a method that returns Void to be invoke when user changes the tab in the property manager page
        /// </summary>
        /// <param name="doThis"></param>
        /// <returns></returns>
        IPmpTabFluent OnClick(Action doThis);

        /// <summary>
        /// saves the tab
        /// </summary>
        /// <returns></returns>
        IPmpUiModelFluent SaveTab();
    }
}
