namespace Framework.Fakes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Security.Principal;
    using System.Web;
    using System.Web.SessionState;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Fake HTTP context.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public class FakeHttpContext : HttpContextBase
    {
        private readonly HttpCookieCollection cookies;
        private readonly NameValueCollection formParams;
        private readonly Dictionary<object, object> items;
        private readonly string method;
        private readonly NameValueCollection queryStringParams;
        private readonly string relativeUrl;
        private readonly NameValueCollection serverVariables;
        private readonly SessionStateItemCollection sessionItems;
        private IPrincipal principal;
        private HttpRequestBase request;
        private HttpResponseBase response;

       /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the FakeHttpContext class.
        /// </summary>
        /// <param name="relativeUrl">
        /// URL of the relative.
        /// </param>
        /// --------------------------------------------------------------------------------------------------
        public FakeHttpContext(string relativeUrl)
            : this(relativeUrl, null, null, null, null, null, null)
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the FakeHttpContext class.
        /// </summary>
        /// <param name="relativeUrl">
        /// URL of the relative.
        /// </param>
        /// <param name="method">
        /// The method.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public FakeHttpContext(string relativeUrl, string method)
            : this(relativeUrl, method, null, null, null, null, null, null)
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the FakeHttpContext class.
        /// </summary>
        /// <param name="relativeUrl">
        /// URL of the relative.
        /// </param>
        /// <param name="principal">
        /// The principal.
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
        /// <param name="sessionItems">
        /// The session items.
        /// </param>
        /// <param name="serverVariables">
        /// The server variables.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public FakeHttpContext(string relativeUrl, IPrincipal principal, NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies, SessionStateItemCollection sessionItems, NameValueCollection serverVariables)
            : this(relativeUrl, null, principal, formParams, queryStringParams, cookies, sessionItems, serverVariables)
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the FakeHttpContext class.
        /// </summary>
        /// <param name="relativeUrl">
        /// URL of the relative.
        /// </param>
        /// <param name="method">
        /// The method.
        /// </param>
        /// <param name="principal">
        /// The principal.
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
        /// <param name="sessionItems">
        /// The session items.
        /// </param>
        /// <param name="serverVariables">
        /// The server variables.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public FakeHttpContext(string relativeUrl, string method, IPrincipal principal, NameValueCollection formParams, NameValueCollection queryStringParams, HttpCookieCollection cookies, SessionStateItemCollection sessionItems, NameValueCollection serverVariables)
        {
            this.relativeUrl = relativeUrl;
            this.method = method;
            this.principal = principal;
            this.formParams = formParams;
            this.queryStringParams = queryStringParams;
            this.cookies = cookies;
            this.sessionItems = sessionItems;
            this.serverVariables = serverVariables;
            this.items = new Dictionary<object, object>();
        }

        /// <summary>
        /// When overridden in a derived class, gets a key/value collection that can be used to organize and share data between a module and a handler during an HTTP request.
        /// </summary>
        /// <value>The items.</value>
        /// <returns>A key/value collection that provides access to an individual value in the collection by using a specified key.</returns>
        public override IDictionary Items
        {
            get
            {
                return this.items;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the <see cref="T:System.Web.HttpRequest" /> object for the current HTTP request.
        /// </summary>
        /// <value>The request.</value>
        /// <returns>The current HTTP request.</returns>
        public override HttpRequestBase Request
        {
            get
            {
                return this.request ?? new FakeHttpRequest(this.relativeUrl, this.method, this.formParams, this.queryStringParams, this.cookies, this.serverVariables);
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the <see cref="T:System.Web.HttpResponse" /> object for the current HTTP response.
        /// </summary>
        /// <value>The response.</value>
        /// <returns>The current HTTP response.</returns>
        public override HttpResponseBase Response
        {
            get
            {
                return this.response ?? new FakeHttpResponse();
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the <see cref="T:System.Web.SessionState.HttpSessionState" /> object for the current HTTP request.
        /// </summary>
        /// <value>The session.</value>
        /// <returns>The session-state object for the current HTTP request.</returns>
        public override HttpSessionStateBase Session
        {
            get
            {
                return new FakeHttpSessionState(this.sessionItems);
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets a value that specifies whether the <see cref="T:System.Web.Security.UrlAuthorizationModule" /> object should skip the authorization check for the current request.
        /// </summary>
        /// <value><c>true</c> if [skip authorization]; otherwise, <c>false</c>.</value>
        /// <returns>true if <see cref="T:System.Web.Security.UrlAuthorizationModule" /> should skip the authorization check; otherwise, false. </returns>
        public override bool SkipAuthorization { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public override IPrincipal User
        {
            get
            {
                return this.principal;
            }

            set
            {
                this.principal = value;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <returns>
        /// Returns Root Context.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static FakeHttpContext Root()
        {
            return new FakeHttpContext("~/");
        }
        
        /// <summary>
        /// When overridden in a derived class, returns an object for the current service type.
        /// </summary>
        /// <param name="serviceType">The type of service object to get.</param>
        /// <returns>The current service type, or null if no service is found.</returns>
        public override object GetService(Type serviceType)
        {
            return null;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets a request.
        /// </summary>
        /// <param name="newRequest">
        /// The request.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public void SetRequest(HttpRequestBase newRequest)
        {
            this.request = newRequest;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets a response.
        /// </summary>
        /// <param name="newResponse">
        /// The response.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public void SetResponse(HttpResponseBase newResponse)
        {
            this.response = newResponse;
        }
    }
}
