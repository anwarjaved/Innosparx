namespace GoogleSDK.Common
{
    using Newtonsoft.Json;

    public class Location
    {

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }
    }
}
