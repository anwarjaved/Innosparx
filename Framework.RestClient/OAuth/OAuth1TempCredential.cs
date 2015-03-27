namespace Framework.Rest.OAuth
{
    using System;
    using System.Collections.Specialized;

    using Framework.Serialization;

    /// <summary>
    /// OAuth Request Token.
    /// </summary>
    /// <author>Anwar</author>
    /// <datetime>3/26/2011 1:59 AM</datetime>
    public class OAuth1TempCredential : OAuth1Token, ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth1TempCredential"/> class.
        /// </summary>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 3:32 PM</datetime>
        public OAuth1TempCredential()
        {
            this.Response = new RestResponse();
        }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        /// <value>The response.</value>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 2:49 AM</datetime>
        public RestResponse Response { get; protected internal set; }

        protected internal virtual void ParseValues(string content)
        {

        }

        string ISerializable.Serialize()
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add(RestConstants.OAuthToken, this.Token);
            nvc.Add(RestConstants.OAuthTokenSecret, this.Secret);
            nvc.Add(RestConstants.OAuthCallbackConfirmed, this.OAuthCallbackConfirmed ? "true" : "false");

            if (this.ExpiresIn > 0)
            {
                nvc.Add(RestConstants.OAuthExpiresIn, this.ExpiresIn.ToString());
            }

            return nvc.ToQueryString();
        }

        void ISerializable.Deserialize(string value)
        {
            this.Token = QueryParameter.ParseQuerystringParameter(RestConstants.OAuthToken, value);
            this.Secret = QueryParameter.ParseQuerystringParameter(RestConstants.OAuthTokenSecret, value);
            this.AuthenticationUrl = QueryParameter.ParseQuerystringParameter(RestConstants.XOAuthRequestAuthUrl, value);

            if (QueryParameter.ParseQuerystringParameter(RestConstants.OAuthCallbackConfirmed, value) == "true")
            {
                this.OAuthCallbackConfirmed = true;
            }

            string expiresIn = QueryParameter.ParseQuerystringParameter(RestConstants.OAuthExpiresIn, value);

            if (!string.IsNullOrEmpty(expiresIn))
            {
                this.ExpiresIn = Convert.ToInt64(expiresIn);
            }
        }
    }
}
