using Hymma.SolidTools.Addins.UI.PropertyManagerPage.PmpControls.WindowsForm;
using System.Windows.Media;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// create a field and button that pop up a folder browser dialog
    /// </summary>
    public class PmpFolderBrowser : PmpWpfHost
    {

        /// <summary>
        /// provides access to a folder borwser dialog
        /// </summary>
        public PmpFolderBrowser() : base(new PmpFolderBrowserUi(), 15)
        {
        }
        /// <summary>
        /// allows access to the windows WPF form that defines this property manager page control
        /// </summary>
        private PmpFolderBrowserUi Ui => WindowsControl as PmpFolderBrowserUi;

        /// <summary>
        /// text for the ui
        /// </summary>
        public string Text { get => Ui.Text; set => Ui.Text = value; }

        /// <summary>
        /// description for the folder browser dialogue
        /// </summary>
        public string Description { get => Ui.Description; set => Ui.Description = value; }

        /// <summary>
        /// assign value to the background
        /// </summary>
        public System.Windows.Media.Color Background { get => Ui.TextBackground.Color; set => Ui.TextBackground = new SolidColorBrush(value); }

    }
}
