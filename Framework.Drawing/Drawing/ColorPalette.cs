namespace Framework.Drawing
{
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// The color palette.
    /// </summary>
    public sealed class ColorPalette
    {
        private static readonly Dictionary<Office2007Themes, ColorPalette> OfficeColorPalletes = BuildThemeTable();

        private readonly Color[] colors;

        public ColorPalette(string name, params Color[] colors)
        {
            this.Name = name;
            this.colors = colors;
        }

        /// <summary>
        /// Gets AllColors.
        /// </summary>
        /// <value>
        /// The all colors.
        /// </value>
        public Color[] AllColors
        {
            get
            {
                return this.colors;
            }
        }

        public string Name { get; private set; }

        public static ColorPalette GetOfficePalette(Office2007Themes themeColors)
        {
            return OfficeColorPalletes[themeColors];
        }

        public static ColorPalette GetOfficePalette(Palette colorGenTheme)
        {
            return GetOfficePalette((Office2007Themes)colorGenTheme);
        }
/*
        public Color GetColor(int index)
        {
            int num = index + 4;
            if (num < this.AllColors.Length)
            {
                return this.AllColors[num];
            }

            this.accentIndex = num;
            return this.GetNextAccent();
        }

        private Color GetNextAccent()
        {
            if (this.colors.Length <= this.accentIndex)
            {
                this.ExtendThemeColors((this.accentIndex - 4) + 1);
            }

            Color color = this.colors[this.accentIndex];
            this.accentIndex++;
            if (this.accentIndex >= 40)
            {
                this.accentIndex = 4;
            }

            return color;
        }*/

        private static Dictionary<Office2007Themes, ColorPalette> BuildThemeTable()
        {
            Dictionary<Office2007Themes, ColorPalette> pallettes = new Dictionary<Office2007Themes, ColorPalette>();
            pallettes[Office2007Themes.Standard] = new ColorPalette(
                string.Empty,
                new[]
                    {
                        Color.FromArgb(0xff, 0xbd, 0, 0), Color.FromArgb(0xff, 0xff, 0, 0),
                        Color.FromArgb(0xff, 0xff, 190, 0), Color.FromArgb(0xff, 0xff, 0xff, 0),
                        Color.FromArgb(0xff, 0x94, 0xd7, 0x52), Color.FromArgb(0xff, 0, 0xb6, 0x52),
                        Color.FromArgb(0xff, 0, 0xb6, 0xef), Color.FromArgb(0xff, 0, 0x75, 0xc6),
                        Color.FromArgb(0xff, 0, 0x22, 0x63), Color.FromArgb(0xff, 0x73, 0x35, 0x9c)
                    });

            pallettes[Office2007Themes.Office] = new ColorPalette(
                "Office",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x15, 0x48, 0x7b),
                        Color.FromArgb(0xff, 0xef, 0xef, 0xe7),

#else
                 Colors.White, Colors.Black, Color.FromArgb(0xff, 0x15, 0x48, 0x7b), Color.FromArgb(0xff, 0xef, 0xef, 0xe7), 

#endif

                        Color.FromArgb(0xff, 0x4a, 130, 0xbd), Color.FromArgb(0xff, 0xc6, 80, 0x4a),
                        Color.FromArgb(0xff, 0x9c, 0xba, 90), Color.FromArgb(0xff, 0x84, 0x65, 0xa5),
                        Color.FromArgb(0xff, 0x4a, 0xae, 0xc6), Color.FromArgb(0xff, 0xf7, 150, 0x42)
                    });

            pallettes[Office2007Themes.GrayScale] = new ColorPalette(
                "GrayScale",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0, 0, 0),
                        Color.FromArgb(0xff, 0xff, 0xff, 0xff),

#else
                 Colors.White, Colors.Black, Color.FromArgb(0xff, 0, 0, 0), Color.FromArgb(0xff, 0xff, 0xff, 0xff), 

#endif
                        Color.FromArgb(0xff, 0xde, 0xde, 0xde), Color.FromArgb(0xff, 180, 180, 180),
                        Color.FromArgb(0xff, 150, 150, 150), Color.FromArgb(0xff, 130, 130, 130),
                        Color.FromArgb(0xff, 90, 90, 90), Color.FromArgb(0xff, 0x4b, 0x4b, 0x4b)
                    });

            pallettes[Office2007Themes.Apex] = new ColorPalette(
                "Apex",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x6b, 0x65, 0x6b),
                        Color.FromArgb(0xff, 0xce, 0xc3, 0xd6),

#else
                Colors.White, Colors.Black, Color.FromArgb(0xff, 0x6b, 0x65, 0x6b), Color.FromArgb(0xff, 0xce, 0xc3, 0xd6), 
 
#endif
                        Color.FromArgb(0xff, 0xce, 0xba, 0x63), Color.FromArgb(0xff, 0x9c, 0xb2, 0x84),
                        Color.FromArgb(0xff, 0x6b, 0xb2, 0xce), Color.FromArgb(0xff, 0x63, 0x86, 0xce),
                        Color.FromArgb(0xff, 0x7b, 0x69, 0xce), Color.FromArgb(0xff, 0xa5, 120, 0xbd)
                    });

            pallettes[Office2007Themes.Aspect] = new ColorPalette(
                "Aspect",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x33, 0x2e, 0x33),
                        Color.FromArgb(0xff, 0xe7, 0xdf, 0xd6),

#else
                Colors.White, Colors.Black, Color.FromArgb(0xff, 0x33, 0x2e, 0x33), Color.FromArgb(0xff, 0xe7, 0xdf, 0xd6), 
 
#endif
                        Color.FromArgb(0xff, 0xf7, 0x7d, 0), Color.FromArgb(0xff, 0x38, 0x27, 0x33),
                        Color.FromArgb(0xff, 0x15, 0x59, 0x7b), Color.FromArgb(0xff, 0x4a, 0x86, 0x42),
                        Color.FromArgb(0xff, 0x63, 0x48, 0x7b), Color.FromArgb(0xff, 0xc6, 0x9a, 90)
                    });

            pallettes[Office2007Themes.Civic] = new ColorPalette(
                "Civic",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x63, 0x69, 0x84),
                        Color.FromArgb(0xff, 0xc6, 0xd3, 0xd6),

#else
                 Colors.White, Colors.Black, Color.FromArgb(0xff, 0x63, 0x69, 0x84), Color.FromArgb(0xff, 0xc6, 0xd3, 0xd6), 

#endif
                        Color.FromArgb(0xff, 0xd6, 0x60, 0x4a), Color.FromArgb(0xff, 0xce, 0xb6, 0),
                        Color.FromArgb(0xff, 40, 0xae, 0xad), Color.FromArgb(0xff, 140, 120, 0x73),
                        Color.FromArgb(0xff, 140, 0xb2, 140), Color.FromArgb(0xff, 14, 0x92, 0x4a)
                    });

            pallettes[Office2007Themes.Concourse] = new ColorPalette(
                "Concourse",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x42, 0x44, 0x42),
                        Color.FromArgb(0xff, 0xde, 0xf7, 0xff),

#else
               Colors.White, Colors.Black, Color.FromArgb(0xff, 0x42, 0x44, 0x42), Color.FromArgb(0xff, 0xde, 0xf7, 0xff), 
  
#endif
                        Color.FromArgb(0xff, 0x2b, 0xa2, 0xbd), Color.FromArgb(0xff, 0xde, 0x1c, 0x2b),
                        Color.FromArgb(0xff, 0xef, 0x65, 0x15), Color.FromArgb(0xff, 0x38, 0x60, 0x9c),
                        Color.FromArgb(0xff, 0x42, 0x48, 0x7b), Color.FromArgb(0xff, 0x7b, 0x3d, 0x4a)
                    });

            pallettes[Office2007Themes.Equity] = new ColorPalette(
                "Equity",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x6b, 0x65, 0x63),
                        Color.FromArgb(0xff, 0xef, 0xe7, 0xde),

#else
                 Colors.White, Colors.Black, Color.FromArgb(0xff, 0x6b, 0x65, 0x63), Color.FromArgb(0xff, 0xef, 0xe7, 0xde), 

#endif
                        Color.FromArgb(0xff, 0xd6, 0x48, 0x15), Color.FromArgb(0xff, 0x9c, 0x2b, 0x15),
                        Color.FromArgb(0xff, 0xa5, 0x8e, 0x6b), Color.FromArgb(0xff, 0x94, 0x60, 0x52),
                        Color.FromArgb(0xff, 0x94, 0x86, 0x84), Color.FromArgb(0xff, 0x84, 0x5d, 90)
                    });

            pallettes[Office2007Themes.Flow] = new ColorPalette(
                "Flow",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0, 0x60, 0x7b),
                        Color.FromArgb(0xff, 0xde, 0xf7, 0xff),

#else
                Colors.White, Colors.Black, Color.FromArgb(0xff, 0, 0x60, 0x7b), Color.FromArgb(0xff, 0xde, 0xf7, 0xff), 
 
#endif
                        Color.FromArgb(0xff, 0, 0x6d, 0xc6), Color.FromArgb(0xff, 0, 0x9e, 0xde),
                        Color.FromArgb(0xff, 0, 0xd3, 0xde), Color.FromArgb(0xff, 0x15, 0xcf, 0x9c),
                        Color.FromArgb(0xff, 0x7b, 0xcb, 0x63), Color.FromArgb(0xff, 0xa5, 0xc3, 0x4a)
                    });

            pallettes[Office2007Themes.Foundry] = new ColorPalette(
                "Foundry",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x63, 0x69, 0x52),
                        Color.FromArgb(0xff, 0xef, 0xeb, 0xde),

#else
              Colors.White, Colors.Black, Color.FromArgb(0xff, 0x63, 0x69, 0x52), Color.FromArgb(0xff, 0xef, 0xeb, 0xde), 
   
#endif
                        Color.FromArgb(0xff, 0x73, 0xa2, 0x73), Color.FromArgb(0xff, 0xb5, 0xcf, 0xb5),
                        Color.FromArgb(0xff, 0xad, 0xcf, 0xd6), Color.FromArgb(0xff, 0xc6, 190, 0xad),
                        Color.FromArgb(0xff, 0xce, 0xc7, 0x94), Color.FromArgb(0xff, 0xef, 0xb6, 0xb5)
                    });

            pallettes[Office2007Themes.Median] = new ColorPalette(
                "Median",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x73, 0x5d, 0x52),
                        Color.FromArgb(0xff, 0xef, 0xdf, 0xc6),

#else
                 Colors.White, Colors.Black, Color.FromArgb(0xff, 0x73, 0x5d, 0x52), Color.FromArgb(0xff, 0xef, 0xdf, 0xc6), 

#endif
                        Color.FromArgb(0xff, 0x94, 0xb6, 0xd6), Color.FromArgb(0xff, 0xde, 130, 0x42),
                        Color.FromArgb(0xff, 0xa5, 170, 0x84), Color.FromArgb(0xff, 0xde, 0xb2, 90),
                        Color.FromArgb(0xff, 0x7b, 0xa6, 0x9c), Color.FromArgb(0xff, 0x94, 0x8e, 140)
                    });


            pallettes[Office2007Themes.Metro] = new ColorPalette(
                "Metro",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x4a, 0x59, 0x6b),
                        Color.FromArgb(0xff, 0xd6, 0xef, 0xff),

#else
                 Colors.White, Colors.Black, Color.FromArgb(0xff, 0x4a, 0x59, 0x6b), Color.FromArgb(0xff, 0xd6, 0xef, 0xff), 

#endif
                        Color.FromArgb(0xff, 0x7b, 0xd3, 0x38), Color.FromArgb(0xff, 0xef, 0x15, 0x7b),
                        Color.FromArgb(0xff, 0xff, 0xba, 0), Color.FromArgb(0xff, 0, 0xae, 0xde),
                        Color.FromArgb(0xff, 0x73, 0x8a, 0xce), Color.FromArgb(0xff, 0x15, 0xb2, 0x9c)
                    });

            pallettes[Office2007Themes.Module] = new ColorPalette(
                "Module",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 90, 0x60, 0x7b),
                        Color.FromArgb(0xff, 0xd6, 0xd7, 0xd6),

#else
                 Colors.White, Colors.Black, Color.FromArgb(0xff, 90, 0x60, 0x7b), Color.FromArgb(0xff, 0xd6, 0xd7, 0xd6), 

#endif
                        Color.FromArgb(0xff, 0xf7, 0xae, 0), Color.FromArgb(0xff, 0x63, 0xb6, 0xce),
                        Color.FromArgb(0xff, 0xe7, 0x6d, 0x7b), Color.FromArgb(0xff, 0x6b, 0xb6, 0x6b),
                        Color.FromArgb(0xff, 0xef, 0x86, 0x52), Color.FromArgb(0xff, 0xc6, 0x48, 0x42)
                    });

            pallettes[Office2007Themes.Opulent] = new ColorPalette(
                "Opulent",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0xb5, 0x3d, 0x9c),
                        Color.FromArgb(0xff, 0xf7, 0xe7, 0xef),

#else
                Colors.White, Colors.Black, Color.FromArgb(0xff, 0xb5, 0x3d, 0x9c), Color.FromArgb(0xff, 0xf7, 0xe7, 0xef), 
 
#endif
                        Color.FromArgb(0xff, 0xbd, 0x3d, 0x6b), Color.FromArgb(0xff, 0xad, 0x65, 0xbd),
                        Color.FromArgb(0xff, 0xde, 0x6d, 0x33), Color.FromArgb(0xff, 0xff, 0xb6, 0x38),
                        Color.FromArgb(0xff, 0xce, 0x6d, 0xa5), Color.FromArgb(0xff, 0xff, 0x8e, 0x38)
                    });

            pallettes[Office2007Themes.Oriel] = new ColorPalette(
                "Oriel",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x52, 0x5d, 0x6b),
                        Color.FromArgb(0xff, 0xff, 0xf3, 0x9c),

#else
              Colors.White, Colors.Black, Color.FromArgb(0xff, 0x52, 0x5d, 0x6b), Color.FromArgb(0xff, 0xff, 0xf3, 0x9c), 
   
#endif
                        Color.FromArgb(0xff, 0xff, 0x86, 0x33), Color.FromArgb(0xff, 0x73, 0x9a, 0xde),
                        Color.FromArgb(0xff, 0xb5, 0x2b, 0x15), Color.FromArgb(0xff, 0xf7, 0xcf, 0x2b),
                        Color.FromArgb(0xff, 0xad, 0xba, 0xd6), Color.FromArgb(0xff, 0x73, 0x7d, 0x84)
                    });

            pallettes[Office2007Themes.Origin] = new ColorPalette(
                "Origin",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x42, 0x44, 0x52),
                        Color.FromArgb(0xff, 0xde, 0xeb, 0xef),

#else
               Colors.White, Colors.Black, Color.FromArgb(0xff, 0x42, 0x44, 0x52), Color.FromArgb(0xff, 0xde, 0xeb, 0xef), 
  
#endif
                        Color.FromArgb(0xff, 0x73, 0x7d, 0xa5), Color.FromArgb(0xff, 0x9c, 0xba, 0xce),
                        Color.FromArgb(0xff, 0xd6, 0xdb, 0x7b), Color.FromArgb(0xff, 0xff, 0xdb, 0x7b),
                        Color.FromArgb(0xff, 0xbd, 0x86, 0x73), Color.FromArgb(0xff, 140, 0x72, 0x6b)
                    });

            pallettes[Office2007Themes.Paper] = new ColorPalette(
                "Paper",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x42, 0x4c, 0x22),
                        Color.FromArgb(0xff, 0xff, 0xfb, 0xce),

#else
                 Colors.White, Colors.Black, Color.FromArgb(0xff, 0x42, 0x4c, 0x22), Color.FromArgb(0xff, 0xff, 0xfb, 0xce), 

#endif
                        Color.FromArgb(0xff, 0xa5, 0xb6, 0x94), Color.FromArgb(0xff, 0xf7, 0xa6, 0x42),
                        Color.FromArgb(0xff, 0xe7, 190, 0x2b), Color.FromArgb(0xff, 0xd6, 0x92, 0xa5),
                        Color.FromArgb(0xff, 0x9c, 0x86, 0xc6), Color.FromArgb(0xff, 0x84, 0x9e, 0xc6)
                    });

            pallettes[Office2007Themes.Solstice] = new ColorPalette(
                "Solstice",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x4a, 0x22, 0x15),
                        Color.FromArgb(0xff, 0xe7, 0xdf, 0xce),

#else
                 Colors.White, Colors.Black, Color.FromArgb(0xff, 0x4a, 0x22, 0x15), Color.FromArgb(0xff, 0xe7, 0xdf, 0xce), 

#endif
                        Color.FromArgb(0xff, 0x38, 0x92, 0xa5), Color.FromArgb(0xff, 0xff, 0xba, 0),
                        Color.FromArgb(0xff, 0xc6, 0x2b, 0x2b), Color.FromArgb(0xff, 0x84, 170, 0x33),
                        Color.FromArgb(0xff, 0x94, 0x42, 0), Color.FromArgb(0xff, 0x42, 0x59, 140)
                    });

            pallettes[Office2007Themes.Technic] = new ColorPalette(
                "Technic",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x38, 0x38, 0x38),
                        Color.FromArgb(0xff, 0xd6, 0xd3, 0xd6),

#else
               Colors.White, Colors.Black, Color.FromArgb(0xff, 0x38, 0x38, 0x38), Color.FromArgb(0xff, 0xd6, 0xd3, 0xd6), 
  
#endif
                        Color.FromArgb(0xff, 0x6b, 0xa2, 0xb5), Color.FromArgb(0xff, 0xce, 0xae, 0),
                        Color.FromArgb(0xff, 140, 0x8a, 0xa5), Color.FromArgb(0xff, 0x73, 0x86, 0x63),
                        Color.FromArgb(0xff, 0x9c, 0x92, 0x73), Color.FromArgb(0xff, 0x7b, 0x86, 140)
                    });

            pallettes[Office2007Themes.Trek] = new ColorPalette(
                "Trek",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x4a, 0x38, 0x33),
                        Color.FromArgb(0xff, 0xff, 0xef, 0xce),

#else
                 Colors.White, Colors.Black, Color.FromArgb(0xff, 0x4a, 0x38, 0x33), Color.FromArgb(0xff, 0xff, 0xef, 0xce), 

#endif
                        Color.FromArgb(0xff, 0xf7, 0xa2, 0x2b), Color.FromArgb(0xff, 0xa5, 0x65, 0x4a),
                        Color.FromArgb(0xff, 0xb5, 0x8a, 0x84), Color.FromArgb(0xff, 0xc6, 0x9a, 0x6b),
                        Color.FromArgb(0xff, 0xa5, 150, 0x73), Color.FromArgb(0xff, 0xc6, 0x75, 0x2b)
                    });

            pallettes[Office2007Themes.Urban] = new ColorPalette(
                "Urban",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x42, 0x44, 0x52),
                        Color.FromArgb(0xff, 0xde, 0xdf, 0xde),

#else
                 Colors.White, Colors.Black, Color.FromArgb(0xff, 0x42, 0x44, 0x52), Color.FromArgb(0xff, 0xde, 0xdf, 0xde), 

#endif
                        Color.FromArgb(0xff, 0x52, 0x55, 140), Color.FromArgb(0xff, 0x42, 130, 0x84),
                        Color.FromArgb(0xff, 0xa5, 0x4c, 0xa5), Color.FromArgb(0xff, 0xc6, 0x65, 0x2b),
                        Color.FromArgb(0xff, 140, 0x5d, 0x38), Color.FromArgb(0xff, 90, 0x92, 0xb5)
                    });

            pallettes[Office2007Themes.Verve] = new ColorPalette(
                "Verve",
                new[]
                    {
#if (!SILVERLIGHT && !WINDOWS_PHONE)
                        Color.White, Color.Black, Color.FromArgb(0xff, 0x63, 0x65, 0x63),
                        Color.FromArgb(0xff, 0xd6, 0xd3, 0xd6),

#else
                 Colors.White, Colors.Black, Color.FromArgb(0xff, 0x63, 0x65, 0x63), Color.FromArgb(0xff, 0xd6, 0xd3, 0xd6), 

#endif
                        Color.FromArgb(0xff, 0xff, 0x38, 140), Color.FromArgb(0xff, 0xe7, 0, 90),
                        Color.FromArgb(0xff, 0x9c, 0, 0x7b), Color.FromArgb(0xff, 0x6b, 0, 0x7b),
                        Color.FromArgb(0xff, 0, 0x59, 0xd6), Color.FromArgb(0xff, 0, 0x35, 0x9c)
                    });

            return pallettes;
        }
/*
        private bool ExtendThemeColors(int maxAccentColors)
        {
            if ((this.colors == null) || (this.colors.Length < 10))
            {
                return false;
            }

            if ((maxAccentColors + 4) > this.colors.Length)
            {
                maxAccentColors = (int)Math.Round(Math.Ceiling(maxAccentColors / 6.0) * 6.0);
                int num = this.colors.Length - 6;
                int num2 = maxAccentColors + 4;
                Color[] array = new Color[num2];
                this.colors.CopyTo(array, 0);
                num2 -= 6;
                for (int i = num; i < num2; i++)
                {
                    Color clr = array[i];
                    float hue = clr.GetHue();
                    float saturation = clr.GetSaturation();
                    float num6 = (clr.GetBrightness() + 0.15f) % 1f;
                    if (num6 < 0.25f)
                    {
                        num6 += 0.25f;
                    }

                    clr = DrawingExtensions.ColorFromHSL(hue, saturation, num6);
                    array[i + 6] = clr;
                }

                this.colors = array;
            }

            return true;
        }*/
    }
}
