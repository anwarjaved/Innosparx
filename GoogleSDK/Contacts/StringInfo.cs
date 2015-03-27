using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK.Contacts
{
    using Newtonsoft.Json;

    public class StringInfo
    {
        [JsonProperty("$t")]
        public string Value { get; set; }
    }
}
