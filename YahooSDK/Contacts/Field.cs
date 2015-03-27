using System;
using System.Collections.Generic;
using Framework.Serialization.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YahooSDK.Contacts
{
    using Framework;

    public class Field
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
        ///     Gets or sets the Field ID.
        /// </summary>
        ///
        /// <value>
        ///     The Field ID.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("id")]
        public int ID { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the Field Type.
        /// </summary>
        ///
        /// <value>
        ///     The Field Type.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public FieldType Type { get; set; }

        [JsonProperty("value")]
        public dynamic Value { get; set; }
    }
}
