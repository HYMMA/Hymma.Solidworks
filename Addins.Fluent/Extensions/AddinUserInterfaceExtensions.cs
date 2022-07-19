// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// Extension for <see cref="AddinUserInterface"/>
    /// </summary>
    public static class AddinUserInterfaceExtensions
    {
        /// <summary>
        /// Get the <see cref="AddinModelBuilder"/> to create property manager page Ui
        /// </summary>
        /// <param name="addinUserInterface"></param>
        /// <returns></returns>
        public static AddinModelBuilder GetBuilder(this AddinMaker addinUserInterface)
        {
            return new AddinModelBuilder();
        }
    }
}
