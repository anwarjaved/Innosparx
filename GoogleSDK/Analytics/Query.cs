using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoogleSDK.Analytics
{

    /// <summary>
    /// This object contains all the values passed as parameters to the query.
    /// </summary>
    public class Query
    {

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>The start date.</value>
        [JsonProperty("start-date")]
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>The end date.</value>
        [JsonProperty("end-date")]
        public string EndDate { get; set; }

        /// <summary>
        /// Unique table ID.
        /// </summary>
        [JsonProperty("ids")]
        public string IDs { get; set; }

        /// <summary>
        /// Gets or sets the List of analytics dimensions.
        /// </summary>
        /// <value>The List of analytics dimensions.</value>
        [JsonProperty("dimensions")]
        public string[] Dimensions { get; set; }


        /// <summary>
        /// Gets or sets List of analytics metrics.
        /// </summary>
        /// <value>The List of analytics metrics.</value>
        [JsonProperty("metrics")]
        public string[] Metrics { get; set; }

        /// <summary>
        /// Gets or sets the desired sampling level.
        /// </summary>
        /// <value>The desired sampling level.</value>
        [JsonProperty("samplingLevel")]
        public string SamplingLevel { get; set; }

        /// <summary>
        /// Comma-separated list of metric or dimension filters.
        /// </summary>
        /// <value>The filters.</value>
        [JsonProperty("filters")]
        public string Filters { get; set; }


        /// <summary>
        /// Selector specifying a subset of fields to include in the response.
        /// </summary>
        /// <value>The filters.</value>
        [JsonProperty("fields")]
        public string Fields { get; set; }

        /// <summary>
        /// Returns response with indentations and line breaks. Default false.
        /// </summary>
        /// <value><see langword="true" /> if [pretty print]; otherwise, <see langword="false" />.</value>
        [JsonProperty("prettyPrint")]
        public bool PrettyPrint { get; set; }


        /// <summary>
        /// The first row of data to retrieve, starting at 1. Use this parameter as a pagination mechanism along with the max-results parameter.
        /// </summary>
        /// <value>The start index.</value>
        [JsonProperty("start-index")]
        public int StartIndex { get; set; }

        /// <summary>
        /// The maximum number of rows to include in the response.
        /// </summary>
        /// <value>The maximum results.</value>
        [JsonProperty("max-results")]
        public int MaxResults { get; set; }

        /// <summary>
        /// Specifies IP address of the end user for whom the API call is being made.
        /// </summary>
        /// <value>The user ip.</value>
        [JsonProperty("userIp")]
        public string UserIPAddress { get; set; }

        /// <summary>
        /// Alternative to userIp in cases when the user's IP address is unknown.
        /// </summary>
        /// <value>The quota user.</value>
        [JsonProperty("quotaUser")]
        public string QuotaUser { get; set; }

        /// <summary>
        /// Analytics advanced segment.
        /// </summary>
        /// <value>The segment.</value>
        [JsonProperty("segment")]
        public string Segment { get; set; }
    }

}
