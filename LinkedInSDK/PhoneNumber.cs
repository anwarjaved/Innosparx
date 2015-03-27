using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace LinkedInSDK
{
    public class PhoneNumber
    {
        [JsonProperty(PropertyName = "phoneNumber")]
        public string Number { get; set; }
       
        [JsonProperty(PropertyName = "phoneType")]
        public string Type { get; set; }
    }
}
