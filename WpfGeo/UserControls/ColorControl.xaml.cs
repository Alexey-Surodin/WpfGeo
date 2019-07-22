using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
namespace WpfGeo.UserControls
{
    public partial class ColorControl
    {
        public static readonly DependencyProperty NumOfColorsProperty;
        public static readonly DependencyProperty AlphaProperty;
        public static readonly DependencyProperty MinRhoProperty;
        public static readonly DependencyProperty MaxRhoProperty;
        public static readonly DependencyProperty AutoColorScaleProperty;

        static ColorControl()
        {
            NumOfColorsProperty = DependencyProperty.Register("NumOfColors", typeof(int), typeof(ColorControl));
            AlphaProperty = DependencyProperty.Register("Alpha", typeof(int), typeof(ColorControl));
            MinRhoProperty = DependencyProperty.Register("MinRho", typeof(float), typeof(ColorControl));
            MaxRhoProperty = DependencyProperty.Register("MaxRho", typeof(float), typeof(ColorControl));
            AutoColorScaleProperty = DependencyProperty.Register("AutoColorScale", typeof(bool), typeof(ColorControl));
        }
        public ColorControl()
        {
            InitializeComponent();
            AutoColorScale = true;
            NumOfColors = 1;
            Alpha = 1;
        }

        public int NumOfColors
        {
            get => (int)GetValue(NumOfColorsProperty);
            set => SetValue(NumOfColorsProperty, value);
        }
        public int Alpha
        {
            get => (int)GetValue(AlphaProperty);
            set => SetValue(AlphaProperty, value);
        }
        public float MinRho
        {
            get => (float)GetValue(MinRhoProperty);
            set => SetValue(MinRhoProperty, value);
        }
        public float MaxRho
        {
            get => (float)GetValue(MaxRhoProperty);
            set => SetValue(MaxRhoProperty, value);
        }
        public bool AutoColorScale
        {
            get => (bool)GetValue(AutoColorScaleProperty);
            set => SetValue(AutoColorScaleProperty, value);
        }
        
        private void TB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"\-?\d*\z");
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}
