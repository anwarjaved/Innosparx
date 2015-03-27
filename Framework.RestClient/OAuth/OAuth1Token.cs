using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Rest.OAuth
{
    public class OAuth1Token
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the token secret.
        /// </summary>
        /// <value>The token secret.</value>
        public string Secret { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether callback confirmed.
        /// </summary>
        /// <value>
        /// <see langword="true"/> If callback confirmed; otherwise, <see langword="false"/>.
        /// </value>
        /// <author>Anwar</author>
        /// <datetime>3/25/2011 12:52 PM</datetime>
        public bool OAuthCallbackConfirmed { get; set; }

        public long ExpiresIn { get; set; }

        public string AuthenticationUrl { get; set; }
    }
}
