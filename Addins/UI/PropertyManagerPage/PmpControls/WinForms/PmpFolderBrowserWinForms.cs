using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Hymma.SolidTools.Addins
{
    /// <summary>
    /// folder browser for property manager page
    /// </summary>
    public partial class PmpFolderBrowserWinForms : UserControl
    {
        /// <summary>
        /// a stack of browsed directory the is bound to the dropdown
        /// </summary>
        public Stack<string> BrowsedDirs { get; set; }

        /// <summary>
        /// description in the folder browser
        /// </summary>
        public string BrowserDescription { get; set; }

        /// <summary>
        /// creates a folder browser
        /// </summary>
        public PmpFolderBrowserWinForms()
        {
            InitializeComponent();
            BrowsedDirs = new Stack<string>(5);
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            var folderBrowser = new FolderBrowserDialog()
            {
                Description = BrowserDescription,
            };
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                BrowsedDirs.Push(folderBrowser.SelectedPath);
                Addresses.DataSource = BrowsedDirs;
                Addresses.SelectedItem = folderBrowser.SelectedPath;
            }
        }

        private void PmpFolderBrowserWinForms_Load(object sender, EventArgs e)
        {
            Addresses.DataSource = BrowsedDirs;
        }
    }
}
