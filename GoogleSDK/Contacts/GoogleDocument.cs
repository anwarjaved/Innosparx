using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK.Contacts
{
    using Framework.Rest;
    using Framework.Rest.OAuth;

    using Newtonsoft.Json;

    public class GoogleDocument : OAuth2BaseResponse
    {
        [JsonProperty("feed")]
        public GoogleFeed Feed { get; set; }
    }
}
