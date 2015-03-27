namespace Framework.Fakes
{
    using System.Text;
    using System.Web;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Fake HTTP response.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public class FakeHttpResponse : HttpResponseBase
    {
        private readonly HttpCookieCollection cookies = new HttpCookieCollection();
        private readonly StringBuilder outputString = new StringBuilder();

        /// <summary>
        /// When overridden in a derived class, gets or sets the value of the HTTP Location header.
        /// </summary>
        /// <value>The redirect location.</value>
        /// <returns>The absolute URL of the HTTP Location header.</returns>
        public override string RedirectLocation { get; set; }

       /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the response output.
        /// </summary>
        /// <value>
        /// The response output.
        /// </value>
        /// --------------------------------------------------------------------------------------------------
        public string ResponseOutput
        {
            get
            {
                return this.outputString.ToString();
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the response cookie collection.
        /// </summary>
        /// <value>The cookies.</value>
        /// <returns>The response cookie collection.</returns>
        public override HttpCookieCollection Cookies
        {
            get
            {
                return this.cookies;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the HTTP status code of the output that is returned to the client.
        /// </summary>
        /// <value>The status code.</value>
        /// <returns>The status code of the HTTP output that is returned to the client. For information about valid status codes, see HTTP Status Codes on the MSDN Web site.</returns>
        public override int StatusCode { get; set; }

        /// <summary>
        /// When overridden in a derived class, adds a session ID to the virtual path if the session is using <see cref="P:System.Web.Configuration.SessionStateSection.Cookieless" /> session state, and returns the combined path.
        /// </summary>
        /// <param name="virtualPath">The virtual path of a resource.</param>
        /// <returns>The virtual path, with the session ID inserted.</returns>
        public override string ApplyAppPathModifier(string virtualPath)
        {
            return virtualPath;
        }

        /// <summary>
        /// When overridden in a derived class, writes the specified string to the HTTP response output stream.
        /// </summary>
        /// <param name="s">The string to write to the HTTP output stream.</param>
        public override void Write(string s)
        {
            this.outputString.Append(s);
        }
    }
}
