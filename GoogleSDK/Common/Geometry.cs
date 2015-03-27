using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK.Common
{
    using Newtonsoft.Json;

    public class Geometry
    {

        [JsonProperty("location")]
        public Location Location { get; set; }
    }
}
