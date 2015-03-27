namespace Framework.Drawing
{
    using System.ComponentModel;

    /// <summary>
    /// An abstract ImageFilter class.
    /// </summary>
    public abstract class ImageFilter
    {
        /// <summary>
        /// Processes the image with current filter.
        /// </summary>
        /// <param name="image">The image to process.</param>
        /// <returns>Returns a Image after applying current filter.</returns>
        public abstract System.Drawing.Image Process(System.Drawing.Image image);

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Image Filter";
        }
    }
}