using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedInSDK
{
    using Newtonsoft.Json;

    public class Skill
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "skill")]
        public SkillInfo Info { get; set; }
    }
}
