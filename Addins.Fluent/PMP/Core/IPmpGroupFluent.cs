// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

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