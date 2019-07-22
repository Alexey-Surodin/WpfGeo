using ClassLibrary;
using Microsoft.Win32;
using System.Windows;
namespace WpfGeo.UserControls
{
    public partial class OpenFileControl
    {
        public OpenFileControl()
        {
            InitializeComponent();
        }
        static OpenFileControl()
        {
            OpenFileParamProperty = DependencyProperty.Register("OpenFileParam", typeof(OpenGeoFileArgs), typeof(OpenFileControl));
        }

        public static readonly DependencyProperty OpenFileParamProperty;
        public OpenGeoFileArgs OpenFileParam
        {
            get => (OpenGeoFileArgs)GetValue(OpenFileParamProperty);
            set => SetValue(OpenFileParamProperty, value);
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() != true) return;
            
            FilePathTextBox.Text = openFileDialog.FileName;
        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            var filePath = FilePathTextBox.Text;
            ModelBoundary mb = null;
            if (!AMC.IsChecked ?? false) mb = new ModelBoundary(MBC.MaxX, MBC.MinX, MBC.MaxY, MBC.MinY);

            OpenFileParam = new OpenGeoFileArgs(filePath, mb);
        }
    }
}
