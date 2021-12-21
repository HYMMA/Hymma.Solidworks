using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var control = new Hymma.WinForms.FolderBrowserCombo() { Description = "select something" };
            control.ComboBoxDropDown.Items.AddRange(new[] { "test", "2" });
            control.ComboBoxDropDown.Text = "text";
            control.Location = new Point(12, 12);
            this.Controls.Add(control);
        }
    }
}
