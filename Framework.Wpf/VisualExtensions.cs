using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
 
    using System.IO;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Framework.Drawing;

    public static class VisualExtensions
    {
        public static MemoryStream ToImage(this Visual vsual, int widhth, int height, ImageFormat format)
        {
            BitmapEncoder encoder = null;

            switch (format)
            {
                case ImageFormat.Jpg:
                    encoder = new JpegBitmapEncoder();
                    break;
                case ImageFormat.Png:
                    encoder = new PngBitmapEncoder();
                    break;
                case ImageFormat.Bmp:
                    encoder = new BmpBitmapEncoder();
                    break;
                case ImageFormat.Gif:
                    encoder = new GifBitmapEncoder();
                    break;
                case ImageFormat.Tif:
                    encoder = new TiffBitmapEncoder();
                    break;
            }

            if (encoder == null) return null;

            RenderTargetBitmap rtb = RenderVisaulToBitmap(vsual, widhth, height);
            MemoryStream file = new MemoryStream();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(file);

            return file;
        }

        public static RenderTargetBitmap RenderVisaulToBitmap(Visual vsual, int width, int height)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap
                (width, height, 96, 96, PixelFormats.Default);
            rtb.Render(vsual);

            return rtb;
        }
    }
}
