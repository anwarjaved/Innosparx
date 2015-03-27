using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterSDK
{
    using Newtonsoft.Json;

    public class TwitterCredential : TwitterObject
    {
        [JsonProperty("id")]
        public string ID { get; set; }

    }
}
