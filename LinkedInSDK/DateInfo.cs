using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedInSDK
{
    using Newtonsoft.Json;

    public class DateInfo
    {
        [JsonProperty(PropertyName = "day")]
        public int Day { get; set; }

        [JsonProperty(PropertyName = "month")]
        public int Month { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }
    }
}
