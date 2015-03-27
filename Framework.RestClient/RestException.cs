using System;

namespace Framework.Rest
{
    using System.Net;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Exception for signalling rest errors.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public class RestException : Exception
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the status code.
        /// </summary>
        ///
        /// <value>
        ///     The status code.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public HttpStatusCode StatusCode { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the RestException class.
        /// </summary>
        ///
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="statusCode">
        ///     The status code.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            this.StatusCode = statusCode;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the RestException class.
        /// </summary>
        ///
        /// <param name="statusCode">
        ///     The status code.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestException(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the RestException class.
        /// </summary>
        ///
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="statusCode">
        ///     The status code.
        /// </param>
        /// <param name="innerException">
        ///     The inner exception.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public RestException(string message, HttpStatusCode statusCode, Exception innerException)
            : base(message, innerException)
        {
            this.StatusCode = statusCode;
        }
    }
}
