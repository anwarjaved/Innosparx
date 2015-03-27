namespace GoogleSDK.Places
{
    using GoogleSDK.Converters;

    using Newtonsoft.Json;

    [JsonConverter(typeof(AddressDetailsConverter))]
    public class AddressDetails
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string Zip { get; set; }
    }
}
