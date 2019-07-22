using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace WpfGeo.UserControls
{
    public partial class ModelBoundaryControl
    {
        public static readonly DependencyProperty MinXProperty;
        public static readonly DependencyProperty MinYProperty;
        public static readonly DependencyProperty MaxXProperty;
        public static readonly DependencyProperty MaxYProperty;
        public int MinX
        {
            get => (int)GetValue(MinXProperty);
            set => SetValue(MinXProperty, value);
        }
        public int MinY
        {
            get => (int)GetValue(MinYProperty);
            set => SetValue(MinYProperty, value);
        }
        public int MaxX
        {
            get => (int)GetValue(MaxXProperty);
            set => SetValue(MaxXProperty, value);
        }
        public int MaxY
        {
            get => (int)GetValue(MaxYProperty);
            set => SetValue(MaxYProperty, value);
        }

        static ModelBoundaryControl()
        {
            MinXProperty = DependencyProperty.Register("MinX", typeof(int), typeof(ModelBoundaryControl));
            MinYProperty = DependencyProperty.Register("MinY", typeof(int), typeof(ModelBoundaryControl));
            MaxXProperty = DependencyProperty.Register("MaxX", typeof(int), typeof(ModelBoundaryControl));
            MaxYProperty = DependencyProperty.Register("MaxY", typeof(int), typeof(ModelBoundaryControl));
        }

        public ModelBoundaryControl()
        {
            InitializeComponent();
        }

        private void TB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"\-?\d*\z");
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}
