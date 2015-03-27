using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Rest;

namespace GoogleSDK.Contacts
{
    using Newtonsoft.Json;

    public class Contact
    {
        [JsonProperty("gd$name")]
        public NameInfo Info { get; set; }

        [JsonProperty("gd$email")]
        public EmailInfo[] Emails { get; set; }
    }
}
