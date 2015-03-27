namespace Framework.Fakes
{
    using System;
    using System.Collections.Specialized;
    using System.Web;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Fake HTTP request.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public class FakeHttpRequest : HttpRequestBase
    {
        private readonly HttpCookieCollection cookies;
        private readonly NameValueCollection formParams;
        private readonly string httpMethod;
        private readonly NameValueCollection queryStringParams;
        private readonly string relativeUrl;
        private readonly NameValueCollection serverVariables;
        private readonly Uri url;
        private readonly Uri urlReferrer;

       /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the FakeHttpRequest class.
        /// </summary>
        /// <param name="relativeUrl">
        /// URL of the relative.
        /// </param>
        /// <param name="url">
        /// URL of the document.
        /// </param>
        /// <param name="urlReferrer">
        /// The URL referrer.
        /// </param>
        /// --------------------------------------------------------------------------------------------------
        public FakeHttpRequest(string relativeUrl, Uri url, Uri urlReferrer)
            : this(relativeUrl, "Get", url, urlReferrer, null, null, null, null)
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the FakeHttpRequest class.
        /// </summary>
        /// <param name="relativeUrl">
        /// URL of the relative.
        /// </param>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="formParams">
        /// Options for controlling the form.
        /// </param>
        /// <param name="queryStringParams">
        /// Options for controlling the query string.
        /// </param>
        /// <param name="cookies">
        /// The cookies.
        /// </param>
        /// <param name="serverVariables">
        /// The server variables.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public FakeHttpRequest(string relativeUrl, string method, NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies, NameValueCollection serverVariables)
        {
            this.httpMethod = method;
            this.relativeUrl = relativeUrl;
            this.formParams = formParams;
            this.queryStringParams = queryStringParams;
            this.cookies = cookies;
            this.serverVariables = serverVariables;
            if (this.formParams == null)
            {
                this.formParams = new NameValueCollection();
            }

            if (this.queryStringParams == null)
            {
                this.queryStringParams = new NameValueCollection();
            }

            if (this.cookies == null)
            {
                this.cookies = new HttpCookieCollection();
            }

            if (this.serverVariables == null)
            {
                this.serverVariables = new NameValueCollection();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the FakeHttpRequest class.
        /// </summary>
        /// <param name="relativeUrl">
        /// URL of the relative.
        /// </param>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="url">
        /// URL of the document.
        /// </param>
        /// <param name="urlReferrer">
        /// The URL referrer.
        /// </param>
        /// <param name="formParams">
        /// Options for controlling the form.
        /// </param>
        /// <param name="queryStringParams">
        /// Options for controlling the query string.
        /// </param>
        /// <param name="cookies">
        /// The cookies.
        /// </param>
        /// <param name="serverVariables">
        /// The server variables.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public FakeHttpRequest(string relativeUrl, string method, Uri url, Uri urlReferrer, NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies, NameValueCollection serverVariables)
            : this(relativeUrl, method, formParams, queryStringParams, cookies, serverVariables)
        {
            this.url = url;
            this.urlReferrer = urlReferrer;
        }

        /// <summary>
        /// When overridden in a derived class, gets the virtual root path of the ASP.NET application on the server.
        /// </summary>
        /// <value>The application path.</value>
        /// <returns>The virtual path of the current application.</returns>
        public override string ApplicationPath
        {
            get
            {
                if ((this.relativeUrl != null) && this.relativeUrl.StartsWith("~/"))
                {
                    return this.relativeUrl.Remove(0, 1);
                }

                return null;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the virtual path of the application root and makes it relative by using the tilde (~) notation for the application root.
        /// </summary>
        /// <value>The application relative current execution file path.</value>
        /// <returns>The virtual path of the application root for the current request with the tilde operator (~) added.</returns>
        public override string AppRelativeCurrentExecutionFilePath
        {
            get
            {
                return this.relativeUrl;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the collection of cookies that were sent by the client.
        /// </summary>
        /// <value>The cookies.</value>
        /// <returns>The client's cookies.</returns>
        public override HttpCookieCollection Cookies
        {
            get
            {
                return this.cookies;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the collection of form variables that were sent by the client.
        /// </summary>
        /// <value>The form.</value>
        /// <returns>The form variables.</returns>
        public override NameValueCollection Form
        {
            get
            {
                return this.formParams;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the HTTP data-transfer method (such as GET, POST, or HEAD) that was used by the client.
        /// </summary>
        /// <value>The HTTP method.</value>
        /// <returns>The HTTP data-transfer method that was used by the client.</returns>
        public override string HttpMethod
        {
            get
            {
                return this.httpMethod;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value that indicates whether the request has been authenticated.
        /// </summary>
        /// <value><c>true</c> if this instance is authenticated; otherwise, <c>false</c>.</value>
        /// <returns>true if the request has been authenticated; otherwise, false.</returns>
        public override bool IsAuthenticated
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value that indicates whether the HTTP connection uses secure sockets (HTTPS protocol).
        /// </summary>
        /// <value><c>true</c> if this instance is secure connection; otherwise, <c>false</c>.</value>
        /// <returns>true if the connection is an SSL connection that uses HTTPS protocol; otherwise, false.</returns>
        public override bool IsSecureConnection
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets additional path information for a resource that has a URL extension.
        /// </summary>
        /// <value>The path information.</value>
        /// <returns>The additional path information for the resource.</returns>
        public override string PathInfo
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the collection of HTTP query-string variables.
        /// </summary>
        /// <value>The query string.</value>
        /// <returns>The query-string variables that were sent by the client in the URL of the current request. </returns>
        public override NameValueCollection QueryString
        {
            get
            {
                return this.queryStringParams;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the complete URL of the current request.
        /// </summary>
        /// <value>The raw URL.</value>
        /// <returns>The complete URL of the current request.</returns>
        public override string RawUrl
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a collection of Web server variables.
        /// </summary>
        /// <value>The server variables.</value>
        public override NameValueCollection ServerVariables
        {
            get
            {
                return this.serverVariables;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets information about the URL of the current request.
        /// </summary>
        /// <value>The URL.</value>
        /// <returns>An object that contains information about the URL of the current request.</returns>
        public override Uri Url
        {
            get
            {
                return this.url;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets information about the URL of the client request that linked to the current URL.
        /// </summary>
        /// <value>The URL referrer.</value>
        /// <returns>The URL of the page that linked to the current request.</returns>
        public override Uri UrlReferrer
        {
            get
            {
                return this.urlReferrer;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the IP host address of the client.
        /// </summary>
        /// <value>The user host address.</value>
        /// <returns>The IP address of the client.</returns>
        public override string UserHostAddress
        {
            get
            {
                return null;
            }
        }
    }
}