using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedInSDK
{
    using Framework.Rest.OAuth;

    using Newtonsoft.Json;

    public class Profile : OAuth2BaseResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }

        [JsonProperty(PropertyName = "emailAddress")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "mainAddress")]
        public string MainAddress { get; set; }
        
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "headline")]
        public string HeadLine { get; set; }

        [JsonProperty(PropertyName = "phoneNumbers")]
        public PhoneNumbers PhoneNumbers { get; set; }

        [JsonProperty(PropertyName = "educations")]
        public Educations Educations { get; set; }

        [JsonProperty(PropertyName = "skills")]
        public Skills Skills { get; set; }


        [JsonProperty(PropertyName = "publications")]
        public Publications Publications { get; set; }

        [JsonProperty(PropertyName = "positions")]
        public Positions Positions { get; set; }

        /// <summary>
        /// the # of connections the member has.
        /// </summary>
        [JsonProperty(PropertyName = "numConnections")]
        public int NoOfConnections { get; set; }

    }
}
