// Copyright (C) HYMMA All rights reserved.
// Licensed under the MIT license

using SolidWorks.Interop.swconst;
using System.Collections.Generic;

namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// A command tab in solidworks
    /// </summary>
    public class FluentCommandTab : AddinCommandTab, IFluentCommandTab
    {
        private AddinModelBuilder builder;
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="builder"></param>
        public FluentCommandTab(AddinModelBuilder builder)
        {
            this.builder = builder;
        }
        ///<inheritdoc/>
        public IFluentCommandTab WithTitle(string title)
        {
            TabTitle = title;
            return this;
        }

        ///<inheritdoc/>
        public IFluentCommandTab IsVisibleIn(IEnumerable<swDocumentTypes_e> types)
        {
            Types = types;
            return this;
        }

        ///<inheritdoc/>
        public IFluentCommandTab That()
        {
            return this;
        }

        ///<inheritdoc/>
        public IFluentCommandGroup SetCommandGroup(int userId)
        {
            return new FluentCommandGroup(this) { UserId = userId };
        }

        ///<inheritdoc/>
        public IAddinModelBuilder SaveCommandTab()
        {
            builder.CommandTabs.Add(this);
            return builder;
        }
    }
}
