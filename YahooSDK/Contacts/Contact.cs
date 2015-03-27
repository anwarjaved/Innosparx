using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YahooSDK.Contacts
{

    public class Contact
    {
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
        ///     Gets or sets a value indicating whether Indicates whether there is a connection with 
        ///     the user specified by the {guid} in the call operation.
        /// </summary>
        ///
        /// <value>
        ///     true there is a connection with the user, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("isConnection")]
        public bool IsConnection { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets The Contact's Id. This is the {cid} variable required in some resource endpoints.
        /// </summary>
        ///
        /// <value>
        ///     The The Contact's Id.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("id")]
        public int ID { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a collection of Field Objects.
        /// </summary>
        ///
        /// <value>
        ///     A collection of Field Objects.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("fields")]
        public ICollection<Field> Fields { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets A collection of Category Objects.
        /// </summary>
        ///
        /// <value>
        ///    A collection of Category Objects.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("categories")]
        public ICollection<Category> Categories { get; set; }
    }
}
