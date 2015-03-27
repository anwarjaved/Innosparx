using System.Collections.Generic;
using Framework;
using Framework.Rest;
namespace GoogleSDK.Plus
{
    using Framework.Rest.OAuth;

    public class GooglePlusClient : GoogleClient
    {
        public GooglePlusClient(string appID, string appSecret)
            : base(appID, appSecret)
        {
        }

        public GooglePlusClient(string appID, string appSecret, OAuth2TokenCredential credential)
            : base(appID, appSecret, credential)
        {
        }

        public GooglePlusClient(string appID, string appSecret, string token)
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

            if (!list.Contains(Scopes.GooglePlus))
            {
                list.Add(Scopes.GooglePlus);
            }

            return base.BuildAuthorizationUrl(redirectUrl, list, state, parameters);
        }

        public RestResponse<People> GetPeople(string userID = "me", params PeopleFields[] fields)
        {
            RestRequest request = new RestRequest(GoogleConstants.GooglePlusGetPeopleUrl.FormatString(userID), RequestMode.UrlEncoded, AcceptMode.Json);
            request.Parameters.Add("fields", fields.ToConcatenatedString(x => x.ToDescription(), ","));
            return this.Get<People>(request);
        }

        public RestResponse<People> GetMe(params PeopleFields[] fields)
        {
            return this.GetPeople("me", fields);
        }
    }
}
