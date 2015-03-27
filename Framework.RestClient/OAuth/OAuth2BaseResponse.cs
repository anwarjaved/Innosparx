namespace Framework.Rest.OAuth
{
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.Serialization;


    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;


    public abstract class OAuth2BaseResponse
    {
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorType { get; set; }

        [JsonProperty("requestId")]
        public string RequestID { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("status")]
        public HttpStatusCode? Status { get; set; }



        protected virtual string Serialize()
        {
            return null;

        }

        [JsonExtensionData]
        internal IDictionary<string, JToken> AdditionalData { get; set; }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            if (this.AdditionalData != null)
            {
                if (this.AdditionalData.ContainsKey("code"))
                {
                    this.ErrorCode = (string)this.AdditionalData["code"];

                }

                if (this.AdditionalData.ContainsKey("error"))
                {
                    JToken errorToken = this.AdditionalData["error"];

                    if (errorToken.Type == JTokenType.String)
                    {
                        string msg = errorToken.ToString();
                        this.ErrorMessage = msg;
                    }
                    else
                    {
                        if (errorToken["code"] != null)
                        {
                            this.ErrorCode = (string)errorToken["code"];
                        }

                        if (errorToken["message"] != null)
                        {
                            this.ErrorMessage = (string)errorToken["message"];
                        }
                    }
                    
                }

                if (this.AdditionalData.ContainsKey("errorCode"))
                {
                    this.ErrorCode = (string)this.AdditionalData["errorCode"];

                }

                if (this.AdditionalData.ContainsKey("message"))
                {
                    this.ErrorMessage = (string)this.AdditionalData["message"];

                }

                if (this.AdditionalData.ContainsKey("error_description"))
                {
                    this.ErrorMessage = (string)this.AdditionalData["error_description"];

                }

                if (this.AdditionalData.ContainsKey("error_uri"))
                {
                    this.ErrorType = (string)this.AdditionalData["error_uri"];
                }
            }

        }
    }
}
