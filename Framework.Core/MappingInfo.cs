using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Information about the mapping.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public sealed class MappingInfo
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the MappingInfo class.
        /// </summary>
        ///
        /// <param name="extension">
        ///     file extention.
        /// </param>
        /// <param name="text">
        ///     file type.
        /// </param>
        /// <param name="description">
        ///     file description.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public MappingInfo(string extension, string text, string description)
        {
            this.Extension = extension;
            this.Description = description;
            this.Text = text;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the extension.
        /// </summary>
        ///
        /// <value>
        ///     The extension.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Extension { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        ///
        /// <value>
        ///     The description.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Description { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the text.
        /// </summary>
        ///
        /// <value>
        ///     The text.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Text { get; private set; }
    }
}
