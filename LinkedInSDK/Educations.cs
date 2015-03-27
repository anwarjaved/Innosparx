using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedInSDK
{
    using Newtonsoft.Json;

    public class Educations
    {
        [JsonProperty(PropertyName = "_total")]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "values")]
        public Education[] Items { get; set; }
    }
}
