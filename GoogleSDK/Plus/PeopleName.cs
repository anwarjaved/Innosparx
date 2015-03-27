using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoogleSDK.Plus
{
    public class PeopleName
    {
        [JsonProperty("familyName")]
        public string LastName { get; set; }

        [JsonProperty("givenName")]
        public string FirstName { get; set; }
    }
}
