using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK.UrlShortener
{
    using Framework.Rest;

    public class GoogleUrlShortnerApiClient : GoogleApiClient
    {
        public GoogleUrlShortnerApiClient(string apikey)
            : base(apikey)
        {
        }

        public RestResponse<GoogleUrlShortnerResponse> Shorten(string longUrl)
        {
            RestRequest request = new RestRequest(GoogleConstants.GoogleUrlShortnerUrl, RequestMode.Json, AcceptMode.Json);
            request.AddBody(new { longUrl = longUrl });
            return this.Post<GoogleUrlShortnerResponse>(request);
        }
    }
}
