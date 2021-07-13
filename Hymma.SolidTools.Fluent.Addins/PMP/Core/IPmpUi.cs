using Hymma.SolidTools.Addins;
using SolidWorks.Interop.swconst;
using System;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// create a new property manager page UI which hosts a number of controls
    /// </summary>
    public interface IPmpUi
    {
        /// <summary>
        /// Add a group that hosts controls in a properyt manager page 
        /// </summary>
        /// <param name="caption">caption of the group as appears in solidworks</param>
        /// <returns></returns>
        IPmpGroup AddGroup(string caption);

        /// <summary>
        /// bitwise option as defined in <see cref="swPropertyManagerPageOptions_e"/> default is 35
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        IPmpUi WithOptions(swPropertyManagerPageOptions_e options);

        /// <summary>
        /// define void() to invoke after the propety manager page is closed
        /// </summary>
        /// <param name="doThis">void to invoke</param>
        /// <returns></returns>
        IPmpUi AfterClose(Action doThis);

        /// <summary>
        /// Action to invoke while the property manger page is closing
        /// </summary>
        /// <param name="doThis"></param>
        /// <returns></returns>
        IPmpUi WhileClosing(Action<PMPCloseReason> doThis);

        /// <summary>
        /// action to take after the property manager page is activated
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IPmpUi AfterActivation(Action action);

        /// <summary>
        /// define a funciton to be invoke when user changes the tab in the property manager page
        /// </summary>
        /// <param name="doThis"></param>
        /// <returns></returns>
        IPmpUi On_TabClicked(Func<int, bool> doThis);

        /// <summary>
        /// builds this property manager page and adds it to the <see cref="AddinUserInterface"/> <br/>
        /// use the <see cref="PropertyManagerPageX64"/>.Show() method in a <see cref="AddinCommand"/> callback function so users of your addin can actually see the property manger page once they clicked on a button
        /// </summary>
        /// <returns></returns>
        IAddinModelBuilder SavePropertyManagerPage(out PropertyManagerPageX64 propertyManagerPage);
    }
}