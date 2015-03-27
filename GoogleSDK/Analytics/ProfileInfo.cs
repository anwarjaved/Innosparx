using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoogleSDK.Analytics
{

    public class ProfileInfo
    {

        [JsonProperty("profileId")]
        public string ProfileId { get; set; }

        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("webPropertyId")]
        public string WebPropertyId { get; set; }

        [JsonProperty("internalWebPropertyId")]
        public string InternalWebPropertyId { get; set; }

        [JsonProperty("profileName")]
        public string ProfileName { get; set; }

        [JsonProperty("tableId")]
        public string TableId { get; set; }
    }

}
