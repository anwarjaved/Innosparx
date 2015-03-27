using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK.Places
{
    using Framework.Serialization.Json.Converters;

    using Newtonsoft.Json;

    public class PlacesResponse
    {
        [JsonProperty("results")]
        public IList<PlaceResult> Results { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StatusCode Status { get; set; }

    }
}
