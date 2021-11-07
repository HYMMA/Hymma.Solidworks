using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;
namespace Hymma.SolidTools.Addins.UI.PropertyManagerPage.PmpControls.WindowsForm
{
    /// <summary>
    /// Interaction logic for PmpFolderBrowserUi.xaml
    /// </summary>
    public partial class PmpFolderBrowserUi : System.Windows.Controls.UserControl
    {
        /// <summary>
        /// creates a text filed with a <see cref="FolderBrowserDialog"/>
        /// </summary>
        public PmpFolderBrowserUi()
        {
            InitializeComponent();
            TextBackground = new SolidColorBrush(Colors.Blue);
        }

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(PmpFolderBrowserUi),new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(PmpFolderBrowserUi),new PropertyMetadata(null));
        public static DependencyProperty TextBackgroundProperty = DependencyProperty.Register("TextBackground", typeof(SolidColorBrush), typeof(PmpFolderBrowserUi), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// this is the text that this control displays
        /// </summary>
        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        /// <summary>
        /// set background of the text box
        /// </summary>
        public SolidColorBrush TextBackground { get => GetValue(TextBackgroundProperty) as SolidColorBrush; set => SetValue(TextBackgroundProperty, value); }

        /// <summary>
        /// description for the dialogue
        /// </summary>
        public string Description
        {
            get { return GetValue(DescriptionProperty) as string; }
            set
            {
                SetValue(DescriptionProperty, value);
            }
        }

        private void BrowseFolder(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = Description;
                dlg.SelectedPath = Text;
                dlg.ShowNewFolderButton = true;
                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Text = dlg.SelectedPath;
                    //BindingExpression be = GetBindingExpression(TextProperty);
                    //if (be != null)
                    //    be.UpdateSource();
                }
            }
        }
    }
}
