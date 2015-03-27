namespace GoogleSDK.Places
{
    using Framework;
    using Framework.Localization;
    using Framework.Rest;

    using GoogleSDK.Places;

    public class GooglePlacesClient : RestClient
    {
        private readonly string apiKey;

        private readonly bool useSensor;

        public GooglePlacesClient(string apiKey, bool useSensor)
        {
            this.apiKey = apiKey;
            this.useSensor = useSensor;
        }


        public RestResponse<PlacesResponse> Search(string query, double? lat = null, double? lon = null, int? radius = null, LanguageCode? language = null)
        {
            RestRequest request = new RestRequest(GoogleConstants.GooglePlacesTextSearchUrl, AcceptMode.Json);

            request.Parameters.Add("query", "\"" + query.Normalize() + "\"");
            request.Parameters.Add("sensor", useSensor ? "true" : "false");
            request.Parameters.Add("key", apiKey);

            if (lat.HasValue && lon.HasValue)
            {
                request.Parameters.Add("location", "{0},{1}".FormatString(lat.Value, lon.Value));

                if (radius.HasValue)
                {
                    request.Parameters.Add("radius", radius.Value);
                }

            }

            if (language.HasValue)
            {
                request.Parameters.Add("language", language.Value.ToDescription());
            }


            return this.Get<PlacesResponse>(request);
        }

        public RestResponse<NearByPlacesResponse> NearBySearch(string query, double lat, double lon, RankByOrder rankBy = RankByOrder.Distance, int radius = 50000)
        {
            RestRequest request = new RestRequest(GoogleConstants.GooglePlacesNearBySearchUrl, AcceptMode.Json);

            request.Parameters.Add("name", "\"" + query.Normalize() + "\"");
            request.Parameters.Add("sensor", useSensor ? "true" : "false");
            request.Parameters.Add("key", apiKey);

            request.Parameters.Add("location", "{0},{1}".FormatString(lat, lon));

            request.Parameters.Add("rankby", rankBy == RankByOrder.Distance ? "distance" : "prominence");

            if (rankBy != RankByOrder.Distance)
            {
                request.Parameters.Add("radius", radius);
            }

            return this.Get<NearByPlacesResponse>(request);
        }


        public RestResponse<AutocompletePlaceResponse> Autocomplete(string query, double? lat = null, double? lon = null, double? radius = null, LanguageCode? language = null)
        {
            RestRequest request = new RestRequest(GoogleConstants.GooglePlacesAutocompleteSearchUrl, AcceptMode.Json);

            request.Parameters.Add("input", "\"" + query.Normalize() + "\"");
            request.Parameters.Add("sensor", useSensor ? "true" : "false");
            request.Parameters.Add("key", apiKey);

            if (lat.HasValue && lon.HasValue)
            {
                request.Parameters.Add("location", "{0},{1}".FormatString(lat.Value, lon.Value));

                if (radius.HasValue)
                {
                    request.Parameters.Add("radius", radius.Value);
                }

            }

            if (language.HasValue)
            {
                request.Parameters.Add("language", language.Value.ToDescription());
            }

            return this.Get<AutocompletePlaceResponse>(request);
        }


        public RestResponse<AddressResponse> GetDetails(string referenceID)
        {
            RestRequest request = new RestRequest(GoogleConstants.GooglePlaceDetailsUrl, AcceptMode.Json);

            request.Parameters.Add("reference", referenceID);
            request.Parameters.Add("sensor", useSensor ? "true" : "false");
            request.Parameters.Add("key", apiKey);

            return this.Get<AddressResponse>(request);
        }
    }
}
