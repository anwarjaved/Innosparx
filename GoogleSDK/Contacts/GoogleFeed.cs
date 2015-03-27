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

    public class GoogleFeed : OAuth2BaseResponse
    {
        [JsonProperty("entry")]
        public Contact[] Contacts { get; set; }
    }
}
