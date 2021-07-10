using Hymma.SolidTools.Addins;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;
using System.Linq;

namespace Hymma.SolidTools.Fluent.Addins
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
        public IFluentCommandGroup AddGroup()
        {
            return new FluentCommandGroup(this);
        }

        ///<inheritdoc/>
        public IAddinModelBuilder SaveCommandTab()
        {
            builder.CommandTabs.ToList().Add(this);
            return builder;
        }
    }
}
