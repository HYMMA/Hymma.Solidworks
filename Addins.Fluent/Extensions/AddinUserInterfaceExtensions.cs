// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymma.Solidworks.Addins.Fluent
{
    public static class AddinUserInterfaceExtensions
    {
        public static AddinModelBuilder GetBuilder(this AddinMaker addinUserInterface)
        {
            return new AddinModelBuilder();
        }
    }
}
