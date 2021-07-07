using Hymma.SolidTools.Addins;
using SolidWorks.Interop.swconst;
using System.Collections.Generic;

namespace Hymma.SolidTools.Fluent.Addins
{
    /// <summary>
    /// A command tab in solidworks
    /// </summary>
    public class AddinCmdTab : AddinCommandTab, IAddinCommnadTab
    {
        ///<inheritdoc/>
        public void WithTitle(string title)
        {
            TabTitle = title;
        }

        ///<inheritdoc/>
        public void IsVisibleIn(IEnumerable<swDocumentTypes_e> types)
        {
            Types = types;
        }

        ///<inheritdoc/>
        public IAddinCommnadTab That()
        {
            return this;
        }

        ///<inheritdoc/>
        public IAddinCommandGroup AddGroup()
        {
            return new AddinCmdGrp();
        }
    }
}
