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
        /// description in the folder browser dialogue
        /// </summary>
<<<<<<< Updated upstream:Addins/UI/PropertyManagerPage/PmpControls/WinForms/PmpFolderBrowserWinForms.cs
        public Stack<string> BrowsedDirs { get; set; }
=======
        public string Description { get; set; }
>>>>>>> Stashed changes:Hymma.WinForms/FolderBrowserCombo.cs

        /// <summary>
        /// Background color of the combo box
        /// </summary>
        public System.Drawing.Color Background { get => ComboBoxDropDown.BackColor; set => ComboBoxDropDown.BackColor = value; }

        /// <summary>
        /// creates a folder browser
        /// </summary>
        public PmpFolderBrowserWinForms()
        {
            InitializeComponent();
<<<<<<< Updated upstream:Addins/UI/PropertyManagerPage/PmpControls/WinForms/PmpFolderBrowserWinForms.cs
            BrowsedDirs = new Stack<string>(5);
=======
            //Folders = new BindingList<string>();
>>>>>>> Stashed changes:Hymma.WinForms/FolderBrowserCombo.cs
        }


        private void Browse_Click(object sender, EventArgs e)
        {
            var folderBrowser = new FolderBrowserDialog()
            {
                Description = Description,
            };
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
<<<<<<< Updated upstream:Addins/UI/PropertyManagerPage/PmpControls/WinForms/PmpFolderBrowserWinForms.cs
                BrowsedDirs.Push(folderBrowser.SelectedPath);
                Addresses.DataSource = BrowsedDirs;
                Addresses.SelectedItem = folderBrowser.SelectedPath;
            }
        }

        private void PmpFolderBrowserWinForms_Load(object sender, EventArgs e)
        {
            Addresses.DataSource = BrowsedDirs;
        }
=======
                //Folders.Add(folderBrowser.SelectedPath);
                ComboBoxDropDown.Items.Add(folderBrowser.SelectedPath);
                ComboBoxDropDown.SelectedIndex = ComboBoxDropDown.Items.Count - 1;
            }
        }
>>>>>>> Stashed changes:Hymma.WinForms/FolderBrowserCombo.cs
    }
}
