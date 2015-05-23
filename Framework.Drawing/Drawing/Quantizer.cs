﻿namespace Framework.Drawing
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// Base Quantizer from which all <see cref="Quantizer"/> derive.
    /// </summary>
    public abstract class Quantizer
    {
        private readonly int pixelSize;

        /// <summary>
        /// Flag used to indicate whether a single pass or two passes are needed for quantization.
        /// </summary>
        private readonly bool singlePass;

        /// <summary>
        /// Initializes a new instance of the <see cref="Quantizer"/> class.
        /// </summary>
        /// <param name="singlePass">
        /// If true, the quantization only needs to loop through the source pixels once.
        /// </param>
        /// <remarks>
        /// If you construct this class with a true value for singlePass, then the code will, when quantizing your image,
        /// only call the 'QuantizeImage' function. If two passes are required, the code will call 'InitialQuantizeImage'
        /// and then 'QuantizeImage'.
        /// </remarks>
        protected Quantizer(bool singlePass)
        {
            this.singlePass = singlePass;
            this.pixelSize = Marshal.SizeOf(typeof(Color32));
        }

        /// <summary>
        /// Quantize an image and return the resulting output bitmap.
        /// </summary>
        /// <param name="source">
        /// The image to quantize.
        /// </param>
        /// <returns>
        /// A quantized version of the image.
        /// </returns>
        
        public Bitmap Quantize(Image source)
        {
            // Get the size of the source image
            int height = source.Height;
            int width = source.Width;

            // And construct a rectangle from these dimensions
            var bounds = new Rectangle(0, 0, width, height);

            // First off take a 32bpp copy of the image
            var copy = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            // And construct an 8bpp version
            var output = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            // Now lock the bitmap into memory
            using (Graphics g = Graphics.FromImage(copy))
            {
                g.PageUnit = GraphicsUnit.Pixel;

                // Draw the source image onto the copy bitmap,
                // which will effect a widening as appropriate.
                g.DrawImage(source, bounds);
            }

            // Define a pointer to the bitmap data
            BitmapData sourceData = null;

            try
            {
                // Get the source image bits and lock into memory
                sourceData = copy.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                // Call the FirstPass function if not a single pass algorithm.
                // For something like an octree quantizer, this will run through
                // all image pixels, build a data structure, and create a palette.
                if (!this.singlePass)
                {
                    this.FirstPass(sourceData, width, height);
                }

                // Then set the color palette on the output bitmap. I'm passing in the current palette 
                // as there's no way to construct a new, empty palette.
                output.Palette = this.GetPalette(output.Palette);

                // Then call the second pass which actually does the conversion
                this.SecondPass(sourceData, output, width, height, bounds);
            }
            finally
            {
                // Ensure that the bits are unlocked
                copy.UnlockBits(sourceData);
            }

            // Last but not least, return the output bitmap
            return output;
        }

        /// <summary>
        /// Execute the first pass through the pixels in the image.
        /// </summary>
        /// <param name="sourceData">
        /// The source data.
        /// </param>
        /// <param name="width">
        /// The width in pixels of the image.
        /// </param>
        /// <param name="height">
        /// The height in pixels of the image.
        /// </param>
        protected virtual void FirstPass(BitmapData sourceData, int width, int height)
        {
            // Define the source data pointers. The source row is a byte to
            // keep addition of the stride value easier (as this is in bytes)              
            IntPtr sourceRow = sourceData.Scan0;

            // Loop through each row
            for (int row = 0; row < height; row++)
            {
                // Set the source pixel to the first pixel in this row
                IntPtr sourcePixel = sourceRow;

                // And loop through each column
                for (int col = 0; col < width; col++)
                {
                    this.InitialQuantizePixel(new Color32(sourcePixel));
                    sourcePixel = (IntPtr)((int)sourcePixel + this.pixelSize);
                }

                // Now I have the pixel, call the FirstPassQuantize function...

                // Add the stride to the source row
                sourceRow = (IntPtr)((long)sourceRow + sourceData.Stride);
            }
        }

        /// <summary>
        /// Retrieve the palette for the quantized image.
        /// </summary>
        /// <param name="original">
        /// Any old palette, this is overrwritten.
        /// </param>
        /// <returns>
        /// The new color palette.
        /// </returns>
        protected abstract System.Drawing.Imaging.ColorPalette GetPalette(System.Drawing.Imaging.ColorPalette original);

        /// <summary>
        /// Override this to process the pixel in the first pass of the algorithm.
        /// </summary>
        /// <param name="pixel">
        /// The pixel to quantize.
        /// </param>
        /// <remarks>
        /// This function need only be overridden if your quantize algorithm needs two passes,
        /// such as an Octree quantizer.
        /// </remarks>
        protected virtual void InitialQuantizePixel(Color32 pixel)
        {
        }

        /// <summary>
        /// Override this to process the pixel in the second pass of the algorithm.
        /// </summary>
        /// <param name="pixel">
        /// The pixel to quantize.
        /// </param>
        /// <returns>
        /// The quantized value.
        /// </returns>
        protected abstract byte QuantizePixel(Color32 pixel);

        /// <summary>
        /// Execute a second pass through the bitmap.
        /// </summary>
        /// <param name="sourceData">
        /// The source bitmap, locked into memory.
        /// </param>
        /// <param name="output">
        /// The output bitmap.
        /// </param>
        /// <param name="width">
        /// The width in pixels of the image.
        /// </param>
        /// <param name="height">
        /// The height in pixels of the image.
        /// </param>
        /// <param name="bounds">
        /// The bounding rectangle.
        /// </param>
        
        protected virtual void SecondPass(BitmapData sourceData, Bitmap output, int width, int height, Rectangle bounds)
        {
            BitmapData outputData = null;

            try
            {
                // Lock the output bitmap into memory
                outputData = output.LockBits(bounds, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

                // Define the source data pointers. The source row is a byte to
                // keep addition of the stride value easier (as this is in bytes)
                IntPtr sourceRow = sourceData.Scan0;
                IntPtr sourcePixel = sourceRow;
                IntPtr previousPixel = sourcePixel;

                // Now define the destination data pointers
                IntPtr destinationRow = outputData.Scan0;
                IntPtr destinationPixel = destinationRow;

                // And convert the first pixel, so that I have values going into the loop
                byte pixelValue = this.QuantizePixel(new Color32(sourcePixel));

                // Assign the value of the first pixel
                Marshal.WriteByte(destinationPixel, pixelValue);

                // Loop through each row
                for (int row = 0; row < height; row++)
                {
                    // Set the source pixel to the first pixel in this row
                    sourcePixel = sourceRow;

                    // And set the destination pixel pointer to the first pixel in the row
                    destinationPixel = destinationRow;

                    // Loop through each pixel on this scan line
                    for (int col = 0; col < width; col++)
                    {
                        // Check if this is the same as the last pixel. If so use that value
                        // rather than calculating it again. This is an inexpensive optimisation.
                        if (Marshal.ReadInt32(previousPixel) != Marshal.ReadInt32(sourcePixel))
                        {
                            // Quantize the pixel
                            pixelValue = this.QuantizePixel(new Color32(sourcePixel));

                            // And setup the previous pointer
                            previousPixel = sourcePixel;
                        }

                        // And set the pixel in the output
                        Marshal.WriteByte(destinationPixel, pixelValue);

                        sourcePixel = (IntPtr)((long)sourcePixel + this.pixelSize);
                        destinationPixel = (IntPtr)((long)destinationPixel + 1);
                    }

                    // Add the stride to the source row
                    sourceRow = (IntPtr)((long)sourceRow + sourceData.Stride);

                    // And to the destination row
                    destinationRow = (IntPtr)((long)destinationRow + outputData.Stride);
                }
            }
            finally
            {
                // Ensure that I unlock the output bits
                output.UnlockBits(outputData);
            }
        }
    }
}