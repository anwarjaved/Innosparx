namespace TwitterSDK
{
    using System.IO;

    using Framework.Rest;
    using Framework.Rest.OAuth;

    /// <summary>
    /// Represents Twitter Client object used to access twitter.
    /// </summary>
    /// <author>Anwar</author>
    /// <datetime>3/25/2011 2:34 PM</datetime>
    public sealed class TwitterClient : OAuth1Client<OAuth1TokenCredential>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterClient"/> class.
        /// </summary>
        /// <param name="appKey">The application key.</param>
        /// <param name="appSecret">The application secret.</param>
        /// <author>Anwar</author>
        /// <datetime>3/24/2011 2:20 PM</datetime>
        public TwitterClient(string appKey, string appSecret)
            : base(appKey, appSecret)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the TwitterClient class.
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
        public TwitterClient(string appKey, string appSecret, OAuth1TokenCredential credential)
            : base(appKey, appSecret, credential)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the TwitterClient class.
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
        public TwitterClient(string appKey, string appSecret, string token, string tokenSecret)
            : base(appKey, appSecret, token, tokenSecret)
        {
        }


        /// <summary>
        /// Gets the Twitter request token.
        /// </summary>
        /// <param name="callbackUrl">The callback URL.</param>
        /// <returns><see cref="OAuth1TempCredential"/> object.</returns>
        /// <author>Anwar</author>
        /// <datetime>3/24/2011 4:10 PM</datetime>
        public OAuth1TempCredential GetRequestToken(string callbackUrl = null)
        {
            return this.GetRequestToken<OAuth1TempCredential>(TwitterConstants.RequestToken, callbackUrl);
        }

        /// <summary>
        /// Gets the Twitter access token.
        /// </summary>
        /// <param name="requestToken">The request token.</param>
        /// <param name="verifier">The verifier.</param>
        /// <returns><see cref="OAuth1TokenCredential"/> object.</returns>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 3:31 PM</datetime>
        public OAuth1TokenCredential GetAccessToken(OAuth1Token requestToken, string verifier)
        {
            return this.GetAccessToken(requestToken, TwitterConstants.AccessToken, verifier);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns an HTTP 200 OK response code and a representation of the requesting user 
        ///     if authentication was successful;
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/14/2013 5:52 PM.
        /// </remarks>
        ///
        /// <returns>
        ///     Returns an HTTP 200 OK response code and a representation of the requesting user 
        ///     if authentication was successful; returns a 401 status code and an error message if not.
        ///      Use this method to test if supplied user credentials are valid.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public TwitterResponse<TwitterCredential> Verify()
        {
            OAuth1RestRequest request = new OAuth1RestRequest(TwitterConstants.VerifyCredentials, RequestMode.UrlEncoded, AcceptMode.Json);

            RestResponse<TwitterCredential> response = this.Get<TwitterCredential>(request);

            return new TwitterResponse<TwitterCredential>(response);
        }


        /// <summary>
        /// Updates the authenticating user's status.
        /// A status update with text identical to the authenticating user's text identical to the authenticating user's current status will be ignored to prevent duplicates.
        /// </summary>
        /// <param name="text">The text to update.</param>
        /// <returns><see cref="TwitterResponse{T}"/> object.</returns>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 4:58 PM</datetime>
        public TwitterResponse<TwitterStatus> SendTweet(string text)
        {
            OAuth1RestRequest request = new OAuth1RestRequest(TwitterConstants.TweetResource, RequestMode.UrlEncoded, AcceptMode.Json);
            request.Parameters.Add("status", text);
            RestResponse<TwitterStatus> response = this.Post<TwitterStatus>(request);

            return new TwitterResponse<TwitterStatus>(response);
        }

        /// <summary>
        /// Returns a collection of user IDs for every user following the specified user.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>TwitterResponse{TwitterStatus}.</returns>
        public TwitterResponse<TwitterFollowers> GetFollowers(string userID = null)
        {
            if (string.IsNullOrWhiteSpace(userID))
            {
                TwitterResponse<TwitterCredential> twitterResponse = this.Verify();

                if (twitterResponse.Completed)
                {
                    userID = twitterResponse.ContentObject.ID;
                }
            }

            OAuth1RestRequest request = new OAuth1RestRequest(TwitterConstants.FollowersResource, RequestMode.UrlEncoded, AcceptMode.Json);
            request.Parameters.Add("user_id", userID);
            RestResponse<TwitterFollowers> response = this.Get<TwitterFollowers>(request);

            return new TwitterResponse<TwitterFollowers>(response);
        }

        /// <summary>
        /// Updates the authenticating user's status.
        /// A status update with text identical to the authenticating user's text identical to the authenticating user's current status will be ignored to prevent duplicates.
        /// </summary>
        /// <param name="text">The text to update.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="media">The media.</param>
        /// <returns>
        ///   <see cref="TwitterResponse{T}"/> object.
        /// </returns>
        /// <datetime>3/26/2011 4:58 PM</datetime>
        /// <author>
        /// Anwar
        /// </author>
        public TwitterResponse<TwitterStatus> SendTweet(string text, string fileName, Stream media)
        {
            OAuth1RestRequest request = new OAuth1RestRequest(TwitterConstants.TweetWithMediaResource, RequestMode.MultiPart, AcceptMode.Json);

            request.Parameters.Add("status", text);
            request.AddFile(new FileParameter("media[]", fileName, media));
            RestResponse<TwitterStatus> response = this.Post<TwitterStatus>(request);

            return new TwitterResponse<TwitterStatus>(response);
        }

        /// <summary>
        /// Builds the autorization URL.
        /// </summary>
        /// <param name="requestToken">The request token.</param>
        /// <returns>AUthentication Url.</returns>
        /// <author>Anwar</author>
        /// <datetime>3/25/2011 1:11 PM</datetime>
        public string BuildAuthorizationUrl(OAuth1TempCredential requestToken)
        {
            return this.BuildAuthorizationUrl(requestToken, TwitterConstants.AuthorizeToken);
        }
    }
}

