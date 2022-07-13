// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

namespace Hymma.Solidworks.Addins.Fluent
{
    ///<inheritdoc/>
    public interface IPmpTabGroupFluent : IPmpGroupFluentBase<IPmpTabGroupFluent>
    {
        /// <summary>
        /// save all the changes 
        /// </summary>
        /// <returns></returns>
        IPmpTabFluent SaveGroup();
    }
}
