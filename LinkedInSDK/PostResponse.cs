using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSDK
{
    using Newtonsoft.Json;

    /// <summary>
    ///  Response of Post.
    /// </summary>
    public class PostResponse : BaseResponse
    {
        [JsonProperty("update-key")]
        public string Key { get; set; }

        [JsonProperty("update-url")]
        public string Url { get; set; }
    }
}
