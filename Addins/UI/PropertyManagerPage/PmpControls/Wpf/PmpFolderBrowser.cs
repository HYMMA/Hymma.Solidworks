using Hymma.SolidTools.Wpf;
using System.Windows.Forms.Integration;
namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// create a field and button that pop up a folder browser dialog
    /// </summary>
    public class PmpFolderBrowser : PmpWpfHost
    {
        private FolderBrowserCombination _combo;

        /// <summary>
        /// Creates a text box next to a button which opens up a folder browser dialogue
        /// </summary>
        /// <param name="elementHost">as solidworks uses Winforms in the background, you need to define an element host for your custom property manager page control</param>
        public PmpFolderBrowser(ElementHost elementHost) : base(elementHost, new FolderBrowserCombination(), 15)
        {
            _combo = base.WindowsControl as FolderBrowserCombination;
        }

        /// <summary>
        /// Creates a text box next to a button which opens up a folder browser dialogue
        /// </summary>
        public PmpFolderBrowser():this(new ElementHost())
        {

        }

        /// <summary>
        /// description for folder browser dialogue
        /// </summary>
        public string Description
        {
            get => _combo.Description;
            set => _combo.Description = value;
        }
        /// <summary>
        /// the text inside the text block
        /// </summary>
        public string Text { get => _combo.Text; set => _combo.Text = value; }

        /// <summary>
        /// assign value to the background color of the text in the text box
        /// </summary>
        public System.Windows.Media.Color TextBackground
        {
            get => _combo.TextBackground.Color;
            set => _combo.TextBackground.Color = value;
        }
    }
}
