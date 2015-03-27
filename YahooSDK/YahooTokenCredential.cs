using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Rest;
using Framework.Rest.OAuth;

namespace YahooSDK
{
    public class YahooTokenCredential : OAuth1TokenCredential
    {
        public YahooTokenCredential()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth1TokenCredential"/> class.
        /// </summary>
        public YahooTokenCredential(string token, string secret) : base(token, secret)
        {
        }

        public YahooTokenCredential(string token, string secret, string yahooID, string sessionHandle)
            : base(token, secret)
        {
            this.YahooID = yahooID;
            this.SessionHandle = sessionHandle;
        }

        public YahooTokenCredential(OAuth1TokenCredential credential)
        {
            this.ExpiresIn = credential.ExpiresIn;
            this.Secret = credential.Secret;
            this.Token = credential.Token;
        }


        public string YahooID { get; set; }

        protected override void Deserialize(string value)
        {
            base.Deserialize(value);

            this.YahooID = QueryParameter.ParseQuerystringParameter(YahooConstants.YahooGuid, value);


           
        }
    }
}
