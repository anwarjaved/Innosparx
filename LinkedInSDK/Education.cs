using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedInSDK
{
    using Newtonsoft.Json;

    public class Education
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "schoolName")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "fieldOfStudy")]
        public string FieldofStudy { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public DateInfo StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public DateInfo EndDate { get; set; }

        [JsonProperty(PropertyName = "degree")]
        public string Degree { get; set; }

        [JsonProperty(PropertyName = "activities")]
        public string Activities { get; set; }

        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
    }
}
