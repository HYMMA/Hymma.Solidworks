// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using Hymma.Solidworks.Addins;
using System.Collections.Generic;

namespace QrifyPlus
{
    public class QrPlusTab : PmpTab
    {
        public QrPlusTab() : base(caption: "Qrify+", icon: null)
        {
            var group = new QrPlusGroupControls();
            this.TabGroups = new List<PmpGroup>() { group };
        }
    }
}
