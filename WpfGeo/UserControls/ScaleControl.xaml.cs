using System.Windows;

namespace WpfGeo.UserControls
{
    public partial class ScaleControl
    {
        public static readonly DependencyProperty ScaleFactorXProperty;
        public static readonly DependencyProperty ScaleFactorYProperty;
        public static readonly DependencyProperty ScaleFactorZProperty;

        static ScaleControl()
        {
            ScaleFactorXProperty = DependencyProperty.Register("ScaleFactorX", typeof(float), typeof(ScaleControl));
            ScaleFactorYProperty = DependencyProperty.Register("ScaleFactorY", typeof(float), typeof(ScaleControl));
            ScaleFactorZProperty = DependencyProperty.Register("ScaleFactorZ", typeof(float), typeof(ScaleControl));
        }
        public ScaleControl()
        {
            InitializeComponent();
            ScaleFactorX = 1;
            ScaleFactorY = 1;
            ScaleFactorZ = 0;
        }

        public float ScaleFactorX
        {
            get => (float)GetValue(ScaleFactorXProperty);
            set => SetValue(ScaleFactorXProperty, value);
        }
        public float ScaleFactorY
        {
            get => (float)GetValue(ScaleFactorYProperty);
            set => SetValue(ScaleFactorYProperty, value);
        }
        public float ScaleFactorZ
        {
            get => (float)GetValue(ScaleFactorZProperty);
            set => SetValue(ScaleFactorZProperty, value);
        }
    }
}
