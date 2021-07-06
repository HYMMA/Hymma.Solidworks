using Hymma.SolidTools.Addins;
using System;
using System.Collections.Generic;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// an interface to add property manager page group to a <see cref="IPmpUi"/>
    /// </summary>
    public interface IPmpGroup
    {

        /// <summary>
        /// add extra context to this <see cref="PMPGroupBase"/>
        /// </summary>
        /// <returns></returns>
        IPmpGroup That();

        /// <summary>
        /// add extra context to this <see cref="PMPGroupBase"/>
        /// </summary>
        /// <returns></returns>
        IPmpGroup And();


        /// <summary>
        /// determines if this <see cref="PMPGroupBase"/> is expanded or not
        /// </summary>
        /// <param name="isExpanded"></param>
        /// <returns></returns>
        IPmpGroup IsExpanded(bool isExpanded = true);


        /// <summary>
        /// event handler for when user expands the group
        /// </summary>
        /// <param name="doThis">delegate that accepts a bool as the parameter and returns void. bool parameter will be assigned by solidworks and indicates the expansion state of the group.</param>
        /// <returns></returns>
        IPmpGroup AndOnExpansionChange(Action<bool> doThis);

        /// <summary>
        /// add these controls to the <see cref="PMPGroupBase"/>
        /// </summary>
        /// <param name="controlMaker">function that returns an <see cref="IEnumerable{T}"/> where T : <see cref="IPmpControl"/></param>
        /// <returns></returns>
        IPmpGroup HasTheseControls(Func<IEnumerable<IPmpControl>> controlMaker);
        /// <summary>
        /// add these controls to the <see cref="PMPGroupBase"/>
        /// </summary>
        /// <param name="controls">an <see cref="IEnumerable{T}"/> where T : <see cref="IPmpControl"/></param>
        /// <returns></returns>
        IPmpGroup HasTheseControls(IEnumerable<IPmpControl> controls);

        /// <summary>
        /// save all the changes 
        /// </summary>
        /// <returns></returns>
        IPmpUi SaveGroup();
    }
}