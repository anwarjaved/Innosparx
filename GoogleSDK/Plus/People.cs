using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoogleSDK.Plus
{
    public class People
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("displayName")]
        public string FullName { get; set; }

        [JsonProperty("name")]
        public PeopleName Name { get; set; }
    }
}
