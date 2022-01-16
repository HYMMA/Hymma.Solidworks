using Hymma.Solidworks.Extensions;
using System;
using System.Collections.Generic;

namespace Hymma.Solidworks.Addins.Fluent
{
    ///<inheritdoc/>
    public interface IPmpTabGroupFluent : IFluent
    {
        /// <summary>
        /// add extra context to this <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <returns></returns>
        IPmpTabGroupFluent That();

        /// <summary>
        /// add extra context to this <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <returns></returns>
        IPmpTabGroupFluent And();

        /// <summary>
        /// determines if this <see cref="Solidworks.Addins.PmpGroup"/> is expanded or not
        /// </summary>
        /// <param name="isExpanded"></param>
        /// <returns></returns>
        IPmpTabGroupFluent IsExpanded(bool isExpanded = true);

        /// <summary>
        /// hides this group
        /// </summary>
        /// <param name="isHidden"></param>
        /// <returns></returns>
        IPmpTabGroupFluent IsHidden(bool isHidden = true);

        /// <summary>
        /// event handler for when user expands the group
        /// </summary>
        /// <param name="doThis">delegate that accepts a bool as the parameter and returns void. bool parameter will be assigned by solidworks and indicates the expansion state of the group.</param>
        /// <returns></returns>
        IPmpTabGroupFluent AndOnExpansionChange(Action<PmpGroup, bool> doThis);

        /// <summary>
        /// add these controls to the <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <param name="controlMaker">function that returns an <see cref="IEnumerable{T}"/> where IPmpTabGroupFluent : <see cref="PmpControl"/></param>
        /// <returns></returns>
        IPmpTabGroupFluent HasTheseControls(Func<IEnumerable<PmpControl>> controlMaker);

        /// <summary>
        /// add these controls to the <see cref="Solidworks.Addins.PmpGroup"/>
        /// </summary>
        /// <param name="controls">an <see cref="IEnumerable{T}"/> where IPmpTabGroupFluent : <see cref="PmpControl"/></param>
        /// <returns></returns>
        IPmpTabGroupFluent HasTheseControls(IEnumerable<PmpControl> controls);

        /// <summary>
        /// background color of this group
        /// </summary>
        /// <param name="sysColor"></param>
        /// <returns></returns>
        IPmpTabGroupFluent Color(SysColor sysColor);

        /// <summary>
        /// save all the changes 
        /// </summary>
        /// <returns></returns>
        IPmpTabFluent SaveGroup();
    }
}
