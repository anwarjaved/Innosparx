using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK.Places
{

    using Framework.Serialization.Json.Converters;

    using GoogleSDK.Common;

    using Newtonsoft.Json;

    public class AddressResponse
    {
        [JsonProperty("result")]
        public AddressResult Result { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StatusCode Status { get; set; }
    }
}
