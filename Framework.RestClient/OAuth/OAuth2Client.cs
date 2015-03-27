namespace Framework.Rest.OAuth
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;


    public abstract class OAuth2Client : RestClient
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="OAuth2Client"/> class.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 10/16/2013 12:07 PM.
        /// </remarks>
        ///
        /// <param name="appID">
        ///     The consumer key.
        /// </param>
        /// <param name="appSecret">
        ///     The consumer secret.
        /// </param>
        /// <param name="tokenAccessType">
        ///     The type of the token access.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected OAuth2Client(string appID, string appSecret, OAuth2TokenAccessType tokenAccessType = OAuth2TokenAccessType.Header)
        {
            this.AppID = appID;
            this.AppSecret = appSecret;
            this.TokenAccessType = tokenAccessType;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the OAuth2Client class.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 10/16/2013 12:08 PM.
        /// </remarks>
        ///
        /// <param name="appID">
        ///     The consumer key.
        /// </param>
        /// <param name="appSecret">
        ///     The consumer secret.
        /// </param>
        /// <param name="credential">
        ///     The credential.
        /// </param>
        /// <param name="tokenAccessType">
        ///     The type of the token access.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected OAuth2Client(string appID, string appSecret, OAuth2TokenCredential credential, OAuth2TokenAccessType tokenAccessType = OAuth2TokenAccessType.Header)
        {
            this.AppID = appID;
            this.AppSecret = appSecret;
            this.Credential = credential;
            this.TokenAccessType = tokenAccessType;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the OAuth2Client class.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 10/16/2013 12:08 PM.
        /// </remarks>
        ///
        /// <param name="appID">
        ///     The consumer key.
        /// </param>
        /// <param name="appSecret">
        ///     The consumer secret.
        /// </param>
        /// <param name="token">
        ///     The token.
        /// </param>
        /// <param name="tokenAccessType">
        ///     The type of the token access.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected OAuth2Client(string appID, string appSecret, string token, OAuth2TokenAccessType tokenAccessType = OAuth2TokenAccessType.Header)
        {
            this.AppID = appID;
            this.AppSecret = appSecret;
            this.Credential = new OAuth2TokenCredential() { Token = token };
            this.TokenAccessType = tokenAccessType;
        }

        /// <summary>
        /// Gets the consumer key.
        /// </summary>
        /// <value>The consumer key.</value>
        public string AppID { get; private set; }

        /// <summary>
        /// Gets the consumer secret.
        /// </summary>
        /// <value>The consumer secret.</value>
        public string AppSecret { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the type of the token access.
        /// </summary>
        ///
        /// <value>
        ///     The type of the token access.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public OAuth2TokenAccessType TokenAccessType { get; private set; }

        public OAuth2TokenCredential Credential { get; protected set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds authorization URL.
        /// </summary>
        ///
        /// <param name="endPoint">
        ///     The end point.
        /// </param>
        /// <param name="redirectUrl">
        ///     URL of the redirect.
        /// </param>
        /// <param name="scope">
        ///     (optional) the scope.
        /// </param>
        /// <param name="responseType">
        ///     (optional) type of the response.
        /// </param>
        /// <param name="state">
        ///     (optional) the state.
        /// </param>
        /// <param name="parameters">
        ///     (optional) options for controlling the operation.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected string BuildAuthorizationUrl(string endPoint, string redirectUrl, string scope = "", OAuth2ResponseType responseType = OAuth2ResponseType.Code, string state = "", IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            UrlBuilder request = new UrlBuilder(endPoint);
            request.QueryString.Add(RestConstants.OAuth2ResponseType, responseType.ToDescription());
            request.QueryString.Add(RestConstants.OAuth2ClientID, this.AppID);
            request.QueryString.Add(RestConstants.OAuth2RedirectUri, redirectUrl);

            if (!string.IsNullOrWhiteSpace(scope))
            {
                request.QueryString.Add(RestConstants.OAuth2Scope, scope);
            }

            if (!string.IsNullOrWhiteSpace(state))
            {
                request.QueryString.Add(RestConstants.OAuth2State, state);
            }

            if (parameters != null)
            {
                foreach (var parameter in parameters.Where(parameter => !string.IsNullOrWhiteSpace(parameter.Value)))
                {
                    request.QueryString.Add(parameter.Key, parameter.Value);
                }
            }

            var buildAuthorizationUrl = request.ToString(this.UrlEncode);

            return buildAuthorizationUrl;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets access token.
        /// </summary>
        ///
        /// <param name="endPoint">
        ///     The end point.
        /// </param>
        /// <param name="code">
        ///     The code.
        /// </param>
        /// <param name="redirectUrl">
        ///     URL of the redirect.
        /// </param>
        /// <param name="grantType">
        ///     (optional) type of the grant.
        /// </param>
        ///
        /// <returns>
        ///     The access token.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        protected RestResponse<OAuth2TokenCredential> GetAccessToken(string endPoint, string code, string redirectUrl, OAuth2GrantType grantType = OAuth2GrantType.AuthorizationCode)
        {
            RestRequest request = new RestRequest(endPoint, RequestMode.UrlEncoded, AcceptMode.Json);

            switch (this.TokenAccessType)
            {
                case OAuth2TokenAccessType.Querystring:
                    request.Parameters.Add(RestConstants.OAuth2AccessCode, code);
                    request.Parameters.Add(RestConstants.OAuth2ClientID, this.AppID);
                    request.Parameters.Add(RestConstants.OAuth2ClientSecret, this.AppSecret);
                    request.Parameters.Add(RestConstants.OAuth2GrantType, grantType.ToDescription());
                    request.Parameters.Add(RestConstants.OAuth2RedirectUri, redirectUrl);
                    break;
                default:
                    request.AddBody(RestConstants.OAuth2AccessCode, code);
                    request.AddBody(RestConstants.OAuth2ClientID, this.AppID);
                    request.AddBody(RestConstants.OAuth2ClientSecret, this.AppSecret);
                    request.AddBody(RestConstants.OAuth2GrantType, grantType.ToDescription());
                    request.AddBody(RestConstants.OAuth2RedirectUri, redirectUrl);
                    break;
            }


            return this.Post<OAuth2TokenCredential>(request);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets refresh token.
        /// </summary>
        ///
        /// <param name="endPoint">
        ///     The end point.
        /// </param>
        /// <param name="refreshToken">
        ///     The refresh token.
        /// </param>
        /// <param name="grantType">
        ///     (optional) type of the grant.
        /// </param>
        ///
        /// <returns>
        ///     The refresh token.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        protected RestResponse<OAuth2TokenCredential> GetRefreshToken(string endPoint, string refreshToken, OAuth2GrantType grantType = OAuth2GrantType.RefreshToken)
        {
            RestRequest request = new RestRequest(endPoint, RequestMode.UrlEncoded, AcceptMode.Json);

            switch (this.TokenAccessType)
            {
                case OAuth2TokenAccessType.Querystring:
                    request.Parameters.Add(RestConstants.OAuth2ClientID, this.AppID);
                    request.Parameters.Add(RestConstants.OAuth2ClientSecret, this.AppSecret);
                    request.Parameters.Add(RestConstants.OAuth2RefreshToken, refreshToken);
                    request.Parameters.Add(RestConstants.OAuth2GrantType, grantType.ToDescription());
                    break;
                default:
                    request.AddBody(RestConstants.OAuth2ClientID, this.AppID);
                    request.AddBody(RestConstants.OAuth2ClientSecret, this.AppSecret);
                    request.AddBody(RestConstants.OAuth2RefreshToken, refreshToken);
                    request.AddBody(RestConstants.OAuth2GrantType, grantType.ToDescription());
                    break;
            }

            return this.Post<OAuth2TokenCredential>(request);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override RestResponse Execute(RestRequest request, MethodType method)
        {
            if (this.Credential != null)
            {
                this.OnInitRequest(request);
                this.SetupOAuth2(request, this.Credential);
            }

            return base.Execute(request, method);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets up the o authentication 2.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request.
        /// </param>
        /// <param name="token">
        ///     The token.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        private void SetupOAuth2(RestRequest request, OAuth2TokenCredential token)
        {
            switch (this.TokenAccessType)
            {
                case OAuth2TokenAccessType.Querystring:
                    request.Parameters.Add(this.OAuth2AccessTokenParameter, token.Token);
                    break;
                default:
                    string authHeader = "{0} {1}".FormatString(token.TokenType, token.Token);
                    request.Headers["Authorization"] = authHeader;
                    break;
            }
        }

        protected virtual string OAuth2AccessTokenParameter
        {
            get
            {
                return RestConstants.OAuth2AccessToken;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the initialise request action.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected internal virtual void OnInitRequest(RestRequest request)
        {
        }
    }
}
