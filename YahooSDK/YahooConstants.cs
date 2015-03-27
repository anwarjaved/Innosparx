using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YahooSDK
{
    internal class YahooConstants
    {
        public const string AccessToken = "https://api.login.yahoo.com/oauth/v2/get_token";

        public const string YahooGuid = "xoauth_yahoo_guid";

        public const string RequestToken = "https://api.login.yahoo.com/oauth/v2/get_request_token";
        public const string OAuthExpiresIn = "oauth_expires_in";


        public const string AllContactsApi = "https://social.yahooapis.com/v1/user/{0}/contacts;count={1}";
    }
}
