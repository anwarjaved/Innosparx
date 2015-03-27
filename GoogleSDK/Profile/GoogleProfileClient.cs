using System.Collections.Generic;
using Framework.Rest;
using GoogleSDK.Common;

namespace GoogleSDK.Profile
{
    using Framework.Rest.OAuth;

    public class GoogleProfileClient : GoogleClient
    {
        public GoogleProfileClient(string appID, string appSecret)
            : base(appID, appSecret)
        {
        }

        public GoogleProfileClient(string appID, string appSecret, OAuth2TokenCredential credential)
            : base(appID, appSecret, credential)
        {
        }

        public GoogleProfileClient(string appID, string appSecret, string token)
            : base(appID, appSecret, token)
        {
        }

        public string BuildAuthorizationUrl(
         string redirectUrl,
         string state = "")
        {
            return base.BuildAuthorizationUrl(redirectUrl);
        }

        public override string BuildAuthorizationUrl(string redirectUrl, IEnumerable<string> scope = null, string state = "", IDictionary<string, string> parameters = null)
        {
            if (scope == null)
            {
                scope = new List<string>();
            }

            List<string> list = new List<string>(scope);

            if (!list.Contains(Scopes.GoogleProfile))
            {
                list.Add(Scopes.GoogleProfile);
            }

            return base.BuildAuthorizationUrl(redirectUrl, list, state, parameters);
        }

        public RestResponse<UserProfile> GetProfile()
        {
            RestRequest request = new RestRequest(GoogleConstants.GoogleProfileUrl, RequestMode.UrlEncoded, AcceptMode.Json);
            return this.Get<UserProfile>(request);
        }
    }
}
