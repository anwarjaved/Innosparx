namespace Framework.Rest
{
    /// <summary>
    /// Reprsesnts Generic Response object.
    /// </summary>
    /// <typeparam name="T">Type of Data.</typeparam>
    /// <author>Anwar</author>
    /// <datetime>3/24/2011 2:42 PM</datetime>
    public class RestResponse<T> : RestResponse
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestResponse&lt;T&gt;"/> class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public RestResponse()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="RestResponse&lt;T&gt;"/> class.
        /// </summary>
        ///
        /// <param name="response">
        ///     The response.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestResponse(RestResponse response)
        {
            ContentEncoding = response.ContentEncoding;
            ContentLength = response.ContentLength;
            ContentType = response.ContentType;
            Cookies = response.Cookies;
            ErrorMessage = response.ErrorMessage;
            Headers = response.Headers;
            this.ContentArray = response.ContentArray;
            this.Status = response.Status;
            ResponseUri = response.ResponseUri;
            Server = response.Server;
            StatusCode = response.StatusCode;
            StatusDescription = response.StatusDescription;
            ResponseMode = response.ResponseMode;
            this.Request = response.Request;
        }

        /// <summary>
        /// Gets or sets the content object.
        /// </summary>
        /// <value>The data of response.</value>
        /// <author>Anwar</author>
        /// <datetime>3/24/2011 2:43 PM</datetime>
        public T ContentObject { get; protected internal set; }
    }
}
