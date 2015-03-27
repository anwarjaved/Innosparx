using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK.Places
{
    using GoogleSDK.Common;

    using Newtonsoft.Json;

    public class AddressResult
    {
        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }
     
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("address_components")]
        public AddressDetails Details { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }
    }
}
