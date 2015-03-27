using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Rest;
using Framework.Rest.OAuth;

namespace GoogleSDK.UrlShortener
{
    public class GoogleUrlShortnerClient : GoogleClient
    {
        
        public GoogleUrlShortnerClient(string appID, string appSecret)
            : base(appID, appSecret)
        {
        }

        public GoogleUrlShortnerClient(string appID, string appSecret, OAuth2TokenCredential credential)
            : base(appID, appSecret, credential)
        {
        }

        public GoogleUrlShortnerClient(string appID, string appSecret, string token)
            : base(appID, appSecret, token)
        {
        }

        public override string BuildAuthorizationUrl(string redirectUrl, IEnumerable<string> scope = null, string state = "", IDictionary<string, string> parameters = null)
        {
            if (scope == null)
            {
                scope = new List<string>();
            }

            List<string> list = new List<string>(scope);

            if (!list.Contains(Scopes.GoogleUrlShortner))
            {
                list.Add(Scopes.GoogleUrlShortner);
            }

            return base.BuildAuthorizationUrl(redirectUrl, list, state, parameters);
        }

        public RestResponse<GoogleUrlShortnerResponse> Shorten(string longUrl)
        {
            RestRequest request = new RestRequest(GoogleConstants.GoogleUrlShortnerUrl, RequestMode.Json, AcceptMode.Json);
            request.AddBody(new { longUrl = longUrl });
            return this.Post<GoogleUrlShortnerResponse>(request);
        }
    }
}
