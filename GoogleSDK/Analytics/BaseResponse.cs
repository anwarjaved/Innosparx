using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoogleSDK.Analytics
{

    public abstract class BaseResponse<T> where T : class, new()
    {
        /// <summary>
        /// Resource type. Value is "analytics#gaData".
        /// </summary>
        /// <value>The kind.</value>
        [JsonProperty("kind")]
        public string Kind { get; set; }

        /// <summary>
        /// An ID for this data response.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// This object contains all the values passed as parameters to the query. 
        /// </summary>
        /// <value>The query.</value>
        [JsonProperty("query")]
        public Query Query { get; set; }

        /// <summary>
        /// Gets or sets the items per page.
        /// </summary>
        /// <value>The items per page.</value>
        [JsonProperty("itemsPerPage")]
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// Gets or sets the total results.
        /// </summary>
        /// <value>The total results.</value>
        [JsonProperty("totalResults")]
        public int TotalResults { get; set; }

        /// <summary>
        /// Gets or sets the self link.
        /// </summary>
        /// <value>The self link.</value>
        [JsonProperty("selfLink")]
        public string SelfLink { get; set; }

        /// <summary>
        /// Gets or sets the profile information.
        /// </summary>
        /// <value>The profile information.</value>
        [JsonProperty("profileInfo")]
        public ProfileInfo ProfileInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [contains sampled data].
        /// </summary>
        /// <value><see langword="true" /> if [contains sampled data]; otherwise, <see langword="false" />.</value>
        [JsonProperty("containsSampledData")]
        public bool ContainsSampledData { get; set; }

        /// <summary>
        /// Gets or sets the column headers.
        /// </summary>
        /// <value>The column headers.</value>
        [JsonProperty("columnHeaders")]
        public ColumnHeader[] ColumnHeaders { get; set; }

        [JsonProperty("totalsForAllResults")]
        public T TotalForAllResults { get; set; }

        [JsonProperty("rows")]
        public List<List<string>> Rows { get; set; }
    }

}
