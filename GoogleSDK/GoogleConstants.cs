using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK
{
    internal class GoogleConstants
    {
        public const string GoogleContactApiVersion = "GData-Version";
        public const string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";

        public const string TokenUrl = "https://accounts.google.com/o/oauth2/token";

        public const string GoogleContactAllFeed = "https://www.google.com/m8/feeds/contacts/default/thin";

        public const string GooglePlacesTextSearchUrl = "https://maps.googleapis.com/maps/api/place/textsearch/json";

        public const string GooglePlacesAutocompleteSearchUrl = "https://maps.googleapis.com/maps/api/place/autocomplete/json";

        public const string GooglePlacesNearBySearchUrl = "https://maps.googleapis.com/maps/api/place/nearbysearch/json";

        public const string GooglePlaceDetailsUrl = "https://maps.googleapis.com/maps/api/place/details/json";

        public const string GooglePlusGetPeopleUrl = "https://www.googleapis.com/plus/v1/people/{0}";

        public const string GoogleProfileUrl = "https://www.googleapis.com/oauth2/v1/userinfo";

        public const string GoogleGeocodingUrl = "http://maps.googleapis.com/maps/api/geocode/json";

        public const string GoogleAnalyticsUrl = "https://www.googleapis.com/analytics/v3/data/ga";

        public const string GoogleUrlShortnerUrl = "https://www.googleapis.com/urlshortener/v1/url";
    }
}
