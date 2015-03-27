using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterSDK
{
    using Newtonsoft.Json;

    public class TwitterFollowers : TwitterObject
    {
        [JsonProperty("ids")]
        public IEnumerable<string> Items { get; set; }

        [JsonIgnore]
        public bool HasCount
        {
            get
            {
                return this.Items != null && this.Items.Any();
            }
        }

        [JsonIgnore]
        public int Count
        {
            get
            {
                return this.Items != null ? this.Items.Count() : 0;
            }
        }
    }
}
