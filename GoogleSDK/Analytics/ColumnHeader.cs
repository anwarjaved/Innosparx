using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoogleSDK.Analytics
{

    public class ColumnHeader
    {

        /// <summary>
        /// Name of the dimension or metric.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Column type. Either "DIMENSION" or "METRIC".
        /// </summary>
        /// <value>The type of the column.</value>
        [JsonProperty("columnType")]
        public string ColumnType { get; set; }

        /// <summary>
        /// Data type. Dimension column headers have only STRING as data type. Metric column headers have data types for metric values such as INTEGER, FLOAT, CURRENCY etc.
        /// </summary>
        /// <value>The type of the data.</value>
        [JsonProperty("dataType")]
        public string DataType { get; set; }
    }

}
