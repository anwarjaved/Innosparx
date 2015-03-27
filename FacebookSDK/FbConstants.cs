namespace FacebookSDK
{
    internal class FBConstants
    {
        public const string AuthorizeUrl = "https://www.facebook.com/dialog/oauth";

        public const string AllFriendsUrl = "https://graph.facebook.com/{0}/friends";

        public const string UserUrl = "https://graph.facebook.com/{0}";

        public const string EventsUrl = "https://graph.facebook.com/{0}/events";

        public const string PostsUrl = "https://graph.facebook.com/{0}/feed";

        public const string TokenUrl = "https://graph.facebook.com/oauth/access_token";

        public const string EventInvitation = "https://graph.facebook.com/{0}/invited";

        public const string ExchangeToken = "fb_exchange_token";

        public const string OAuth2ClientID = "client_id";

        public const string OAuth2ClientSecret = "client_secret";
        public const string OAuth2GrantType = "grant_type";
    }
}
