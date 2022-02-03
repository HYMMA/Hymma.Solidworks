using Hymma.Solidworks.Extensions;
using System;
using System.Collections.Generic;

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// an interface to add property manager page group to a <see cref="IPmpTabFluent"/>
    /// </summary>
    public interface IPmpTabGroupFluentCheckable : IFluent
    {
        /// <summary>
        /// add extra context to this <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <returns></returns>
        IPmpTabGroupFluentCheckable That();

        /// <summary>
        /// add extra context to this <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <returns></returns>
        IPmpTabGroupFluentCheckable And();

        /// <summary>
        /// determines if this <see cref="Solidworks.Addins.PmpGroup"/> is expanded or not
        /// </summary>
        /// <param name="isExpanded"></param>
        /// <returns></returns>
        IPmpTabGroupFluentCheckable SetExpansion(bool isExpanded = true);

        /// <summary>
        /// assing value to visibility status of this group
        /// </summary>
        /// <param name="visibility"></param>
        /// <returns></returns>
        IPmpTabGroupFluentCheckable SetVisibility(bool visibility);

        /// <summary>
        /// event handler for when user expands the group
        /// </summary>
        /// <param name="onPmpGroupExpandedEventHandler">delegate that accepts a bool as the parameter and returns void. bool parameter will be assigned by solidworks and indicates the expansion state of the group.</param>
        /// <returns></returns>
        IPmpTabGroupFluentCheckable OnExpansion(OnPmpGroupExpansionEventHandler onPmpGroupExpandedEventHandler);

        /// <summary>
        /// add these controls to the <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <param name="makePmpControls">function that returns an <see cref="IEnumerable{T}"/> where IPmpTabGroupFluentCheckable : <see cref="PmpControl"/></param>
        /// <returns></returns>
        IPmpTabGroupFluentCheckable HasTheseControls(MakePmpControls makePmpControls);

        /// <summary>
        /// add these controls to the <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <param name="controls">an <see cref="IEnumerable{T}"/> where IPmpTabGroupFluentCheckable : <see cref="PmpControl"/></param>
        /// <returns></returns>
        IPmpTabGroupFluentCheckable HasTheseControls(PmpControl[] controls);

        /// <summary>
        /// background color of this group
        /// </summary>
        /// <param name="sysColor"></param>
        /// <returns></returns>
        IPmpTabGroupFluentCheckable Color(SysColor sysColor);

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
        IPmpTabGroupFluentCheckable SetCheckedStatus(bool status = true);

        /// <summary>
        /// fires when user check/un-check the gourp check box
        /// </summary>
        /// <param name="doThis"></param>
        /// <returns><see cref="PmpGroupCheckable"/></returns>
        IPmpTabGroupFluentCheckable OnChecked(EventHandler<bool> doThis);
     
        /// <summary>
        /// fires when this group is displayed on the screen
        /// </summary>
        /// <param name="evntHandler"></param>
        /// <returns></returns>
        IPmpTabGroupFluentCheckable OnDisplaying(EventHandler evntHandler);
    }
}