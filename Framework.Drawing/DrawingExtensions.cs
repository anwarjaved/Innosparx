using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;

    using Framework.Drawing;

    public static class DrawingExtensions
    {
        private const float OneThird = 0.3333333f;
        private const float TwoThirds = 0.6666667f;
        private const float OneSixth = 0.1666667f;

        public static Color Darken(this Color clr)
        {
            float hue = clr.GetHue();
            float saturation = clr.GetSaturation();
            float luminance = clr.GetBrightness() * 0.5f;
            return ColorFromHSL(clr.A, hue, saturation, luminance);
        }

        public static Bitmap Combine(IEnumerable<string> files, int padding = 0, int margin = 0)
        {
            //read all images into memory
            List<Bitmap> images = new List<Bitmap>();

            images.AddRange(files.Select(image => new System.Drawing.Bitmap(image)));

            return Combine(images, padding, margin);

        }

        public static Bitmap Combine(this IReadOnlyList<Bitmap> images, int padding = 0, int margin = 0)
        {
            //read all images into memory
            Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                foreach (Bitmap bitmap in images)
                {
                    //update the size of the final bitmap
                    width += (bitmap.Width + padding);
                    height = bitmap.Height > height ? bitmap.Height : height;
                }

                if (margin > 0)
                {
                    width += (margin * 2);
                    height += (margin * 2);
                }

                //create a bitmap to hold the combined image
                finalImage = new Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(Color.Black);

                    //go through each image and draw it on the final image
                    int offset = 0;

                    if (margin > 0)
                    {
                        offset = margin;
                    }

                    foreach (Bitmap image in images)
                    {
                        g.DrawImage(image,
                            new Rectangle(offset, margin, image.Width, image.Height));
                        offset += (image.Width + padding);
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }

        public static Color ColorFromHSL(byte alpha, float hue, float saturation, float luminance)
        {
            if (luminance == 0.0)
            {
                return Color.Black;
            }

            if (saturation == 0.0)
            {
                return Color.FromArgb(0xff, (byte)(255.0 * luminance), (byte)(255.0 * luminance), (byte)(255.0 * luminance));
            }

            float num = (luminance < 0.5f) ? (luminance * (saturation + 1f)) : ((luminance + saturation) - (luminance * saturation));
            float num2 = (luminance + luminance) - num;
            float r = hue + OneThird;
            if (r > 1f)
            {
                r--;
            }

            float g = hue;
            float b = hue - OneThird;
            if (b < 0f)
            {
                b++;
            }

            if (r < OneSixth)
            {
                r = (((num - num2) * r) * 6f) + num2;
            }
            else if (r < 0.5f)
            {
                r = num;
            }
            else if (r < TwoThirds)
            {
                r = (((num - num2) * (TwoThirds - r)) * 6f) + num2;
            }
            else
            {
                r = num2;
            }

            if (g < OneSixth)
            {
                g = (((num - num2) * g) * 6f) + num2;
            }
            else if (g < 0.5f)
            {
                g = num;
            }
            else if (g < TwoThirds)
            {
                g = (((num - num2) * (TwoThirds - g)) * 6f) + num2;
            }
            else
            {
                g = num2;
            }

            if (b < OneSixth)
            {
                b = (((num - num2) * b) * 6f) + num2;
            }
            else if (b < 0.5f)
            {
                b = num;
            }
            else if (b < TwoThirds)
            {
                b = (((num - num2) * (TwoThirds - b)) * 6f) + num2;
            }
            else
            {
                b = num2;
            }

            return Color.FromArgb(alpha, Convert.ToByte(r * 255f), Convert.ToByte(g * 255f), Convert.ToByte(b * 255f));
        }

        /// <summary>
        /// Convert specified hex string into Color.
        /// </summary>
        /// <param name="hex">Hex color to convert.</param>
        /// <returns>Converted <see cref="Color"/>.</returns>
        public static Color FromHtmlColor(this string hex)
        {
            hex = hex.Replace("#", String.Empty);
            byte a = Convert.ToByte("ff", 16);
            byte pos = 0;
            if (hex.Length == 8)
            {
                a = Convert.ToByte(hex.Substring(pos, 2), 16);
                pos = 2;
            }
            byte r = Convert.ToByte(hex.Substring(pos, 2), 16);
            pos += 2;
            byte g = Convert.ToByte(hex.Substring(pos, 2), 16);
            pos += 2;
            byte b = Convert.ToByte(hex.Substring(pos, 2), 16);
            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Saves the image to  specified stream and format.
        /// </summary>
        /// <param name="image">The image to save.</param>
        /// <param name="outputStream">The stream to used.</param>
        /// <param name="format">The format of new image.</param>
        public static void SaveTo(this Image image, Stream outputStream, ImageFormat format)
        {
            SaveTo(image, outputStream, format, 70);
        }

        /// <summary>
        /// Saves the image to  specified stream and format.
        /// </summary>
        /// <param name="image">The image to save.</param>
        /// <param name="outputStream">The stream to used.</param>
        /// <param name="format">The format of new image.</param>
        /// <param name="quality">The quality of the image in percent.</param>
        public static void SaveTo(this Image image, Stream outputStream, ImageFormat format, int quality)
        {
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            if (format == ImageFormat.Gif)
            {
                OctreeQuantizer quantizer = new OctreeQuantizer(255, 8);
                using (Bitmap quantized = quantizer.Quantize(image))
                {
                    quantized.Save(outputStream, ImageFormat.Gif);
                }
            }
            else if (format == ImageFormat.Jpeg)
            {
                image.Save(outputStream, encoders[1], encoderParameters);
            }
            else if (format == ImageFormat.Png)
            {
                image.Save(outputStream, encoders[4], encoderParameters);
            }
            else if (format == ImageFormat.Bmp)
            {
                image.Save(outputStream, encoders[0], encoderParameters);
            }
            else
            {
                image.Save(outputStream, format);
            }
        }

        public static int CalculateHeight(this Size size, int requiredWidth)
        {
            double aspect = (double)size.Width / (double)size.Height;
            return (int)(requiredWidth * aspect);
        }

        /// <summary>
        /// Calculates the transparent color of the Bitmap.
        /// </summary>
        /// <param name="image">The bitmap for which color is calculated.</param>
        /// <returns>Transparent color of the Bitmap.</returns>
        public static Color CalculateTransparentColor(this Bitmap image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            return image.GetPixel(0, image.Height - 1);
        }

        /// <summary>
        /// Clones the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>Clone of specified <see cref="Image"/>.</returns>
        public static Image Clone(this Image image)
        {
            return Clone(image, true);
        }

        /// <summary>
        /// Clones the specified image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="disposeImage">If set to <c>true</c> dispose original image.</param>
        /// <returns>Clone of specified <see cref="Image"/>.</returns>
        public static Image Clone(this Image image, bool disposeImage)
        {
            if (image == null)
            {
                return image;
            }

            if (image.Width < 0 || image.Height < 0)
            {
                return image;
            }

            Bitmap thumb = new Bitmap(image.Width, image.Height);

            using (Graphics g = Graphics.FromImage(thumb))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(image, 0, 0, image.Width, image.Height);
            }

            if (disposeImage)
            {
                image.Dispose();
            }

            return thumb;
        }
    }
}
