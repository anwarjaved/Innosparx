namespace Framework.Rest.OAuth
{
    using System;

    using Framework.Serialization;

    using Newtonsoft.Json;

    public class OAuth2TokenCredential : OAuth2BaseResponse, ISerializable
    {
        public OAuth2TokenCredential()
        {
            this.TokenType = RestConstants.OAuth2BearerToken;
        }

        [JsonProperty(RestConstants.OAuth2AccessToken)]
        public string Token { get; set; }

        [JsonProperty(RestConstants.OAuth2RefreshToken)]
        public string RefreshToken { get; set; }

        [JsonProperty(RestConstants.OAuth2ExpiresIn)]
        public long ExpiresIn { get; set; }

        [JsonProperty(RestConstants.OAuth2TokenType)]
        public string TokenType { get; set; }

        [JsonProperty(RestConstants.OAuth2State)]
        public string State { get; set; }

        [JsonIgnore]
        public bool Success
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Token))
                {
                    return string.IsNullOrWhiteSpace(this.ErrorMessage) && string.IsNullOrWhiteSpace(this.ErrorCode);
                }

                return false;
            }
        }

        string ISerializable.Serialize()
        {
            return null;
        }

        void ISerializable.Deserialize(string value)
        {
            this.Token = QueryParameter.ParseQuerystringParameter(RestConstants.OAuth2AccessToken, value);
            this.RefreshToken = QueryParameter.ParseQuerystringParameter(RestConstants.OAuth2RefreshToken, value);
            string tokenType = QueryParameter.ParseQuerystringParameter(RestConstants.OAuth2TokenType, value);

            if (!string.IsNullOrEmpty(tokenType))
            {
                this.TokenType = tokenType;
            }

            this.State = QueryParameter.ParseQuerystringParameter(RestConstants.OAuth2State, value);

            string expiresIn = QueryParameter.ParseQuerystringParameter(RestConstants.OAuthExpiresIn, value);

            if (!string.IsNullOrEmpty(expiresIn))
            {
                this.ExpiresIn = Convert.ToInt64(expiresIn);
            }
            else
            {
                expiresIn = QueryParameter.ParseQuerystringParameter(RestConstants.OAuth2Expires, value);

                if (!string.IsNullOrEmpty(expiresIn))
                {
                    this.ExpiresIn = Convert.ToInt64(expiresIn);
                }
            }
        }
    }
}
