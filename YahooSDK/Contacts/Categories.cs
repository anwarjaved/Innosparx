using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YahooSDK.Contacts
{
    using Newtonsoft.Json;

    public class Categories
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the Index of the first category returned.
        /// </summary>
        ///
        /// <value>
        ///     The Index of the first category returned..
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("start")]
        public int Start { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the Number of categories returned.
        /// </summary>
        ///
        /// <value>
        ///     The Number of categories returned.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("count")]
        public int Count { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the Total number of categories available.
        /// </summary>
        ///
        /// <value>
        ///     The Total number of categories available.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("total")]
        public int Total { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a link to the actual resource.
        /// </summary>
        ///
        /// <value>
        ///     A link to the actual resource.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("uri")]
        public string Uri { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     This is a system-generated response indicating the date on which the object was created. 
        ///     The date is in RFC 3339 format. Example: Wed, 13 Aug 2008 21:24:09 GMT
        /// </summary>
        ///
        /// <value>
        ///     The date on which the object was created.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("created")]
        public string Created { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets This is a system-generated response indicating the date that the object 
        ///     was modified. The date is in RFC 3339 format. Example: Wed, 13 Aug 2008 21:24:09 GMT.
        /// </summary>
        ///
        /// <value>
        ///     The date on which the object was updated.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("updated")]
        public string Updated { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets A collection of Category Objects.
        /// </summary>
        ///
        /// <value>
        ///    A collection of Category Objects.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("category")]
        public ICollection<Category> Items { get; set; }
    }
}
