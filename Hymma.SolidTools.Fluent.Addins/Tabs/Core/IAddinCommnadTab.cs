
using SolidWorks.Interop.swconst;
using System.Collections.Generic;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// an interface to add command tabs to a solidworks ui for example 'Features' is a command tab
    /// </summary>
    public interface IAddinCommnadTab
    {
        /// <summary>
        /// define a title for the command tab for exaple 'Features' is a title for it tab
        /// </summary>
        /// <param name="title"></param>
        void WithTitle(string title);

        /// <summary>
        /// the type of documents this tab should be visible in
        /// </summary>
        /// <param name="types"></param>
        void IsVisibleIn(IEnumerable<swDocumentTypes_e> types);

        /// <summary>
        /// add more context 
        /// </summary>
        /// <returns><see cref="IAddinCommnadTab"/></returns>
        IAddinCommnadTab That();

        /// <summary>
        /// A command group can be listed in a command tab or be listed under 'Tools' drop down box where you can hover over and get the list of command in that command group
        /// </summary>
        /// <returns></returns>
        IAddinCommandGroup AddGroup();
    }
}
