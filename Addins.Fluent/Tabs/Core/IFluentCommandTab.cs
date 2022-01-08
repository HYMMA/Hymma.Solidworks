using System.Collections.Generic;
using SolidWorks.Interop.swconst;
namespace Hymma.Solidworks.Addins.Fluent
{
    /// <summary>
    /// an interface to add command tabs to a solidworks ui for example 'Features' is a command tab
    /// </summary>
    public interface IFluentCommandTab
    {
        /// <summary>
        /// define a title for the command tab for exaple 'Features' is a title for it tab
        /// </summary>
        /// <param name="title"></param>
        IFluentCommandTab WithTitle(string title);

        /// <summary>
        /// the type of documents this tab should be visible in
        /// </summary>
        /// <param name="types"></param>
        IFluentCommandTab IsVisibleIn(IEnumerable<swDocumentTypes_e> types);

        /// <summary>
        /// add more context 
        /// </summary>
        /// <returns><see cref="IFluentCommandTab"/></returns>
        IFluentCommandTab That();

        /// <summary>
        /// A command group can be listed in a command tab or be listed under 'Tools' drop down box where you can hover over and get the list of command in that command group
        /// </summary>
        /// <param name="userId"> 
        /// If you change the definition of an existing CommandGroup (i.e., add or remove toolbar buttons), you must assign a new unique user-defined UserID to that CommandGroup. <br/>
        /// You must perform this action to avoid conflicts with any previously existing CommandGroupa and to allow for backward and forward compatibility of the CommandGroups in your application.<br/>
        /// The user ID and the GUID of the CoClass implementing ISwAddin are a unique pair.</param>
        /// <returns></returns>
        IFluentCommandGroup SetCommandGroup(int userId);

        /// <summary>
        /// saves this command tab and returns the command builder
        /// </summary>
        /// <returns></returns>
        IAddinModelBuilder SaveCommandTab();
    }
}
