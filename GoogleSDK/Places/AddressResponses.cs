using System.Collections.Generic;

namespace GoogleSDK.Places
{
    using Framework.Serialization.Json.Converters;

    using Newtonsoft.Json;

    public class AddressResponses
    {
        [JsonProperty("results")]
        public IList<AddressResult> Results { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StatusCode Status { get; set; }
    }
}
