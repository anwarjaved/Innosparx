namespace Framework.Rest
{
    using System;
    using System.Reflection;

    internal class RestConstants
    {
        /// <summary>
        /// 16-KB Buffer.
        /// </summary>
        public const int BufferSize = 16384;
      
        public const string RestComponent = "Rest Client";

        public const string OAuthConsumerKey = "oauth_consumer_key";

        public const string OAuthConsumerSecret = "oauth_consumer_secret";

        public const string OAuthCallback = "oauth_callback";

        public const string OAuthVersion = "oauth_version";

        public const string OAuthSignatureMethod = "oauth_signature_method";

        public const string OAuthSignature = "oauth_signature";

        public const string OAuthTimestamp = "oauth_timestamp";

        public const string OAuthNonce = "oauth_nonce";

        public const string OAuthToken = "oauth_token";

        public const string OAuthSessionHandle = "oauth_session_handle";

        public const string AuthExpiresIn = "oauth_authorization_expires_in";

        public const string OAuthTokenSecret = "oauth_token_secret";

        public const string HMACSHA1Signature = "HMAC-SHA1";

        public const string OAuthDefaultVersion = "1.0";

        public const string OAuthParameterPrefix = "oauth_";

        public const string OAuthCallbackConfirmed = "oauth_callback_confirmed";

        public const string XOAuthRequestAuthUrl = "xoauth_request_auth_url";

        public const string OAuth2ClientID = "client_id";

        public const string OAuth2ClientSecret = "client_secret";

        public const string OAuth2RedirectUri = "redirect_uri";

        public const string OAuth2AccessType = "access_type";

        public const string OAuth2GrantType = "grant_type";

        public const string OAuth2ResponseType = "response_type";

        public const string OAuth2State = "state";

        public const string OAuth2Scope = "scope";

        public const string OAuth2ApprovalPrompt = "approval_prompt";

        public const string OAuth2AccessCode = "code";

        public const string OAuth2AccessToken = "access_token";

        public const string OAuth2TokenType = "token_type";

        public const string OAuth2RefreshToken = "refresh_token";

        public const string OAuth2ExpiresIn = "expires_in";

        public const string OAuthExpiresIn = "oauth_expires_in";

        public const string OAuthError = "error";

        public const string OAuthCode = "code";

        public const string OAuthErrorMessage = "message";

        public const string OAuthErrorUrl = "error_uri";

        public const string OAuth2Expires = "expires";

        public const string OAuth2BearerToken = "Bearer";

        public const string OAuthErrorType = "type";

        public const string AlphaNumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public const string OAuthVerifier = "oauth_verifier";

        public const string UnreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        public static readonly Version AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public static readonly string FrameworkVersion = "Inno Sparx {0} Beta".FormatString(AssemblyVersion.ToString(2));
    }
}
