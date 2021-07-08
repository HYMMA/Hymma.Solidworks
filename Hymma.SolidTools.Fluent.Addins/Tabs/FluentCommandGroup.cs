using Hymma.SolidTools.Addins;
using System;
using System.Drawing;

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

        /*///<inheritdoc/>
        public IAddinCommand AddCommand()
        {
            return new AddinCmd();
        }*/
        
        /// <summary>
        /// saves the command group and returns its command tab
        /// </summary>
        /// <returns></returns>
        public IFluentCommandTab SaveCommnadGroup()
        {
            Tab.CommandGroup = this;
            return Tab.CastTo<IFluentCommandTab>();
        }

        ///<inheritdoc/>
        public AddinCommands Add()
        {
            return new AddinCommands(this);
        }

        ///<inheritdoc/>
        public IFluentCommandGroup And()
        {
            return this;
        }

        ///<inheritdoc/>
        public void WithDescription(string description)
        {
            Description = description;
        }

        ///<inheritdoc/>
        public void WithHint(string hint)
        {
            Hint = hint;
        }

        ///<inheritdoc/>
        public void WithIcon(Bitmap bitmap)
        {
            MainIconBitmap = bitmap;
        }

        ///<inheritdoc/>
        public void WithTitle(string title)
        {
            Title = title;
        }

        ///<inheritdoc/>
        public void WithToolTip(string toolTip)
        {
            ToolTip = toolTip;
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public void WithUserID(int id)
        {
            UserId = id;
        }
    }
}
