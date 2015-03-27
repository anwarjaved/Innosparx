using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    using System.Net;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Exception for signalling API errors.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public class ApiException : Exception
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
        ///     Initializes a new instance of the ApiException class.
        /// </summary>
        ///
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="statusCode">
        ///     The status code.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public ApiException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}
