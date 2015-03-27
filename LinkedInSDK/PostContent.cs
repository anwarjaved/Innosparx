using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSDK
{
    using Newtonsoft.Json;

    internal class PostContent
    {
        /// <summary>
        /// Gets or sets the Title of shared document.
        /// </summary>
        /// <value>The Title of shared document.</value>
        /// <remarks> Max length is 200 characters.</remarks>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL for shared content.
        /// </summary>
        /// <value>The URL for shared content.</value>
        [JsonProperty("submitted-url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Description of shared content.
        /// </summary>
        /// <value>The Description of shared content.</value>
        /// <remarks>Max length of 256 characters.</remarks>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL for image of shared content.
        /// </summary>
        /// <value>The URL for image of shared content.</value>
        [JsonProperty("submitted-image-url")]
        public string ImageUrl { get; set; }
    }
}
