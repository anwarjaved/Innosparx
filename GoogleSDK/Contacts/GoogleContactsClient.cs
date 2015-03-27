using System.Collections.Generic;

namespace GoogleSDK.Contacts
{
    using Framework.Rest;
    using Framework.Rest.OAuth;

    public class GoogleContactsClient : GoogleClient
    {
        public GoogleContactsClient(string appID, string appSecret)
            : base(appID, appSecret)
        {
        }

        public GoogleContactsClient(string appID, string appSecret, OAuth2TokenCredential credential)
            : base(appID, appSecret, credential)
        {
        }

        public GoogleContactsClient(string appID, string appSecret, string token)
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

            if (!list.Contains(Scopes.GoogleContacts))
            {
                list.Add(Scopes.GoogleContacts);
            }

            return base.BuildAuthorizationUrl(redirectUrl, list, state, parameters);
        }

        protected override void OnInitRequest(RestRequest request)
        {
            request.Parameters.Add("alt", "json");
            request.Headers[GoogleConstants.GoogleContactApiVersion] = "3.0";
            base.OnInitRequest(request);
        }

        public RestResponse<GoogleDocument> GetAllContacts()
        {
            RestRequest request = new RestRequest(GoogleConstants.GoogleContactAllFeed, RequestMode.UrlEncoded, AcceptMode.Json);

            request.Parameters.Add("max-results", 5000);
            return this.Get<GoogleDocument>(request);
        }
    }
}
