using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSDK
{
    internal class TwitterConstants
    {
        public const string RequestToken = "https://api.twitter.com/oauth/request_token";

        public const string AccessToken = "https://api.twitter.com/oauth/access_token";

        public const string AuthorizeToken = "https://api.twitter.com/oauth/authorize";

        public const string TwitterAPI = "https://api.twitter.com/1.1/";

        public const string TwitterUploadAPI = "https://upload.twitter.com/1/";

        public const string TweetResource = TwitterAPI + "statuses/update.json";

        public const string FollowersResource = TwitterAPI + "followers/ids.json";

        public const string TweetWithMediaResource = TwitterUploadAPI + "statuses/update_with_media.json";


        public const string TwitPicAPI = "http://api.twitpic.com/2/";
        public const string TwitPicAPIv1 = "http://api.twitpic.com/1/";

        public const string UploadResource = TwitPicAPI + "upload.json";
        public const string UploadResourcev1 = TwitPicAPIv1 + "upload.json";

        public static readonly Uri TwitPicServiceProvider = new Uri("https://api.twitter.com/1/account/verify_credentials.json");

        public const string VerifyCredentials = TwitterAPI + "account/verify_credentials.json";

    }
}
