using System.Collections.Generic;

namespace GoogleSDK.Places
{
 
    using Framework.Serialization.Json.Converters;

    using Newtonsoft.Json;

    public class AutocompletePlaceResponse
    {
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StatusCode Status { get; set; }

        [JsonProperty("predictions")]
        public IList<PredictionResult> Results { get; set; }

    }
}
