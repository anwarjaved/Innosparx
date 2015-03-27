using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK.Contacts
{
    using Newtonsoft.Json;

    public class EmailInfo
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("primary")]
        public bool Primary { get; set; }
    }
}
