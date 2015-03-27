using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK.Places
{
    using GoogleSDK.Common;

    using Newtonsoft.Json;

    public class NearByPlaceResult
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("vicinity")]
        public string Address { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("reference")]
        public string ReferenceID { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

    }
}