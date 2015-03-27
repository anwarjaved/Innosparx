namespace Framework.Rest.OAuth
{
    using System;
    using System.Collections.Specialized;

    using Framework.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    /// OAuth  AccessToken.
    /// </summary>
    /// <author>Anwar</author>
    /// <datetime>3/26/2011 3:24 PM</datetime>
    public class OAuth1TokenCredential : ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth1TokenCredential"/> class.
        /// </summary>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 3:34 PM</datetime>
        public OAuth1TokenCredential()
        {
            this.Response = new RestResponse();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth1TokenCredential"/> class.
        /// </summary>
        public OAuth1TokenCredential(string token, string secret) : this()
        {
            Token = token;
            Secret = secret;
        }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 3:25 PM</datetime>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the token secret.
        /// </summary>
        /// <value>The token secret.</value>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 3:25 PM</datetime>
        public string Secret { get; set; }

        public long ExpiresIn { get; set; }

        public string SessionHandle { get; set; }

        [JsonIgnore]
        public bool Success
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Token) && !string.IsNullOrWhiteSpace(this.Secret);
            }
        }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        /// <value>The response.</value>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 2:49 AM</datetime>
        public RestResponse Response { get; protected internal set; }

        string ISerializable.Serialize()
        {
            return Serialize();
        }

        protected virtual string Serialize()
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add(RestConstants.OAuthToken, this.Token);
            nvc.Add(RestConstants.OAuthTokenSecret, this.Secret);

            if (this.ExpiresIn > 0)
            {
                nvc.Add(RestConstants.OAuthExpiresIn, this.ExpiresIn.ToString());
            }

            return nvc.ToQueryString();
        }

        void ISerializable.Deserialize(string value)
        {
            Deserialize(value);
        }

        protected virtual void Deserialize(string value)
        {
            this.SessionHandle = QueryParameter.ParseQuerystringParameter(RestConstants.OAuthSessionHandle, value);
            this.Token = QueryParameter.ParseQuerystringParameter(RestConstants.OAuthToken, value);
            this.Secret = QueryParameter.ParseQuerystringParameter(RestConstants.OAuthTokenSecret, value);

            string expiresIn = QueryParameter.ParseQuerystringParameter(RestConstants.OAuthExpiresIn, value);

            if (!string.IsNullOrEmpty(expiresIn))
            {
                this.ExpiresIn = Convert.ToInt64(expiresIn);
            }
            else
            {
                expiresIn = QueryParameter.ParseQuerystringParameter(RestConstants.AuthExpiresIn, value);
                if (!string.IsNullOrEmpty(expiresIn))
                {
                    this.ExpiresIn = (long)(new DateTime(Convert.ToInt64(expiresIn)) - DateTime.UtcNow).TotalSeconds;
                }
            }
        }
    }
}
