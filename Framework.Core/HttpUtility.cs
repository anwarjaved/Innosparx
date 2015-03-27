namespace Framework
{
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     HTTP utility.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/04/2013 4:27 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public static class HttpUtility
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Perform RFC 3986 Percent-encoding on a string.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/04/2013 4:28 PM.
        /// </remarks>
        ///
        /// <param name="input">
        ///     The input string.
        /// </param>
        ///
        /// <returns>
        ///     The RFC 3986 Percent-encoded string.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string UrlEncode(string input)
        {
            return Rfc3986Parser.Encode(input);
        }

        /// <summary>
        /// Perform RFC 3986 Percent-encoding on a string.
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>The RFC 3986 Percent-encoded string</returns>
        public static byte[] UrlEncodeToBytes(string input)
        {
            return Rfc3986Parser.EncodeToBytes(input);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Perform RFC 3986 Percent-decoding on a string.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/04/2013 4:28 PM.
        /// </remarks>
        ///
        /// <param name="input">
        ///     The input RFC 3986 Percent-encoded string.
        /// </param>
        ///
        /// <returns>
        ///     The decoded string.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string UrlDecode(string input)
        {
            return Rfc3986Parser.Decode(input);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A NameValueCollection extension method that converts a nvc to a query string.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/04/2013 4:21 PM.
        /// </remarks>
        ///
        /// <param name="nvc">
        ///     The nvc to act on.
        /// </param>
        ///
        /// <returns>
        ///     nvc as a string.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string ToQueryString(this NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", Rfc3986Parser.Encode(key), Rfc3986Parser.Encode(value)))
                .ToArray();
            return string.Join("&", array);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Parse query string.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/04/2013 4:32 PM.
        /// </remarks>
        ///
        /// <param name="query">
        ///     The query.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static NameValueCollection ParseQueryString(string query)
        {
            return ParseQueryString(query, Encoding.UTF8);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Parse query string.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/04/2013 4:32 PM.
        /// </remarks>
        ///
        /// <param name="query">
        ///     The query.
        /// </param>
        /// <param name="encoding">
        ///     The encoding.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static NameValueCollection ParseQueryString(string query, Encoding encoding)
        {
            return System.Web.HttpUtility.ParseQueryString(query, Encoding.UTF8);
        }



    }
}
