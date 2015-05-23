using System;

namespace Framework
{
    using System.Data.Entity.Spatial;
    using System.Globalization;
    using System.Security;

    /// <summary>
    /// Geolocation helpers for Entity Framework types
    /// </summary>
    
    public static class GeoUtils
    {
        /// <summary>
        /// Create a GeoLocation point based on latitude and longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public static DbGeography CreatePoint(double latitude, double longitude)
        {
            var text = string.Format(CultureInfo.InvariantCulture.NumberFormat,
                                     "POINT({0} {1})", longitude, latitude);
            // 4326 is most common coordinate system used by GPS/Maps
            return DbGeography.PointFromText(text, 4326);
        }

        /// <summary>
        /// Create a GeoLocation point based on latitude and longitude
        /// </summary>
        /// <param name="latitudeLongitude">
        /// String should be two values either single comma or space delimited
        /// 45.710030,-121.516153
        /// 45.710030 -121.516153
        /// </param>
        /// <returns></returns>
        public static DbGeography CreatePoint(string latitudeLongitude)
        {
            var tokens = latitudeLongitude.Split(',', ' ');
            if (tokens.Length != 2)
                throw new ArgumentException("Invalid Location String Passed");
            var text = string.Format(CultureInfo.InvariantCulture.NumberFormat,
                                     "POINT({0} {1})", tokens[1], tokens[0]);
            return DbGeography.PointFromText(text, 4326);
        }


        /// <summary>
        /// Convert meters to miles
        /// </summary>
        /// <param name="meters"></param>
        /// <returns></returns>
        public static double MetersToMiles(double? meters)
        {
            if (meters == null)
                return 0F;

            return meters.Value * 0.000621371192;
        }

        /// <summary>
        /// Convert meters to feet
        /// </summary>
        /// <param name="meters"></param>
        /// <returns></returns>
        public static double MetersToFeet(double? meters)
        {
            if (meters == null)
                return 0;

            return meters.Value * 3.2808399;
        }

        /// <summary>
        /// Convert miles to meters
        /// </summary>
        /// <param name="miles"></param>
        /// <returns></returns>
        public static double MilesToMeters(double? miles)
        {
            if (miles == null)
                return 0;

            return miles.Value * 1609.344;
        }


        /// <summary>
        /// Displays a miles value as a string with a mile postfix
        /// 1.0
        /// </summary>
        /// <param name="meters"></param>
        /// <returns></returns>
        public static string DisplayMiles(double? meters)
        {
            var miles = MetersToMiles(meters);
            if (miles <= 0)
            {
                return string.Empty;
            }

            if (miles <= 1)
            {
                return string.Format("{0:n2} mile", miles);
            }

            if (miles < 10)
            {
                return string.Format("{0:n2} miles", miles);
            }

            return string.Format("{0:n0} miles", miles);
        }
    }
}
