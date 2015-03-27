namespace GoogleSDK.Maps
{
    using Framework.Rest;

    using GoogleSDK.Places;

    public class GoogleMapClient : RestClient
    {
        private readonly string apiKey;

        private readonly bool useSensor;

        public GoogleMapClient(string apiKey, bool useSensor = false)
        {
            this.apiKey = apiKey;
            this.useSensor = useSensor;
        }


        public RestResponse<AddressResponses> FindByAddress(string address)
        {
            RestRequest request = new RestRequest(GoogleConstants.GoogleGeocodingUrl, AcceptMode.Json);

            //request.Parameters.Add("key", apiKey);
            request.Parameters.Add("address", address);
            request.Parameters.Add("sensor", useSensor ? "true" : "false");

            return this.Get<AddressResponses>(request);
        }

        public RestResponse<AddressResponses> FindByZip(string zip)
        {
            RestRequest request = new RestRequest(GoogleConstants.GoogleGeocodingUrl, AcceptMode.Json);

            //request.Parameters.Add("key", apiKey);
            request.Parameters.Add("address", zip);
            request.Parameters.Add("components", "postal_code");

            request.Parameters.Add("sensor", useSensor ? "true" : "false");

            return this.Get<AddressResponses>(request);
        }
    }
}
