using System;
using System.Collections.Generic;
using System.Drawing;
namespace ClassLibrary
{
    public class ColorPalette
    {
        public readonly List<KeyValuePair<float, Color>> rhoColors = new List<KeyValuePair<float, Color>>();
        private float minRho, maxRho;
        public float MinRho
        {
            get => minRho;
            set
            {
                minRho = value;
                SetRhoColors();
            }
        }
        public float MaxRho
        {
            get => maxRho;
            set
            {
                maxRho = value;
                SetRhoColors();
            }
        }
        private void SetRhoColors()
        {
            rhoColors.Clear();
            if (MaxRho != MinRho && MaxRho > MinRho)
            {
                for (var i = 0; i < NumOfColors; i++)
                {
                    rhoColors.Add(new KeyValuePair<float, Color>(MinRho + (MaxRho - MinRho) * i / NumOfColors, gLColors[i]));
                }
            }
            else rhoColors.Add(new KeyValuePair<float, Color>(MinRho, gLColors[0]));
            rhoColors.Add(new KeyValuePair<float, Color>(MaxRho, gLColors[NumOfColors - 1]));
        }
        public Color GetColorByRho(float rho)
        {
            if (rho < MinRho || rho > MaxRho) return Color.FromArgb(50, 255, 255, 255);
            if (MaxRho == MinRho) return gLColors[0];
            var n = (int)Math.Abs((rho - MinRho) / (MaxRho - MinRho) * NumOfColors);
            n = n < 0 ? 0 : n >= NumOfColors ? NumOfColors - 1 : n;
            return gLColors[n];
        }
        private readonly List<Color> gLColors;
        private byte bAlpha, bSaturation, bBrightness;
        public int NumOfColors
        {
            get => gLColors.Count;
            set
            {
                if (value < 1 || value > 360) return;
                gLColors.Clear();
                GeneratePalette(value, bAlpha, bSaturation, bBrightness);
                SetRhoColors();
            }
        }
        public byte Alpha
        {
            get => bAlpha;
            set
            {
                for (var i = 0; i < gLColors.Count; i++)
                    gLColors[i] = Color.FromArgb(value, gLColors[i]);
                bAlpha = value;
            }
        }
        public ColorPalette(int numOfColors, byte alpha = byte.MaxValue, byte saturation = 100, byte brightness = 100)
        {
            gLColors = new List<Color>();
            GeneratePalette(numOfColors, alpha, saturation, brightness);
        }
        private void GeneratePalette(int numOfColors, byte alpha = byte.MaxValue, byte saturation = 100, byte brightness = 100)
        {
            if (numOfColors < 1 || numOfColors > 360) throw new ArgumentOutOfRangeException(nameof(numOfColors));
            if (saturation > 100) throw new ArgumentOutOfRangeException(nameof(saturation));
            if (brightness > 100) throw new ArgumentOutOfRangeException(nameof(brightness));
            gLColors.Clear();
            bAlpha = alpha;
            bBrightness = brightness;
            bSaturation = saturation;
            
            var hueDelta = 240 / numOfColors;
            for (var i = 0; i < numOfColors; i++)
            {
                int red, green, blue;
                var h = hueDelta * i;
                var hi = h / 60 % 6;
                var bMin = (100 - saturation) * brightness / 100;
                var a = (brightness - bMin) * (h % 60) / 60;
                var bInc = bMin + a;
                var bDec = brightness - a;
                switch (hi)
                {
                    case 0:
                        red = brightness;
                        green = bInc;
                        blue = bMin;
                        break;
                    case 1:
                        red = bDec;
                        green = brightness;
                        blue = bMin;
                        break;
                    case 2:
                        red = bMin;
                        green = brightness;
                        blue = bInc;
                        break;
                    case 3:
                        red = bMin;
                        green = bDec;
                        blue = brightness;
                        break;
                    case 4:
                        red = bInc;
                        green = bMin;
                        blue = brightness;
                        break;
                    case 5:
                        red = brightness;
                        green = bMin;
                        blue = bDec;
                        break;
                    default:
                        red = 0;
                        green = 0;
                        blue = 0;
                        break;
                }
                red *= 255 / 100;
                green *= 255 / 100;
                blue *= 255 / 100;
                gLColors.Add(Color.FromArgb(alpha, (byte)red, (byte)green, (byte)blue));
            }
        }
    }
}