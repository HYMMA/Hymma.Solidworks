using Hymma.Solidworks.Extensions;
using System;
using System.Collections.Generic;

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// an interface to add property manager page group to a <see cref="IPmpUiModelFluent"/>
    /// </summary>
    public interface IPmpGroupFluent : IFluent
    {
        /// <summary>
        /// add extra context to this <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <returns></returns>
        IPmpGroupFluent That();

        /// <summary>
        /// add extra context to this <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <returns></returns>
        IPmpGroupFluent And();

        /// <summary>
        /// determines if this <see cref="Solidworks.Addins.PmpGroup"/> is expanded or not
        /// </summary>
        /// <param name="isExpanded"></param>
        /// <returns></returns>
        IPmpGroupFluent IsExpanded(bool isExpanded = true);

        /// <summary>
        /// hides this group
        /// </summary>
        /// <param name="isHidden"></param>
        /// <returns></returns>
        IPmpGroupFluent IsHidden(bool isHidden = true);

        /// <summary>
        /// event handler for when user expands the group
        /// </summary>
        /// <param name="doThis">delegate that accepts a bool as the parameter and returns void. bool parameter will be assigned by solidworks and indicates the expansion state of the group.</param>
        /// <returns></returns>
        IPmpGroupFluent OnExpansionChange(Action<PmpGroup, bool> doThis);

        /// <summary>
        /// add these controls to the <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <param name="controlMaker">function that returns an <see cref="IEnumerable{T}"/> where IPmpGroupFluent : <see cref="PmpControl"/></param>
        /// <returns></returns>
        IPmpGroupFluent HasTheseControls(Func<IEnumerable<PmpControl>> controlMaker);

        /// <summary>
        /// add these controls to the <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <param name="controls">an <see cref="IEnumerable{T}"/> where IPmpGroupFluent : <see cref="PmpControl"/></param>
        /// <returns></returns>
        IPmpGroupFluent HasTheseControls(IEnumerable<PmpControl> controls);

        /// <summary>
        /// background color of this group
        /// </summary>
        /// <param name="sysColor"></param>
        /// <returns></returns>
        IPmpGroupFluent Color(SysColor sysColor);
        /// <summary>
        /// save all the changes 
        /// </summary>
        /// <returns></returns>
        IPmpUiModelFluent SaveGroup();
    }
}