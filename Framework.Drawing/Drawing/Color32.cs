// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Color32.cs" company="Luce & Morker">
//   Copyright (c) www.lucemorker.com All rights reserved.
// </copyright>
// <summary>
//   Struct that defines a 32 bpp colour.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Drawing
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// Struct that defines a 32 bpp colour.
    /// </summary>
    /// <remarks>
    /// This struct is used to read data from a 32 bits per pixel image
    /// in memory, and is ordered in this manner as this is the way that
    /// the data is layed out in memory.
    /// </remarks>
    [StructLayout(LayoutKind.Explicit)]
    public struct Color32
    {
        /// <summary>
        /// Holds the blue component of the colour.
        /// </summary>
        [FieldOffset(0)]
        public byte Blue;

        /// <summary>
        /// Holds the green component of the colour.
        /// </summary>
        [FieldOffset(1)]
        public byte Green;

        /// <summary>
        /// Holds the red component of the colour.
        /// </summary>
        [FieldOffset(2)]
        public byte Red;

        /// <summary>
        /// Holds the alpha component of the colour.
        /// </summary>
        [FieldOffset(3)]
        public byte Alpha;

        /// <summary>
        /// Permits the color32 to be treated as an int32.
        /// </summary>
        [FieldOffset(0)]
        public int ARGB;

        /// <summary>
        /// Initializes a new instance of the <see cref="Color32"/> struct.
        /// </summary>
        /// <param name="sourcePixel">
        /// The p source pixel.
        /// </param>
        
        public Color32(IntPtr sourcePixel)
        {
            this = (Color32)Marshal.PtrToStructure(sourcePixel, typeof(Color32));
        }

        /// <summary>
        /// Gets the color for this Color32 object.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public Color Color
        {
            get
            {
                return Color.FromArgb(this.Alpha, this.Red, this.Green, this.Blue);
            }
        }
    }
}