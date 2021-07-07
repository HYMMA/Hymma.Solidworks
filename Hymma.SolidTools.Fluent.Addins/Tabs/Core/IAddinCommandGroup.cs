using SolidWorks.Interop.swpublished;
using System.Collections.Generic;
using System.Drawing;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// an interface to define a command group in solidworks
    /// </summary>
    public interface IAddinCommandGroup
    {
        /// <summary>
        /// add more context
        /// </summary>
        /// <returns></returns>
        IAddinCommandGroup And();
        ///<summary>
        /// <param name="id"> 
        /// If you change the definition of an existing CommandGroup (i.e., add or remove toolbar buttons), you must assign a new unique user-defined UserID to that CommandGroup. <br/>
        /// You must perform this action to avoid conflicts with any previously existing CommandGroupa and to allow for backward and forward compatibility of the CommandGroups in your application.<br/>
        /// The user id and the GUID of the CoClass implementing <see cref="ISwAddin"/> are a unique pair.</param>
        ///</summary>
        void WithUserID(int id);

        /// <summary>
        /// Adds commands to this command group
        /// </summary>
        AddinCommands AddCommands();

        /// <summary>
        /// Add a single command to the command group
        /// </summary>
        IAddinCommand AddCommand( );

        /// <summary>
        /// title for this command group
        /// </summary>
        /// <param name="title"></param>
        void WithTitle(string title);

        /// <summary>
        /// tooltip for this command group
        /// </summary>
        /// <param name="toolTip"></param>
        void WithToolTip(string toolTip);

        /// <summary>
        /// provide a description for your users
        /// </summary>
        /// <param name="description"></param>
        void WithDescription(string description);

        /// <summary>
        /// Provide a hint for your users
        /// </summary>
        /// <param name="hint"></param>
        void WithHint(string hint);

        /// <summary>
        /// define an icon for this command group. it will be next to the command group name inside the command manager
        /// </summary>
        /// <param name="bitmap"></param>
        void WithIcon(Bitmap bitmap);
    }
}
