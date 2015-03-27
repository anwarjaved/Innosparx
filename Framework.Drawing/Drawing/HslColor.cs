namespace Framework.Drawing
{
    using System;
    using System.Drawing;


    /// <summary>
    /// Class that allows to increase/decrease lightness
    /// of the specified color.
    /// </summary>
    public sealed class HslColor
    {
        private byte alpha = 0xff;
        private float hue;
        private float saturation;
        private float luminance;

        /// <summary>
        /// Initializes a new instance of the <see cref="HslColor" /> class.
        /// </summary>
        /// <param name="h">Hue is a value between 0 and 1.</param>
        /// <param name="s">Saturation is a value between 0 and 1.</param>
        /// <param name="l">Luminance is a value between 0 and 1.</param>
        public HslColor(float h, float s, float l)
        {
            this.Hue = h;
            this.Saturation = s;
            this.Luminance = l;
        }

        public HslColor(Color rgbColor)
        {
            this.alpha = rgbColor.A;
            this.Hue = rgbColor.GetHue() / 360.0F;
            this.Saturation = rgbColor.GetSaturation();
            this.Luminance = rgbColor.GetBrightness();
        }

        public float Hue
        {
            get
            {
                return this.hue;
            }

            set
            {
                this.hue = Math.Max(0f, Math.Min(1f, value));
            }
        }

        public float Saturation
        {
            get
            {
                return this.saturation;
            }

            set
            {
                this.saturation = Math.Max(0f, Math.Min(1f, value));
            }
        }

        public float Luminance
        {
            get
            {
                return this.luminance;
            }

            set
            {
                this.luminance = Math.Max(0f, Math.Min(1f, value));
            }
        }

        public static Color ToColor(float h, float s, float l)
        {
            HslColor color = new HslColor(h, s, l);
            return color.ToColor(0xff);
        }

        public HslColor Lighter(float percent)
        {
            float l = this.luminance;
            if (percent > 0)
            {
                l += (l * percent) * 0.01f;
            }

            return new HslColor(this.hue, this.saturation, l);
        }

        public HslColor Darker(float percent)
        {
            float l = this.luminance;
            if (percent > 0)
            {
                l -= (l * percent) * 0.01f;
            }

            return new HslColor(this.hue, this.saturation, l);
        }

        public Color ToColor()
        {
            return this.ToColor(this.alpha);
        }

        public Color ToColor(byte alpha)
        {
            return DrawingExtensions.ColorFromHSL(alpha, this.Hue, this.Saturation, this.Luminance);
        }
    }
}
