namespace Framework
{
    using System;
    using System.ComponentModel;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     <see cref="Guid"/> Extensions.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class GuidExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// A GUID extension method that converts a value to a string value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// value as a string.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static string ToStringValue(this Guid value)
        {
            return value.ToString("N").ToLowerInvariant();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// A GUID extension method that converts a value to a short unique identifier.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// value as a string.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static string ToShortGuid(this Guid value)
        {
            if (value == Guid.Empty)
            {
                value = Guid.NewGuid();
            }

            return Convert.ToBase64String(value.ToByteArray())
                .Substring(0, 22)
                .Replace("/", "_")
                .Replace("+", "-");
        }

        /// <summary>
        /// A string extension method that parse short unique identifier.
        /// </summary>
        /// <param name="guid">The GUID to act on.</param>
        /// <returns>Parsed GUID.</returns>
        public static Guid ParseShortGuid(this string guid)
        {
            return !string.IsNullOrEmpty(guid)
                       ? new Guid(Convert.FromBase64String(guid.Replace("_", "/").Replace("-", "+") + "=="))
                       : Guid.Empty;
        }

        /// <summary>
        /// Get a globally unique identifier (GUID) with database
        /// performance optimization.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Globally unique identifier (GUID) with database
        /// performance optimization.</returns>
        /// <remarks>
        /// same as <see cref="Guid"/> but with optimization for database
        /// according to the article InformIT: The Cost of GUIDs as Primary Keys
        /// at http://www.informit.com/articles/article.aspx?p=25862
        /// </remarks>
        public static Guid ToCombGuid(this Guid value)
        {
            if (value == Guid.Empty)
            {
                value = Guid.NewGuid();
            }

            byte[] guidArray = value.ToByteArray();

            var baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.UtcNow;

            // Get the days and milliseconds which will be used to build the byte string
            var days = new TimeSpan(now.Ticks - baseDate.Ticks);
            var msecs = new TimeSpan(now.Ticks - new DateTime(now.Year, now.Month, now.Day).Ticks);

            // Convert to a byte array
            // Note that SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333
            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            // Copy the bytes into the guid
            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }
    }
}
