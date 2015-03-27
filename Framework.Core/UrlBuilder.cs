namespace Framework
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Web;

    using Framework.Collections;

    /// <summary>Provides a custom constructor for uniform resource identifiers (URIs) and modifies URIs for the <see cref="T:System.Uri"></see> class.</summary>
    public class UrlBuilder
    {
        private readonly OrderedDictionary<string, string> queryString = new OrderedDictionary<string, string>();

        /// <summary>Initializes a new instance of the <see cref="UrlBuilder"></see> class with the specified URL.</summary>
        /// <param name="url">A URL string. </param>
        /// <exception cref="T:System.ArgumentNullException">uri is null. </exception>
        /// <exception cref="T:System.UriFormatException">uri is a zero length string or contains only spaces.-or- The parsing routine detected a scheme in an invalid form.-or- The parser detected more than two consecutive slashes in a URI that does not use the "file" scheme.-or- uri is not a valid URI. </exception>
        public UrlBuilder(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            this.Populate(url);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="url">The URL to use.</param>
        public UrlBuilder(Uri url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            this.Populate(url.AbsoluteUri);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlBuilder"/> class.
        /// </summary>
        /// <param name="request">The request to use.</param>
        public UrlBuilder(HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            if (request.Url == null)
            {
                throw new ArgumentException("Request Url is Null", "request");
            }

            this.Populate(request.Url.AbsoluteUri);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the UrlBuilder class.
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        ///
        /// <param name="context">
        ///     The context.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public UrlBuilder(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (context.Request.Url == null)
            {
                throw new ArgumentException("Request Url is Null", "request");
            }

            this.Populate(context.Request.Url.AbsoluteUri);
        }

        /// <summary>
        /// Gets the url Path.
        /// </summary>
        /// <value>The url Path.</value>
        public string Url
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the query string.
        /// </summary>
        /// <value>The query string.</value>
        public OrderedDictionary<string, string> QueryString
        {
            get
            {
                return this.queryString;
            }
        }

        /// <summary>
        /// Appends the relative Url.
        /// </summary>
        /// <param name="relativeUrl">The url to append.</param>
        public void AppendUrl(string relativeUrl)
        {
            string path = UrlPath.AppendTrailingSlash(this.Url);

            string pathToAppend = UrlPath.RemoveTrailingSlash(relativeUrl);

            while (pathToAppend.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                if (pathToAppend.Length == 1)
                {
                    return;
                }

                pathToAppend = pathToAppend.Substring(1, pathToAppend.Length - 1);
            }

            this.Url = Path.Combine(path, pathToAppend);
        }

        /// <summary>
        /// Returns the display string for the specified <see cref="UrlBuilder"></see> instance.
        /// </summary>
        /// <param name="encoded">If set to <see langword="true"/> query will be encoded.</param>
        /// <returns>
        /// The string that contains the unescaped display string of the <see cref="T:System.UriBuilder"></see>.
        /// </returns>
        public string ToString(bool encoded)
        {
            return this.Url + this.GetQueryString(HttpUtility.UrlEncode);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the display string for the specified <see cref="UrlBuilder"></see> instance.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/15/2013 12:02 PM.
        /// </remarks>
        ///
        /// <param name="encoderFunc">
        ///     If set to <see langword="true"/> query will be encoded.
        /// </param>
        ///
        /// <returns>
        ///     The string that contains the unescaped display string of the
        ///     <see cref="T:System.UriBuilder"></see>.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public string ToString(Func<string, string> encoderFunc)
        {
            return this.Url + this.GetQueryString(encoderFunc);
        }

        /// <summary>Returns the display string for the specified <see cref="UrlBuilder"></see> instance.</summary>
        /// <returns>The string that contains the unescaped display string of the <see cref="T:System.UriBuilder"></see>.</returns>
        public override string ToString()
        {
            return this.ToString(false);
        }

        private string GetQueryString(Func<string, string> encoderFunc)
        {
            int count = this.QueryString.Count;

            if (count != 0)
            {
                string[] keys = new string[count];
                string[] values = new string[count];
                string[] encodedPairs = new string[count];

                this.QueryString.Keys.CopyTo(keys, 0);
                this.QueryString.Values.CopyTo(values, 0);

                for (int i = 0; i < count; i++)
                {
                    encodedPairs[i] = encoderFunc != null ? string.Concat(encoderFunc(keys[i]), "=", encoderFunc(values[i])) : string.Concat(keys[i], "=", values[i]);
                }

                return "?" + string.Join("&", encodedPairs);
            }

            return string.Empty;
        }


        private void Populate(string url)
        {
            int index = url.IndexOf("?", StringComparison.Ordinal);
            NameValueCollection nameValueCollection;
            if (index != -1)
            {
                this.Url = url.Substring(0, index);
                string querystring = url.Substring(index + 1, (url.Length - 1 - index));
                nameValueCollection = HttpUtility.ParseQueryString(querystring);
            }
            else
            {
                this.Url = url;
                nameValueCollection = new NameValueCollection();
            }

            foreach (string key in nameValueCollection.Keys)
            {
                this.QueryString.Add(key, nameValueCollection.Get(key));
            }
        }
    }
}
