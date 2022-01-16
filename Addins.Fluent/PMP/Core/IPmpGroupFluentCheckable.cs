using Hymma.Solidworks.Extensions;
using System;
using System.Collections.Generic;

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// an interface to add property manager page group to a <see cref="IPmpUiModelFluent"/>
    /// </summary>
    public interface IPmpGroupFluentCheckable : IFluent
    {
        /// <summary>
        /// add extra context to this <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <returns></returns>
        IPmpGroupFluentCheckable That();

        /// <summary>
        /// add extra context to this <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <returns></returns>
        IPmpGroupFluentCheckable And();

        /// <summary>
        /// determines if this <see cref="Solidworks.Addins.PmpGroup"/> is expanded or not
        /// </summary>
        /// <param name="isExpanded"></param>
        /// <returns></returns>
        IPmpGroupFluentCheckable IsExpanded(bool isExpanded = true);

        /// <summary>
        /// hides this group
        /// </summary>
        /// <param name="isHidden"></param>
        /// <returns></returns>
        IPmpGroupFluentCheckable IsHidden(bool isHidden = true);

        /// <summary>
        /// event handler for when user expands the group
        /// </summary>
        /// <param name="doThis">delegate that accepts a bool as the parameter and returns void. bool parameter will be assigned by solidworks and indicates the expansion state of the group.</param>
        /// <returns></returns>
        IPmpGroupFluentCheckable AndOnExpansionChange(Action<PmpGroup, bool> doThis);

        /// <summary>
        /// add these controls to the <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <param name="controlMaker">function that returns an <see cref="IEnumerable{T}"/> where IPmpGroupFluentCheckable : <see cref="PmpControl"/></param>
        /// <returns></returns>
        IPmpGroupFluentCheckable HasTheseControls(Func<IEnumerable<PmpControl>> controlMaker);

        /// <summary>
        /// add these controls to the <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <param name="controls">an <see cref="IEnumerable{T}"/> where IPmpGroupFluentCheckable : <see cref="PmpControl"/></param>
        /// <returns></returns>
        IPmpGroupFluentCheckable HasTheseControls(IEnumerable<PmpControl> controls);

        /// <summary>
        /// background color of this group
        /// </summary>
        /// <param name="sysColor"></param>
        /// <returns></returns>
        IPmpGroupFluentCheckable Color(SysColor sysColor);

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