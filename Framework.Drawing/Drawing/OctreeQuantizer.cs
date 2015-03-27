// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OctreeQuantizer.cs" company="Luce & Morker">
//   Copyright (c) www.lucemorker.com All rights reserved.
// </copyright>
// <summary>
//   Quantize using an Octree.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Framework.Drawing
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Imaging;

    /// <summary>
    /// Quantize using an Octree.
    /// </summary>
    public class OctreeQuantizer : Quantizer
    {
        /// <summary>
        /// Maximum allowed color depth.
        /// </summary>
        private readonly int maxColors;

        /// <summary>
        /// Stores the tree.
        /// </summary>
        private readonly Octree octree;

        /// <summary>
        /// Initializes a new instance of the <see cref="OctreeQuantizer"/> class. 
        /// Construct the octree quantizer.
        /// </summary>
        /// <remarks>
        /// The Octree quantizer is a two pass algorithm. The initial pass sets up the octree,
        /// the second pass quantizes a color based on the nodes in the tree.
        /// </remarks>
        /// <param name="maxColors">
        /// The maximum number of colors to return.
        /// </param>
        /// <param name="maxColorBits">
        /// The number of significant bits.
        /// </param>
        public OctreeQuantizer(int maxColors, int maxColorBits)
            : base(false)
        {
            if (maxColors > 255)
            {
                throw new ArgumentOutOfRangeException("maxColors", maxColors, "The number of colors should be less than 256");
            }

            if ((maxColorBits < 1) | (maxColorBits > 8))
            {
                throw new ArgumentOutOfRangeException("maxColorBits", maxColorBits, "This should be between 1 and 8");
            }

            // Construct the octree
            this.octree = new Octree(maxColorBits);
            this.maxColors = maxColors;
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
        protected override System.Drawing.Imaging.ColorPalette GetPalette(System.Drawing.Imaging.ColorPalette original)
        {
            // First off convert the octree to maxColors colors
            ArrayList palette = this.octree.Palletize(this.maxColors - 1);

            // Then convert the palette based on those colors
            for (int index = 0; index < palette.Count; index++)
            {
                original.Entries[index] = (Color)palette[index];
            }

            // Add the transparent color
            original.Entries[this.maxColors] = Color.FromArgb(0, 0, 0, 0);

            return original;
        }

        /// <summary>
        /// Process the pixel in the first pass of the algorithm.
        /// </summary>
        /// <param name="pixel">
        /// The pixel to quantize.
        /// </param>
        /// <remarks>
        /// This function need only be overridden if your quantize algorithm needs two passes,
        /// such as an Octree quantizer.
        /// </remarks>
        protected override void InitialQuantizePixel(Color32 pixel)
        {
            // Add the color to the octree
            this.octree.AddColor(pixel);
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
        protected override byte QuantizePixel(Color32 pixel)
        {
            var paletteIndex = (byte)this.maxColors; // The color at [maxColors] is set to transparent

            // Get the palette index if this non-transparent
            if (pixel.Alpha > 0)
            {
                paletteIndex = (byte)this.octree.GetPaletteIndex(pixel);
            }

            return paletteIndex;
        }

        /// <summary>
        /// Class which does the actual quantization.
        /// </summary>
        private class Octree
        {
            /// <summary>
            /// Mask used when getting the appropriate pixels for a given node.
            /// </summary>
            private static readonly int[] mask = new[] { 0x80, 0x40, 0x20, 0x10, 0x08, 0x04, 0x02, 0x01 };

            /// <summary>
            /// Maximum number of significant bits in the image.
            /// </summary>
            private readonly int maxColorBits;

            /// <summary>
            /// The root of the octree.
            /// </summary>
            private readonly OctreeNode root;

            /// <summary>
            /// Cache the previous color quantized.
            /// </summary>
            private int previousColor;

            /// <summary>
            /// Store the last node quantized.
            /// </summary>
            private OctreeNode previousNode;

            /// <summary>
            /// Initializes a new instance of the <see cref="Octree"/> class. 
            /// Construct the octree.
            /// </summary>
            /// <param name="maxColorBits">
            /// The maximum number of significant bits in the image.
            /// </param>
            public Octree(int maxColorBits)
            {
                this.maxColorBits = maxColorBits;
                this.Leaves = 0;
                this.ReducibleNodes = new OctreeNode[9];
                this.root = new OctreeNode(0, this.maxColorBits, this);
                this.previousColor = 0;
                this.previousNode = null;
            }

            /// <summary>
            /// Gets or sets the number of leaves in the tree.
            /// </summary>
            /// <value>
            /// The leaves.
            /// </value>
            private int Leaves { get; set; }

            private OctreeNode[] ReducibleNodes { get; set; }

            /// <summary>
            /// Add a given color value to the octree.
            /// </summary>
            /// <param name="pixel">The pixel.</param>
            public void AddColor(Color32 pixel)
            {
                // Check if this request is for the same color as the last
                if (this.previousColor == pixel.ARGB)
                {
                    // If so, check if I have a previous node setup. This will only ocurr if the first color in the image
                    // happens to be black, with an alpha component of zero.
                    if (null == this.previousNode)
                    {
                        this.previousColor = pixel.ARGB;
                        this.root.AddColor(pixel, this.maxColorBits, 0, this);
                    }
                    else
                    {
                        // Just update the previous node
                        this.previousNode.Increment(pixel);
                    }
                }
                else
                {
                    this.previousColor = pixel.ARGB;
                    this.root.AddColor(pixel, this.maxColorBits, 0, this);
                }
            }

            /// <summary>
            /// Get the palette index for the passed color.
            /// </summary>
            /// <param name="pixel">The pixel.</param>
            /// <returns>The get palette index.</returns>
            public int GetPaletteIndex(Color32 pixel)
            {
                return this.root.GetPaletteIndex(pixel, 0);
            }

            /// <summary>
            /// Convert the nodes in the octree to a palette with a maximum of colorCount colors.
            /// </summary>
            /// <param name="colorCount">The maximum number of colors.</param>
            /// <returns>An arraylist with the palettized colors.</returns>
            public ArrayList Palletize(int colorCount)
            {
                while (this.Leaves > colorCount)
                {
                    this.Reduce();
                }

                // Now palettize the nodes
                var palette = new ArrayList(this.Leaves);
                int paletteIndex = 0;
                this.root.ConstructPalette(palette, ref paletteIndex);

                // And return the palette
                return palette;
            }

            /// <summary>
            /// Reduce the depth of the tree.
            /// </summary>
            private void Reduce()
            {
                int index;

                // Find the deepest level containing at least one reducible node
                for (index = this.maxColorBits - 1; (index > 0) && (null == this.ReducibleNodes[index]); index--)
                {
                }

                // Reduce the node most recently added to the list at level 'index'
                OctreeNode node = this.ReducibleNodes[index];
                this.ReducibleNodes[index] = node.NextReducible;

                // Decrement the leaf count after reducing the node
                this.Leaves -= node.Reduce();

                // And just in case I've reduced the last color to be added, and the next color to
                // be added is the same, invalidate the previousNode...
                this.previousNode = null;
            }

            /// <summary>
            /// Keep track of the previous node that was quantized.
            /// </summary>
            /// <param name="node">The node last quantized.</param>
            private void TrackPrevious(OctreeNode node)
            {
                this.previousNode = node;
            }

            /// <summary>
            /// Class which encapsulates each node in the tree.
            /// </summary>
            protected class OctreeNode
            {
                /// <summary>
                /// Blue component.
                /// </summary>
                private int blue;

                /// <summary>
                /// Green Component.
                /// </summary>
                private int green;

                /// <summary>
                /// Flag indicating that this is a leaf node.
                /// </summary>
                private bool leaf;

                /// <summary>
                /// The index of this node in the palette.
                /// </summary>
                private int paletteIndex;

                /// <summary>
                /// Number of pixels in this node.
                /// </summary>
                private int pixelCount;

                /// <summary>
                /// Red component.
                /// </summary>
                private int red;

                /// <summary>
                /// Initializes a new instance of the <see cref="OctreeNode"/> class. 
                /// Construct the node.
                /// </summary>
                /// <param name="level">
                /// The level in the tree = 0 - 7.
                /// </param>
                /// <param name="colorBits">
                /// The number of significant color bits in the image.
                /// </param>
                /// <param name="octree">
                /// The tree to which this node belongs.
                /// </param>
                public OctreeNode(int level, int colorBits, Octree octree)
                {
                    // Construct the new node
                    this.leaf = level == colorBits;

                    this.red = this.green = this.blue = 0;
                    this.pixelCount = 0;

                    // If a leaf, increment the leaf count
                    if (this.leaf)
                    {
                        octree.Leaves++;
                        this.NextReducible = null;
                        this.Children = null;
                    }
                    else
                    {
                        // Otherwise add this to the reducible nodes
                        this.NextReducible = octree.ReducibleNodes[level];
                        octree.ReducibleNodes[level] = this;
                        this.Children = new OctreeNode[8];
                    }
                }

                /// <summary>
                /// Gets the next reducible node.
                /// </summary>
                /// <value>
                /// The next reducible.
                /// </value>
                public OctreeNode NextReducible { get; private set; }

                private OctreeNode[] Children { get; set; }

                /// <summary>
                /// Add a color into the tree.
                /// </summary>
                /// <param name="pixel">
                /// The color.
                /// </param>
                /// <param name="colorBits">
                /// The number of significant color bits.
                /// </param>
                /// <param name="level">
                /// The level in the tree.
                /// </param>
                /// <param name="octree">
                /// The tree to which this node belongs.
                /// </param>
                public void AddColor(Color32 pixel, int colorBits, int level, Octree octree)
                {
                    // Update the color information if this is a leaf
                    if (this.leaf)
                    {
                        this.Increment(pixel);

                        // Setup the previous node
                        octree.TrackPrevious(this);
                    }
                    else
                    {
                        // Go to the next level down in the tree
                        int shift = 7 - level;
                        int index = ((pixel.Red & mask[level]) >> (shift - 2)) | ((pixel.Green & mask[level]) >> (shift - 1)) |
                                    ((pixel.Blue & mask[level]) >> shift);

                        OctreeNode child = this.Children[index];

                        if (null == child)
                        {
                            // Create a new child node & store in the array
                            child = new OctreeNode(level + 1, colorBits, octree);
                            this.Children[index] = child;
                        }

                        // Add the color to the child node
                        child.AddColor(pixel, colorBits, level + 1, octree);
                    }
                }

                /// <summary>
                /// Traverse the tree, building up the color palette.
                /// </summary>
                /// <param name="palette">
                /// The palette.
                /// </param>
                /// <param name="newPaletteIndex">
                /// The current palette index.
                /// </param>
                public void ConstructPalette(IList palette, ref int newPaletteIndex)
                {
                    if (this.leaf)
                    {
                        // Consume the next palette index
                        this.paletteIndex = newPaletteIndex++;

                        // And set the color of the palette entry
                        palette.Add(
                          Color.FromArgb(
                            this.red / this.pixelCount, this.green / this.pixelCount, this.blue / this.pixelCount));
                    }
                    else
                    {
                        // Loop through children looking for leaves
                        for (int index = 0; index < 8; index++)
                        {
                            if (null != this.Children[index])
                            {
                                this.Children[index].ConstructPalette(palette, ref newPaletteIndex);
                            }
                        }
                    }
                }

                /// <summary>
                /// Return the palette index for the passed color.
                /// </summary>
                /// <param name="pixel">
                /// The pixel.
                /// </param>
                /// <param name="level">
                /// The level.
                /// </param>
                /// <returns>
                /// The get palette index.
                /// </returns>
                public int GetPaletteIndex(Color32 pixel, int level)
                {
                    int currentPaletteIndex = this.paletteIndex;

                    if (!this.leaf)
                    {
                        int shift = 7 - level;
                        int index = ((pixel.Red & mask[level]) >> (shift - 2)) | ((pixel.Green & mask[level]) >> (shift - 1)) |
                                    ((pixel.Blue & mask[level]) >> shift);

                        if (null != this.Children[index])
                        {
                            currentPaletteIndex = this.Children[index].GetPaletteIndex(pixel, level + 1);
                        }
                        else
                        {
                            throw new Exception("Didn't expect this!");
                        }
                    }

                    return currentPaletteIndex;
                }

                /// <summary>
                /// Increment the pixel count and add to the color information.
                /// </summary>
                /// <param name="pixel">
                /// The pixel.
                /// </param>
                public void Increment(Color32 pixel)
                {
                    this.pixelCount++;
                    this.red += pixel.Red;
                    this.green += pixel.Green;
                    this.blue += pixel.Blue;
                }

                /// <summary>
                /// Reduce this node by removing all of its children.
                /// </summary>
                /// <returns>
                /// The number of leaves removed.
                /// </returns>
                public int Reduce()
                {
                    this.red = this.green = this.blue = 0;
                    int childrenIndex = 0;

                    // Loop through all children and add their information to this node
                    for (int index = 0; index < 8; index++)
                    {
                        if (null != this.Children[index])
                        {
                            this.red += this.Children[index].red;
                            this.green += this.Children[index].green;
                            this.blue += this.Children[index].blue;
                            this.pixelCount += this.Children[index].pixelCount;
                            ++childrenIndex;
                            this.Children[index] = null;
                        }
                    }

                    // Now change this to a leaf node
                    this.leaf = true;

                    // Return the number of nodes to decrement the leaf count by
                    return childrenIndex - 1;
                }
            }
        }
    }
}