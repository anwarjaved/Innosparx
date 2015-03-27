using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YahooSDK;

namespace YahooSDK.Contacts
{
    public class YahooDocument
    {
        [JsonProperty("contacts")]
        public ContactList ContactList { get; set; }
    }
}
