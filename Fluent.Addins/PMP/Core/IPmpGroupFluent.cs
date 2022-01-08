using Hymma.Solidworks.Addins;
using Hymma.Solidworks.Extensions;
using System;
using System.Collections.Generic;

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// an interface to add property manager page group to a <see cref="IPmpUiModelFluent"/>
    /// </summary>
    public interface IPmpGroupFluent : IPmpGroupFluentBase<IPmpGroupFluent>
    {
        /// <summary>
        /// save all the changes 
        /// </summary>
        /// <returns></returns>
        IPmpUiModelFluent SaveGroup();
    }
}