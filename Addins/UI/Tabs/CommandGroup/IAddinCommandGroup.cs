using System.Drawing;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// represents a solidworks command group that hosts serveral commands
    /// </summary>
    public interface IAddinCommandGroup
    {
        #region  properties

        /// <summary>
        /// an array of <see cref="AddinCommand"/> this group presents
        /// </summary>
        AddinCommand[] Commands { get; set; }

        /// <summary>
        /// Set this to true to prevent SOLIDWORKS from saving the current toolbar setting to the registry, even if there is no previous version.
        /// </summary>
        bool IgnorePrevious { get; set; }

        /// <summary>
        /// Determines if command Group is registred in the Registry <br/>
        /// use <see cref="IgnorePrevious"/> to decide  a new commandGroup is required or not
        /// </summary>
        bool IsRegistered { get; set; }

        /// <summary>
        /// If you change the definition of an existing CommandGroup (i.e., add or remove toolbar buttons), you must assign a new unique user-defined UserID to that CommandGroup. <br/>
        /// You must perform this action to avoid conflicts with any previously existing CommandGroupa and to allow for backward and forward compatibility of the CommandGroups in your application.<br/>
        /// The user ID and the GUID of the CoClass implementing ISwAddin are a unique pair.
        /// </summary>
        int UserId { get; set; }

        /// <summary>
        /// To add a menu item for a CommandGroup to an existing SOLIDWORKS menu, specify the name of a parent menu here.<br/>
        /// <example><c>"&amp;Help\\MyApp Title"</c></example>
        /// </summary>
        string Title { get; set; } 

        /// <summary>
        /// Description of this AddinCommandGroup
        /// </summary>
        string Description { get; set; } 

        /// <summary>
        /// Tooltip of this AddinCommandGroup
        /// </summary>
        string ToolTip { get; set; }

        /// <summary>
        /// A Hint for this AddinCommandGroup
        /// </summary>
        string Hint { get; set; } 

        /// <summary>
        /// position of this command group in command manager 0 is on top and -1 is at the bottom
        /// </summary>
        int Position { get; set; }

        /// <summary>
        /// defines if this command group has toolbar or not
        /// </summary>
        bool HasToolbar { get; set; } 

        /// <summary>
        /// if it has menue
        /// </summary>
        bool HasMenue { get; set; }

        /// <summary>
        /// <see cref="Bitmap"/> object as icon
        /// </summary>
        Bitmap MainIconBitmap { get; set; }

        /// <summary>
        /// returns list of command strips for this command group
        /// </summary>
        string[] CommandIcons { get; set; }
        
        /// <summary>
        /// returns a list of command group icon in standard solidworks sizes
        /// </summary>
        string[] GroupIcon { get; set; }
        
        #endregion
    }
}
