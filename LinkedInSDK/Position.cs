using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedInSDK
{
    using Newtonsoft.Json;

    public class Position
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "company")]
        public Company Company { get; set; }

        [JsonProperty(PropertyName = "isCurrent")]
        public bool IsCurrent { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public DateInfo StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateInfo EndDate { get; set; }
        
    }
}
