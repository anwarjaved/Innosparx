using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookSDK
{
    public class PostResponse : BaseResponse
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        ///
        /// <value>
        ///     The message.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Message { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the link.
        /// </summary>
        ///
        /// <value>
        ///     The link.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Link { get; set; }
    }
}
