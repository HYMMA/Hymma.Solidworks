using Hymma.Solidworks.Addins;
using Hymma.Solidworks.Extensions;
using System;
using System.Collections.Generic;

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// an interface to add property manager page group to a <see cref="IPmpUiModelFluent"/>
    /// </summary>
    public interface IPmpGroupFluentBase<T> : IFluent
    {
        /// <summary>
        /// add extra context to this <see cref="SolidTools.Addins.PmpGroup"/>
        /// </summary>
        /// <returns></returns>
        T That();

        /// <summary>
        /// add extra context to this <see cref="SolidTools.Addins.PmpGroup"/>
        /// </summary>
        /// <returns></returns>
        T And();

        /// <summary>
        /// determines if this <see cref="SolidTools.Addins.PmpGroup"/> is expanded or not
        /// </summary>
        /// <param name="isExpanded"></param>
        /// <returns></returns>
        T IsExpanded(bool isExpanded = true);

        /// <summary>
        /// hides this group
        /// </summary>
        /// <param name="isHidden"></param>
        /// <returns></returns>
        T IsHidden(bool isHidden = true);

        /// <summary>
        /// event handler for when user expands the group
        /// </summary>
        /// <param name="doThis">delegate that accepts a bool as the parameter and returns void. bool parameter will be assigned by solidworks and indicates the expansion state of the group.</param>
        /// <returns></returns>
        T AndOnExpansionChange(Action<PmpGroup, bool> doThis);

        /// <summary>
        /// add these controls to the <see cref="SolidTools.Addins.PmpGroup"/>
        /// </summary>
        /// <param name="controlMaker">function that returns an <see cref="IEnumerable{T}"/> where T : <see cref="IPmpControl"/></param>
        /// <returns></returns>
        T HasTheseControls(Func<IEnumerable<IPmpControl>> controlMaker);
     
        /// <summary>
        /// add these controls to the <see cref="SolidTools.Addins.PmpGroup"/>
        /// </summary>
        /// <param name="controls">an <see cref="IEnumerable{T}"/> where T : <see cref="IPmpControl"/></param>
        /// <returns></returns>
        T HasTheseControls(IEnumerable<IPmpControl> controls);
        
        /// <summary>
        /// background color of this group
        /// </summary>
        /// <param name="sysColor"></param>
        /// <returns></returns>
        T Color(SysColor sysColor);
    }
}