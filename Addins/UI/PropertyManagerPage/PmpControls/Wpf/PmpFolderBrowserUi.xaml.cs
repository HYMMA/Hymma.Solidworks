using System.Windows;
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
        }

        #region Dependency properties
        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty DescriptionProperty;
        public static readonly DependencyProperty TextBackgroundProperty;
        #endregion

        #region static constructor
        static PmpFolderBrowserUi()
        {

            TextProperty = DependencyProperty.Register("Text",
               typeof(string),
               typeof(PmpFolderBrowserUi),
               new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

            DescriptionProperty = DependencyProperty.Register("Description",
                typeof(string),
                typeof(PmpFolderBrowserUi),
                new PropertyMetadata(null));

            TextBackgroundProperty = DependencyProperty.Register("TextBackground",
                typeof(SolidColorBrush),
                typeof(PmpFolderBrowserUi),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        }
        #endregion

        #region CLI wrappers for DP 

        /// <summary>
        /// this is the text that this control displays
        /// </summary>
        public string Text
        {
            get => GetValue(TextProperty) as string;
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// set background of the text box
        /// </summary>
        public SolidColorBrush TextBackground
        {
            get => GetValue(TextBackgroundProperty) as SolidColorBrush;
            set => SetValue(TextBackgroundProperty, value);
        }

        /// <summary>
        /// description for the dialogue
        /// </summary>
        public string Description
        {
            get =>GetValue(DescriptionProperty) as string; 
            set=>SetValue(DescriptionProperty, value);
        }

        #endregion

        #region private methods
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
                }
            }
        }
        #endregion
    }
}
