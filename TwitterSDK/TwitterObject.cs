using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSDK
{
    using System.Runtime.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// Represents Twitter Object.
    /// </summary>
    /// <author>Anwar</author>
    /// <datetime>3/26/2011 4:55 PM</datetime>
    public abstract class TwitterObject
    {
        [JsonProperty("errors")]
        public List<TwitterError> Errors { get; set; }
    }
}
