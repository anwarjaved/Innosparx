using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace LinkedInSDK
{
    public  class Publisher
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
