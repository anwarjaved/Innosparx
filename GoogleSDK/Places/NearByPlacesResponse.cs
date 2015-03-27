using System.Collections.Generic;

namespace GoogleSDK.Places
{
    using Framework.Serialization.Json.Converters;

    using Newtonsoft.Json;

    public class NearByPlacesResponse
    {
        [JsonProperty("results")]
        public IList<NearByPlaceResult> Results { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StatusCode Status { get; set; }

    }
}
