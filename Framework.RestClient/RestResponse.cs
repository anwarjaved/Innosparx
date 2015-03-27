namespace Framework.Rest
{
    using System;
    using System.Net;

    /// <summary>
    /// Rest Response.
    /// </summary>
    /// <author>Anwar</author>
    /// <datetime>3/22/2011 11:23 AM</datetime>
    public class RestResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestResponse"/> class.
        /// </summary>
        /// <author>Anwar</author>
        /// <datetime>3/22/2011 11:24 AM</datetime>
        internal RestResponse()
        {
            this.Cookies = new CookieCollection();
            this.Headers = new WebHeaderCollection();
            this.Status = ResponseStatus.None;
            this.ResponseMode = Rest.ResponseMode.None;
        }

        /// <summary>
        /// Gets String representation of response content.
        /// </summary>
        /// <value>The content.</value>
        public string Content
        {
            get
            {
                if (this.ContentArray != null && this.ContentArray.Length > 0)
                {
                    return this.ContentArray.GetString();
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets Encoding of the response content.
        /// </summary>
        /// <value>The content encoding.</value>
        public string ContentEncoding { get; internal set; }

        /// <summary>
        /// Gets Length in bytes of the response content.
        /// </summary>
        /// <value>The length of the content.</value>
        public long ContentLength { get; internal set; }

        /// <summary>
        /// Gets MIME content type of response.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType { get; internal set; }

        /// <summary>
        /// Gets Response mode.
        /// </summary>
        /// <value>The type of the response.</value>
        public ResponseMode ResponseMode { get; internal set; }

        /// <summary>
        /// Gets the Cookies returned by server with the response.
        /// </summary>
        /// <value>The cookies.</value>
        public CookieCollection Cookies { get; internal set; }

        /// <summary>
        /// Gets The exception thrown during the request, if any.
        /// </summary>
        /// <value>The error exception.</value>
        public RestException ErrorException { get; internal set; }

        /// <summary>
        /// Gets Transport or other non-HTTP error generated while attempting request.
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage { get; internal set; }

        /// <summary>
        /// Gets Headers returned by server with the response.
        /// </summary>
        /// <value>The headers.</value>
        public WebHeaderCollection Headers { get; internal set; }

        /// <summary>
        /// Gets Response content.
        /// </summary>
        /// <value>The raw bytes.</value>
        public byte[] ContentArray { get; internal set; }

        /// <summary>
        /// Gets The RestRequest that was made to get this RestResponse.
        /// </summary>
        /// <value>The request.</value>
        /// <remarks>
        /// Mainly for debugging if ResponseStatus is not OK.
        /// </remarks>
        public RestRequest Request { get; internal set; }

        /// <summary>
        /// Gets Status of the request. Will return Error for transport errors.
        /// HTTP errors will still return ResponseStatus.Completed, check StatusCode instead.
        /// </summary>
        /// <value>The response status.</value>
        public ResponseStatus Status { get; internal set; }

        /// <summary>
        /// Gets The URL that actually responded to the content (different from request if redirected).
        /// </summary>
        /// <value>The response URI.</value>
        public Uri ResponseUri { get; internal set; }

        /// <summary>
        /// Gets HttpWebResponse.Server.
        /// </summary>
        /// <value>The server.</value>
        public string Server { get; internal set; }

        /// <summary>
        /// Gets HTTP response status code.
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatusCode StatusCode { get; internal set; }

        /// <summary>
        /// Gets Description of HTTP status returned.
        /// </summary>
        /// <value>The status description.</value>
        public string StatusDescription { get; internal set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a value indicating whether the completed.
        /// </summary>
        ///
        /// <value>
        ///     true if completed, false if not.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public bool Completed
        {
            get
            {
                return this.Status == ResponseStatus.Completed;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets The date and time Server responded, for example, Wed, 01 Mar 2009 12:00:00 GMT..
        /// </summary>
        ///
        /// <value>
        ///    The date and time Server responded, for example, Wed, 01 Mar 2009 12:00:00 GMT.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Date
        {
            get
            {
                return this.Headers[HttpHeaders.Date];
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets The entity tag is a hash of the object.
        /// </summary>
        ///
        /// <value>
        ///     The entity tag is a hash of the object.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string ETag
        {
            get
            {
                return this.Headers[HttpHeaders.ETag];
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Specifies whether the connection to the server is open or closed.
        /// </summary>
        ///
        /// <value>
        ///    Specifies whether the connection to the server is open or closed.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Connection
        {
            get
            {
                return this.Headers[HttpHeaders.Connection];
            }
        }
    }
}
