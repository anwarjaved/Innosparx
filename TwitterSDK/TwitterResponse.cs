using System;
using System.Collections.Generic;
using System.Text;

namespace TwitterSDK
{
    using Framework.Rest;

    /// <summary>
    /// Represents Twitter Response.
    /// </summary>
    /// <typeparam name="T">Type of Twitter Response. </typeparam>
    public class TwitterResponse<T> : RestResponse<T> where T : TwitterObject, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterResponse&lt;T&gt;"/> class.
        /// </summary>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 4:54 PM</datetime>
        public TwitterResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterResponse&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <author>Anwar</author>
        /// <datetime>3/26/2011 4:54 PM</datetime>
        public TwitterResponse(RestResponse<T> response)
            : base(response)
        {
            this.ContentObject = response.ContentObject;
        }
    }
}

