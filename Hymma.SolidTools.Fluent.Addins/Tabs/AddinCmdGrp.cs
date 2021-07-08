using Hymma.SolidTools.Addins;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// adds a command group to solidworks ui
    /// </summary>
    public class AddinCmdGrp : AddinCommandGroup, IAddinCmdGroup
    {
        ///<inheritdoc/>
        public IAddinCommand AddCommand()
        {
            return new AddinCmd();
        }

        ///<inheritdoc/>
        public AddinCommands Add()
        {
            return new AddinCommands(this);
        }

        ///<inheritdoc/>
        public IAddinCmdGroup And()
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
