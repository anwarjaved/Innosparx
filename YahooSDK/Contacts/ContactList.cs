using System.Collections.Generic;
using Newtonsoft.Json;

namespace YahooSDK.Contacts
{

    public class ContactList
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the Index of the first contact returned.
        /// </summary>
        ///
        /// <value>
        ///     The Index of the first contact returned.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("start")]
        public int Start { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the Number of contacts returned.
        /// </summary>
        ///
        /// <value>
        ///     The Number of contacts returned.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("count")]
        public int Count { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the Total number of contacts available.
        /// </summary>
        ///
        /// <value>
        ///     The Total number of contacts available.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("total")]
        public int Total { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets A link to the actual resource..
        /// </summary>
        ///
        /// <value>
        ///     A link to the actual resource..
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
        ///     Gets or sets A collection of ContactList Objects.
        /// </summary>
        ///
        /// <value>
        ///    A collection of ContactList Objects.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("contact")]
        public ICollection<Contact> Items { get; set; }
    }
}
