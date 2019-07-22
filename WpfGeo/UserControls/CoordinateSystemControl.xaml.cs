using System.Windows;
namespace WpfGeo.UserControls
{
    public partial class CoordinateSystemControl
    {
        public static readonly DependencyProperty CoordinateSystemRightHandProperty;

        static CoordinateSystemControl()
        {
            CoordinateSystemRightHandProperty = DependencyProperty.Register("CoordinateSystemRightHand", typeof(bool), typeof(CoordinateSystemControl));
        }
        public CoordinateSystemControl()
        {
            InitializeComponent();
            CoordinateSystemRightHand = true;
        }

        public bool CoordinateSystemRightHand
        {
            get => (bool)GetValue(CoordinateSystemRightHandProperty);
            set => SetValue(CoordinateSystemRightHandProperty, value);
        }
    }
}
