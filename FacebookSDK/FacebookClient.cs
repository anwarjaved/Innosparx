using System;
using System.Collections.Generic;

namespace FacebookSDK
{
    using Framework;
    using Framework.Rest;
    using Framework.Rest.OAuth;

    public class FacebookClient : OAuth2Client
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the FacebookClient class.
        /// </summary>
        ///
        /// <param name="appID">
        ///     Identifier for the client.
        /// </param>
        /// <param name="appSecret">
        ///     The client secret.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public FacebookClient(string appID, string appSecret)
            : base(appID, appSecret)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the FacebookClient class.
        /// </summary>
        ///
        /// <param name="appID">
        ///     Identifier for the client.
        /// </param>
        /// <param name="appSecret">
        ///     The client secret.
        /// </param>
        /// <param name="credential">
        ///     The credential.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public FacebookClient(string appID, string appSecret, OAuth2TokenCredential credential)
            : base(appID, appSecret, credential)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the FacebookClient class.
        /// </summary>
        ///
        /// <param name="appID">
        ///     Identifier for the client.
        /// </param>
        /// <param name="appSecret">
        ///     The client secret.
        /// </param>
        /// <param name="token">
        ///     The token.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public FacebookClient(string appID, string appSecret, string token)
            : base(appID, appSecret, token)
        {
        }


        public string BuildAuthorizationUrl(
            string redirectUrl,
            string state = "",
            params string[] permissions)
        {
            return base.BuildAuthorizationUrl(FBConstants.AuthorizeUrl, redirectUrl,
                permissions.ToConcatenatedString(x => x, ","), OAuth2ResponseType.Code, state);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds authorization URL.
        /// </summary>
        ///
        /// <param name="redirectUrl">
        ///     URL of the redirect.
        /// </param>
        /// <param name="state">
        ///     (optional) the state.
        /// </param>
        /// <param name="permissions">
        ///     A variable-length parameters list containing permissions.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public string BuildAuthorizationUrl(
            string redirectUrl,
            string state = "",
            string permissions = "")
        {
            return base.BuildAuthorizationUrl(FBConstants.AuthorizeUrl, redirectUrl, permissions, OAuth2ResponseType.Code, state);
        }

        public OAuth2TokenCredential GetAccessToken(string code, string redirectUrl, bool throwException = false)
        {
            var response = this.GetAccessToken(FBConstants.TokenUrl, code, redirectUrl);

            OAuth2TokenCredential credential = response.ContentObject;
            if (!response.Completed || credential == null || !credential.Success)
            {
                if (throwException)
                {
                    credential.ThrowException();
                }
            }

            if (credential != null)
            {
                this.Credential = credential;
                return credential;
            }

            return null;
        }

        public OAuth2TokenCredential GetExtendedAccessToken(bool throwException = false)
        {
            RestRequest request = new RestRequest(FBConstants.TokenUrl, RequestMode.UrlEncoded, AcceptMode.Json);
            request.Parameters.Add(FBConstants.OAuth2ClientID, this.AppID);
            request.Parameters.Add(FBConstants.OAuth2ClientSecret, this.AppSecret);
            request.Parameters.Add(FBConstants.OAuth2GrantType, FBConstants.ExchangeToken);
            request.Parameters.Add(FBConstants.ExchangeToken, this.Credential.Token);

            var response = this.Post<OAuth2TokenCredential>(request);

            OAuth2TokenCredential credential = response.ContentObject;
            if (!response.Completed || credential == null || !credential.Success)
            {
                if (throwException)
                {
                    credential.ThrowException();
                }
            }

            if (credential != null)
            {
                this.Credential = credential;
                return credential;
            }

            return null;
        }

        /// <summary>
        /// Gets all friends.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>RestResponse{ResponseCollection{FriendResponse}}.</returns>
        public RestResponse<ResponseCollection<FriendResponse>> GetAllFriends(string userID = "me", int? limit = null)
        {
            RestRequest request = new RestRequest(FBConstants.AllFriendsUrl.FormatString(userID), RequestMode.UrlEncoded, AcceptMode.Json);

            if (limit.HasValue && limit.Value > 0)
            {
                request.Parameters.Add("limit", limit.Value);
            }

            return this.Get<ResponseCollection<FriendResponse>>(request);
        }

        public RestResponse<UserResponse> GetUser(string userID = "me", params UserFields[] fields)
        {
            var fieldList = fields.ToConcatenatedString(x => x.ToDescription(), ",");
            RestRequest request = new RestRequest(FBConstants.UserUrl.FormatString(userID), RequestMode.UrlEncoded, AcceptMode.Json);

            if (!string.IsNullOrWhiteSpace(fieldList))
            {
                request.Parameters.Add("fields", fieldList);
            }

            return this.Get<UserResponse>(request);
        }

        public RestResponse<UserResponse> CreateEvent(string name, DateTime startDate, string userID = "me", DateTime? endDate = null, 
            string description = "")
        {
            RestRequest request = new RestRequest(FBConstants.EventsUrl.FormatString(userID), RequestMode.UrlEncoded, AcceptMode.Json);

            request.AddBody("name", name);
            request.AddBody("start_time", new W3CDateTime(startDate).ToString());

            if (endDate.HasValue)
            {
                request.AddBody("end_time", new W3CDateTime(endDate.Value).ToString());
            }

            if (!string.IsNullOrEmpty(description))
            {
                request.AddBody("description", description);
            }

            return this.Post<UserResponse>(request);
        }

        public RestResponse<PostResponse> Publish(Post post, Privacy privacy = null, string id = "me")
        {
            if (post == null)
            {
                throw new ArgumentNullException("post");
            }


            if (string.IsNullOrWhiteSpace(post.Message))
            {
                throw new ArgumentException("message is required", "post");
               
            }
            RestRequest request = new RestRequest(FBConstants.PostsUrl.FormatString(id), RequestMode.UrlEncoded, AcceptMode.Json);


            post.AddFields(request);

            if (privacy != null)
            {
                privacy.AddFields(request);
            }

            return this.Post<PostResponse>(request);
        }

        public RestResponse<bool> SendEventInvitation(string eventId, IEnumerable<string> users)
        {
            RestRequest request = new RestRequest(FBConstants.EventInvitation.FormatString(eventId), RequestMode.UrlEncoded, AcceptMode.Json);

            request.Parameters.Add("users", users.ToConcatenatedString(x => x, ","));

            return this.Post<bool>(request);
        }
    }
}
