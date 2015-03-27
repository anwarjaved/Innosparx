namespace Framework.Rest.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// OAuth Client.
    /// </summary>
    /// <author>Anwar</author>
    /// <datetime>3/26/2011 1:15 AM</datetime>
    public abstract class OAuth1Client<T> : RestClient where T : OAuth1TokenCredential, new()
    {
        private static readonly Random Randomizer = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth1Client{T}"/> class.
        /// </summary>
        /// <param name="appKey">The application key.</param>
        /// <param name="appSecret">The application secret.</param>
        /// <author>Anwar</author>
        /// <datetime>3/24/2011 2:21 PM</datetime>
        protected OAuth1Client(string appKey, string appSecret)
        {
            this.AppKey = appKey;
            this.AppSecret = appSecret;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the OAuth1Client class.
        /// </summary>
        ///
        /// <param name="appKey">
        ///     The application key.
        /// </param>
        /// <param name="appSecret">
        ///     The application secret.
        /// </param>
        /// <param name="credential">
        ///     The credential.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected OAuth1Client(string appKey, string appSecret, T credential)
        {
            this.AppKey = appKey;
            this.AppSecret = appSecret;
            this.Credential = credential;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the OAuth1Client class.
        /// </summary>
        ///
        /// <param name="appKey">
        ///     The application key.
        /// </param>
        /// <param name="appSecret">
        ///     The application secret.
        /// </param>
        /// <param name="token">
        ///     The token.
        /// </param>
        /// <param name="tokenSecret">
        ///     The token secret.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        protected OAuth1Client(string appKey, string appSecret, string token, string tokenSecret)
        {
            this.AppKey = appKey;
            this.AppSecret = appSecret;
            this.Credential = new T() { Secret = tokenSecret, Token = token };
        }

        /// <summary>
        /// Gets the consumer key.
        /// </summary>
        /// <value>The consumer key.</value>
        public string AppKey { get; private set; }

        /// <summary>
        /// Gets the consumer secret.
        /// </summary>
        /// <value>The consumer secret.</value>
        public string AppSecret { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the credential.
        /// </summary>
        ///
        /// <value>
        ///     The credential.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public T Credential { get; protected set; }

        /// <summary>
        /// Builds the autorization URL.
        /// </summary>
        /// <param name="requestToken">The request token.</param>
        /// <param name="authorizationUrl">The authoriozation URL.</param>
        /// <returns>AUthentication Url.</returns>
        /// <author>Anwar</author>
        /// <datetime>3/25/2011 1:11 PM</datetime>
        protected virtual string BuildAuthorizationUrl(OAuth1TempCredential requestToken, string authorizationUrl)
        {
            UrlBuilder restRequest = new UrlBuilder(authorizationUrl);
            restRequest.QueryString.Add(RestConstants.OAuthToken, requestToken.Token);
            return restRequest.ToString();
        }

        /// <summary>
        /// Gets the request token.
        /// </summary>
        /// <param name="requestTokenUrl">The request token URL.</param>
        /// <param name="callbackUrl">The callback URL.</param>
        /// <returns><see cref="OAuth1TempCredential"/> object.</returns>
        /// <author>Anwar</author>
        /// <datetime>3/24/2011 4:10 PM</datetime>
        protected TV GetRequestToken<TV>(string requestTokenUrl, string callbackUrl = null) where TV : OAuth1TempCredential, new()
        {
            RestRequest request = new RestRequest(requestTokenUrl);

            request.Method = MethodType.Post;
            this.SetupOAuth(request, callbackUrl);

            RestResponse<TV> response = this.Post<TV>(request);

            return response.ContentObject;
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="requestToken">The request token.</param>
        /// <param name="accessTokenUrl">The access token URL.</param>
        /// <param name="verifier">The verifier.</param>
        /// <returns><see cref="OAuth1TokenCredential"/> object.</returns>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 3:31 PM</datetime>
        protected T GetAccessToken(OAuth1Token requestToken, string accessTokenUrl, string verifier)
        {
            var request = this.BuildAccessTokenRequest(requestToken, accessTokenUrl, verifier);
            request.Method = MethodType.Post;
            this.SetupOAuth(request, requestToken, verifier);

            RestResponse<T> response = this.Post<T>(request);

            if (response.Completed && response.ContentObject != null && response.ContentObject.Success)
            {
                this.Credential = response.ContentObject;
            }

            return response.ContentObject;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the access token.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/28/2014 3:13 PM.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="accessToken">
        ///     The request token.
        /// </param>
        /// <param name="accessTokenUrl">
        ///     The access token URL.
        /// </param>
        /// <param name="dictionary">
        ///     (Optional) the dictionary.
        /// </param>
        ///
        /// <returns>
        ///     <see cref="OAuth1TokenCredential"/> object.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        protected T GetRefreshToken(T accessToken, string accessTokenUrl, IDictionary<string, string> dictionary = null)
        {
            var request = this.BuildRefreshTokenRequest(accessToken, accessTokenUrl, dictionary);
            request.Method = MethodType.Post;
            this.SetupOAuth(request, accessToken);

            RestResponse<T> response = this.Post<T>(request);

            if (response.Completed && response.ContentObject != null && response.ContentObject.Success)
            {
                this.Credential = response.ContentObject;
            }

            return response.ContentObject;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds access token request.
        /// </summary>
        ///
        /// <param name="requestToken">
        ///     The request token.
        /// </param>
        /// <param name="accessTokenUrl">
        ///     The access token URL.
        /// </param>
        /// <param name="verifier">
        ///     The verifier.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual RestRequest BuildAccessTokenRequest(OAuth1Token requestToken, string accessTokenUrl, string verifier)
        {
            RestRequest request = new RestRequest(accessTokenUrl);

            return request;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual RestRequest BuildRefreshTokenRequest(T accessToken, string accessTokenUrl, IDictionary<string, string> dictionary = null)
        {
            RestRequest request = new RestRequest(accessTokenUrl);
            request.Parameters.Add(RestConstants.OAuthSessionHandle, this.Credential.SessionHandle);

            if (dictionary != null)
            {
                dictionary.ForEach(x => request.Parameters.Add(x.Key, x.Value));
            }

            return request;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override RestResponse Execute(RestRequest request, MethodType method)
        {
            if (this.Credential != null)
            {
                request.Method = method;
                this.OnInitRequest(request);
                this.SetupOAuth(request, this.Credential);
            }

            return base.Execute(request, method);
        }


        /// <summary>
        /// Generates the time stamp.
        /// </summary>
        /// <returns>Timestamp value.</returns>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 1:19 AM</datetime>
        internal static string GenerateTimeStamp()
        {
            DateTime now = DateTime.UtcNow;
            DateTime then = new DateTime(1970, 1, 1);

            TimeSpan timespan = now - then;
            long timestamp = (long)timespan.TotalSeconds;

            return timestamp.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Generates the nonce.
        /// </summary>
        /// <returns>A randomly generated alphabetical string.</returns>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 1:20 AM</datetime>
        internal static string GenerateNonce()
        {
            var sb = new StringBuilder();

            for (var i = 0; i <= 12; i++)
            {
                var index = Randomizer.Next(RestConstants.AlphaNumeric.Length);
                sb.Append(RestConstants.AlphaNumeric[index]);
            }

            return sb.ToString();
        }

        internal string GenerateSignature(string consumerSecret, string tokenSecret, string signatureBase)
        {
            byte[] hashBytes;
            using (HMACSHA1 hmacsha1 = new HMACSHA1
              {
                  Key = Encoding.UTF8.GetBytes(string.Format("{0}&{1}", this.UrlEncode(consumerSecret), string.IsNullOrEmpty(tokenSecret) ? string.Empty : UrlEncode(tokenSecret)))
              })
            {
                byte[] dataBuffer = Encoding.UTF8.GetBytes(signatureBase);
                hashBytes = hmacsha1.ComputeHash(dataBuffer);
            }

            return Convert.ToBase64String(hashBytes);
        }

        internal string GenerateSignatureBase(Uri url, string consumerKey, string token, string httpMethod, string timeStamp, string nonce, string verifier, QueryParameterCollection parameters)
        {
            if (token == null)
            {
                token = string.Empty;
            }

            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException("consumerKey");
            }

            if (string.IsNullOrEmpty(httpMethod))
            {
                throw new ArgumentNullException("httpMethod");
            }

            QueryParameterCollection sortedParameters = new QueryParameterCollection
                                              {
                                                new QueryParameter(RestConstants.OAuthVersion, RestConstants.OAuthDefaultVersion),
                                                new QueryParameter(RestConstants.OAuthNonce, nonce),
                                                new QueryParameter(RestConstants.OAuthTimestamp, timeStamp),
                                                new QueryParameter(RestConstants.OAuthSignatureMethod, RestConstants.HMACSHA1Signature),
                                                new QueryParameter(RestConstants.OAuthConsumerKey, consumerKey),
                                              };

            if (!string.IsNullOrEmpty(token))
            {
                sortedParameters.Add(new QueryParameter(RestConstants.OAuthToken, token));
            }

            if (!string.IsNullOrEmpty(verifier))
            {
                sortedParameters.Add(new QueryParameter(RestConstants.OAuthVerifier, verifier));
            }

            sortedParameters.AddRange(parameters.Where(queryParameter => !string.IsNullOrEmpty(queryParameter.Value)));

            for (int index = sortedParameters.Count - 1; index >= 0; index--)
            {
                QueryParameter parameter = sortedParameters[index];
                if (parameter.Name == RestConstants.OAuthConsumerSecret ||
                    parameter.Name == RestConstants.OAuthTokenSecret)
                {
                    sortedParameters.RemoveAt(index);
                }
            }

            sortedParameters.Sort(new QueryParameterComparer());
            parameters.Clear();
            parameters.AddRange(sortedParameters);

            string normalizedRequestParameters = NormalizeParameters(sortedParameters);

            StringBuilder signatureBase = new StringBuilder();
            signatureBase.AppendFormat("{0}&", httpMethod.ToUpperInvariant());

            if (url != null)
            {
                string normalizedUrl = NormalizeUrl(url);
                signatureBase.AppendFormat("{0}&", UrlEncode(normalizedUrl));
            }

            signatureBase.AppendFormat("{0}", UrlEncode(normalizedRequestParameters));

            return signatureBase.ToString();
        }

        /// <summary>
        /// Normalizes the URL.
        /// </summary>
        /// <param name="url">The URL to normalize.</param>
        /// <returns>The normalized url string.</returns>
        private static string NormalizeUrl(Uri url)
        {
            string normalizedUrl = string.Format(CultureInfo.InvariantCulture, "{0}://{1}", url.Scheme, url.Host);
            if (!((url.Scheme == "http" && url.Port == 80) || (url.Scheme == "https" && url.Port == 443)))
            {
                normalizedUrl += ":" + url.Port;
            }

            normalizedUrl += url.AbsolutePath;
            return normalizedUrl;
        }

        /// <summary>
        /// Normalizes the parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Normalized Paramters as string.</returns>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 1:41 AM</datetime>
        private string NormalizeParameters(QueryParameterCollection parameters)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (QueryParameter pair in parameters)
            {
                sb.AppendFormat("{0}={1}", pair.Name, UrlEncode(pair.Value));

                if (i < parameters.Count - 1)
                {
                    sb.Append("&");
                }

                i++;
            }

            return sb.ToString();
        }

        internal string GenerateAuthorizationHeader(Uri resource, QueryParameterCollection parameters)
        {
            parameters.Sort(new QueryParameterComparer());
            StringBuilder sb = new StringBuilder();
            sb.Append("OAuth ");

            UrlBuilder urlBuilder = new UrlBuilder(resource);
            sb.AppendFormat("realm=\"{0}\", ", urlBuilder.Url);

            foreach (var item in parameters)
            {
                if (item.Name.StartsWith(RestConstants.OAuthParameterPrefix))
                {
                    sb.AppendFormat(
                      "{0}=\"{1}\", ",
                      item.Name,
                      UrlEncode(item.Value));
                }
            }

            return sb.ToString(0, sb.Length - 2);
        }

        protected virtual void OnInitRequest(RestRequest request)
        {
        }

        protected override string UrlEncode(string value)
        {
            StringBuilder result = new StringBuilder();

            foreach (char symbol in value)
            {
                if (RestConstants.UnreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append('%' + string.Format("{0:X2}", (int)symbol));
                }
            }

            return result.ToString();
        }

        private void SetupOAuth(RestRequest request, string callbackUrl)
        {
            string nonce = GenerateNonce();
            string timeStamp = GenerateTimeStamp();

            QueryParameterCollection parameters = new QueryParameterCollection(request.Parameters);

            OAuth1RestRequest authRestRequest = request as OAuth1RestRequest;

            if (authRestRequest != null)
            {
                parameters.AddRange(authRestRequest.OAuthValues.Select(x => new QueryParameter(x.Key, x.Value)));
                authRestRequest.OAuthValues.Clear();
            }

            if (!string.IsNullOrWhiteSpace(callbackUrl))
            {
                parameters.Add(new QueryParameter(RestConstants.OAuthCallback, callbackUrl));
            }

            var signatureBase = GenerateSignatureBase(
              request.RequestUrl,
              this.AppKey,
              string.Empty,
              request.Method.ToString().ToUpperInvariant(),
              timeStamp,
              nonce,
              string.Empty,
              parameters);

            // obtain a signature and add it to oauth header parameters
            var signature = GenerateSignature(this.AppSecret, null, signatureBase);
            parameters.Add(RestConstants.OAuthSignature, signature);

            string authHeader = GenerateAuthorizationHeader(request.RequestUrl, parameters);
            request.Headers["Authorization"] = authHeader;
        }

        private void SetupOAuth(RestRequest request, OAuth1Token requestToken, string verifier)
        {
            string nonce = GenerateNonce();
            string timeStamp = GenerateTimeStamp();

            QueryParameterCollection parameters = new QueryParameterCollection(request.Parameters);

            OAuth1RestRequest authRestRequest = request as OAuth1RestRequest;

            if (authRestRequest != null)
            {
                parameters.AddRange(authRestRequest.OAuthValues.Select(x => new QueryParameter(x.Key, x.Value)));
                authRestRequest.OAuthValues.Clear();
            }

            if (string.IsNullOrEmpty(requestToken.Token))
            {
                throw new ArgumentException("Null Request Token");
            }

            if (string.IsNullOrEmpty(requestToken.Secret))
            {
                throw new ArgumentException("Null Request Token Secret.");
            }

            if (string.IsNullOrEmpty(verifier))
            {
                throw new ArgumentException("Null Auth Verifier.");
            }

            var signatureBase = GenerateSignatureBase(
                request.RequestUrl,
                this.AppKey,
                requestToken.Token,
                request.Method.ToString(),
                timeStamp,
                nonce,
                verifier,
                parameters);

            // obtain a signature and add it to oauth header parameters
            var signature = GenerateSignature(this.AppSecret, requestToken.Secret, signatureBase);
            parameters.Add(RestConstants.OAuthSignature, signature);

            string authHeader = GenerateAuthorizationHeader(request.RequestUrl, parameters);
            request.Headers["Authorization"] = authHeader;
        }

        private void SetupOAuth(RestRequest request, OAuth1TokenCredential token)
        {
            string nonce = GenerateNonce();
            string timeStamp = GenerateTimeStamp();

            QueryParameterCollection parameters = new QueryParameterCollection(request.Parameters);

            OAuth1RestRequest authRestRequest = request as OAuth1RestRequest;

            if (authRestRequest != null)
            {
                parameters.AddRange(authRestRequest.OAuthValues.Select(x => new QueryParameter(x.Key, x.Value)));
                authRestRequest.OAuthValues.Clear();
            }

            if (string.IsNullOrEmpty(token.Token))
            {
                throw new ArgumentException("Null Request Token");
            }

            if (string.IsNullOrEmpty(token.Secret))
            {
                throw new ArgumentException("Null Request Token Secret.");
            }

            var signatureBase = GenerateSignatureBase(
                request.RequestUrl,
                this.AppKey,
                token.Token,
                request.Method.ToString(),
                timeStamp,
                nonce,
                string.Empty,
                parameters);

            // obtain a signature and add it to oauth header parameters
            var signature = GenerateSignature(this.AppSecret, token.Secret, signatureBase);
            parameters.Add(RestConstants.OAuthSignature, signature);

            string authHeader = GenerateAuthorizationHeader(request.RequestUrl, parameters);
            request.Headers["Authorization"] = authHeader;
        }
    }
}
