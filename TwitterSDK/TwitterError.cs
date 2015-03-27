using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSDK
{
    using System.Runtime.Serialization;

    using Newtonsoft.Json;

    public class TwitterError
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
