﻿namespace Framework.Drawing
{
    /// <summary>
    /// Image Resize Modes.
    /// </summary>
    public enum ResizeMode
    {
        /// <summary>
        /// Fit mode maintains the aspect ratio of the original image while ensuring that the dimensions of the result
        /// do not exceed the maximum values for the resize transformation.
        /// </summary>
        Fit,

        /// <summary>
        /// Crop resizes the image and removes parts of it to ensure that the dimensions of the result are exactly 
        /// as specified by the transformation.
        /// </summary>
        Crop
    }
}