using System.Collections.Generic;
using Framework;

namespace GoogleSDK
{
    using Framework.Rest.OAuth;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Google client.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 12/12/2012.
    /// </remarks>
    ///
    /// <seealso cref="OAuth2Client"/>
    ///-------------------------------------------------------------------------------------------------
    public class GoogleClient : OAuth2Client
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the GoogleClient class.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 12/12/2012.
        /// </remarks>
        ///
        /// <param name="appID">
        ///     Identifier for the client.
        /// </param>
        /// <param name="appSecret">
        ///     The client secret.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public GoogleClient(string appID, string appSecret)
            : base(appID, appSecret)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the GoogleClient class.
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
        public GoogleClient(string appID, string appSecret, OAuth2TokenCredential credential)
            : base(appID, appSecret, credential)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the GoogleClient class.
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
        public GoogleClient(string appID, string appSecret, string token)
            : base(appID, appSecret, token)
        {
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds authorization URL.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 12/12/2012.
        /// </remarks>
        ///
        /// <param name="redirectUrl">
        ///     URL of the redirect.
        /// </param>
        /// <param name="scope">
        ///     (optional) the scope.
        /// </param>
        /// <param name="state">
        ///     (optional) the state.
        /// </param>
        /// <param name="parameters">
        ///     (optional) options for controlling the operation.
        /// </param>
        ///
        /// <returns>
        ///     The URL used when authenticating a user.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public virtual string BuildAuthorizationUrl(
            string redirectUrl,
            IEnumerable<string> scope = null,
            string state = "",
            IDictionary<string, string> parameters = null)
        {
            if (scope == null)
            {
                scope = new List<string>();
            }

            string scopes = scope.ToConcatenatedString();
            return base.BuildAuthorizationUrl(GoogleConstants.AuthorizeUrl, redirectUrl, scopes, OAuth2ResponseType.Code, state, parameters);
        }

        public OAuth2TokenCredential GetAccessToken(string code, string redirectUrl, bool throwException = false)
        {
            var response = this.GetAccessToken(GoogleConstants.TokenUrl, code, redirectUrl);

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

        public OAuth2TokenCredential GetRefreshToken(string refreshToken, bool throwException = false)
        {
            var response = base.GetRefreshToken(GoogleConstants.TokenUrl, refreshToken);

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
    }
}
