using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK.Places
{
    using Newtonsoft.Json;

    public class PredictionResult
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the  human-readable name for the returned result.
        /// </summary>
        ///
        /// <value>
        ///     The description.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("description")]
        public string Description { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the identifier of the reference.
        /// </summary>
        ///
        /// <value>
        ///     A unique token that you can use to retrieve additional information about this place in a Place Details request.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        [JsonProperty("reference")]
        public string ReferenceID { get; set; }


        [JsonProperty("id")]
        public string ID { get; set; }

    }
}
