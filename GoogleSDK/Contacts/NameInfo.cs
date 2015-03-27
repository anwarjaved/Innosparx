using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK.Contacts
{
    using Newtonsoft.Json;

    public class NameInfo
    {
        [JsonProperty("gd$fullName")]
        public StringInfo FullName { get; set; }

        [JsonProperty("gd$givenName")]
        public StringInfo GivenName { get; set; }

        [JsonProperty("gd$familyName")]
        public StringInfo FamilyName { get; set; }
    }
}
