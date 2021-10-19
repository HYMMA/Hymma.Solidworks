using Hymma.SolidTools.Addins.UI.PropertyManagerPage.PmpControls.WindowsForm;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// create a field and button that pop up a folder browser dialog
    /// </summary>
    public class PmpFolderBrowser : PmpWindowHandler
    {
        /// <summary>
        /// provides access to a folder borwser dialog
        /// </summary>
        public PmpFolderBrowser() : base(new PmpFolderBrowserUi(), 18)
        {

        }
    }
}
