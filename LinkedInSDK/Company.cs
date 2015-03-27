using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedInSDK
{
    using Newtonsoft.Json;

    public class Company
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "industry")]
        public string Industry { get; set; }

        [JsonProperty(PropertyName = "size")]
        public string Size { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

    }
}
