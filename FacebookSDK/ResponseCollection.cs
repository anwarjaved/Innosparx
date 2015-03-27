using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace FacebookSDK
{
    public class ResponseCollection<T> where T : BaseResponse, new()
    {
        public ResponseCollection()
        {
            this.Items = new List<T>();
        }

        [JsonProperty("data")]
        public IEnumerable<T> Items { get; set; }

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
