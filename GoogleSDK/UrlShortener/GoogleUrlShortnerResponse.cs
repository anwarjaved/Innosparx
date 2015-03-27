using Framework.Rest.OAuth;
using Newtonsoft.Json;

namespace GoogleSDK.UrlShortener
{
    public class GoogleUrlShortnerResponse : OAuth2BaseResponse
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("id")]
        public string ShortUrl { get; set; }

        [JsonProperty("longUrl")]
        public string LongUrl { get; set; }

    }
}
