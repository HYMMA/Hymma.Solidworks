using Hymma.SolidTools.Addins;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// adds a command group to solidworks ui
    /// </summary>
    public class FluentCommandGroup : AddinCommandGroup, IFluentCommandGroup
    {
        private AddinCommandTab Tab { get; }

        /// <summary>
        /// default constructor
        /// </summary>
        public FluentCommandGroup(AddinCommandTab tab)
        {
            this.Tab = tab as FluentCommandTab;
        }

        /// <inheritdoc/>
        public IFluentCommandTab SaveCommandGroup()
        {
            Tab.CommandGroup = this;
            return (IFluentCommandTab)Tab;
        }

        ///<inheritdoc/>
        public AddinCommands Has()
        {
            return new AddinCommands(this);
        }

        ///<inheritdoc/>
        public IFluentCommandGroup And()
        {
            return this;
        }

        ///<inheritdoc/>
        public IFluentCommandGroup WithDescription(string description)
        {
            Description = description;
            return this;

        }

        ///<inheritdoc/>
        public IFluentCommandGroup WithHint(string hint)
        {
            Hint = hint;
            return this;

        }

        ///<inheritdoc/>
        public IFluentCommandGroup WithIcon(Bitmap bitmap)
        {
            MainIconBitmap = bitmap;
            return this;
        }

        ///<inheritdoc/>
        public IFluentCommandGroup WithToolTip(string toolTip)
        {
            ToolTip = toolTip;
            return this;
        }

        ///<inheritdoc/>
        public IFluentCommandGroup WithTitle(string title)
        {
            Title = title;
            return this;
        }
    }
}
