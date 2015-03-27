using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxmindSDK
{
    using Framework.Domain;

    public class Location
    {
        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int DMACode { get; set; }

        public int AreaCode { get; set; }

        public string RegionName { get; set; }

        public int MetroCode { get; set; }

        private const double EarthDiameter = 2 * 6378.2;

        private const double PI = 3.14159265;

        private const double RadConvert = PI / 180;

        public double CalculateDistance(Location loc)
        {
            double lat1 = this.Latitude;
            double lon1 = this.Longitude;
            double lat2 = loc.Latitude;
            double lon2 = loc.Longitude;

            // convert degrees to radians
            lat1 *= RadConvert;
            lat2 *= RadConvert;

            // find the deltas
            double deltaLat = lat2 - lat1;
            double deltaLon = (lon2 - lon1) * RadConvert;

            // Find the great circle distance
            double temp = Math.Pow(Math.Sin(deltaLat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(deltaLon / 2), 2);
            return EarthDiameter * Math.Atan2(Math.Sqrt(temp), Math.Sqrt(1 - temp));
        }
    }
}
