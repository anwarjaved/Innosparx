using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedInSDK
{
    using Newtonsoft.Json;

    public class SkillInfo
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
