using System.Collections.Generic;
using SolidWorks.Interop.sldworks;
namespace Hymma.SolidTools.SolidAddins
{
    /// <summary>
    /// a model for <see cref="CommandGroup"/>
    /// </summary>
    public class AddinCommandGroup
    {
        /// <summary>
        /// a list of <see cref="AddinCommand"/> this group presents
        /// </summary>
        public IEnumerable<AddinCommand> Commands { get; set; }

        /// <summary>
        /// Set this to true to prevent SOLIDWORKS from saving the current toolbar setting to the registry, even if there is no previous version.
        /// </summary>
        public bool IgnorePrevious { get; set; }

        /// <summary>
        /// Determines if command Group is registred in the Registry <br/>
        /// use <see cref="IgnorePrevious"/> to decide  a new commandGroup is required or not
        /// </summary>
        public bool IsRegistered { get; set; }

        /// <summary>
        /// group id of this command group registerd in Registry, gets assigned by solidworks
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// To add a menu item for a CommandGroup to an existing SOLIDWORKS menu, specify the name of a parent menu here.<br/>
        /// <example><c>"&amp;Help\\MyApp Title"</c></example>
        /// </summary>
        public string Title { get; set; } = "Title of this AddinCommandGroup";
        public string Description { get; set; } = "Description of this AddinCommandGroup";
        public string ToolTip { get; set; } = "Tooltip of this AddinCommandGroup";
        public string Hint { get; set; } = "A Hint for this AddinCommandGroup";
        public int Position { get; set; }

        /// <summary>
        /// <see cref="swCreateCommandGroupErrors"/><br/>
        /// 2=Exceeds ToolBarIDs , 1=success, 0 =Failed
        /// </summary>
        public int Errors { get; set; } = 0;
        public bool HasToolbar { get; set; } = true;
        public bool HasMenue { get; set; } = true;

        /// <summary>
        /// should be a .png file
        /// </summary>
        public string MainIcon { get; set; }
    }
}
