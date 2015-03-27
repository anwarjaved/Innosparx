using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Rest.OAuth;
using YahooSDK.Contacts;

namespace YahooSDK
{
    using System.Threading;

    using Framework;
    using Framework.Rest;

    public class YahooClient : OAuth1Client<YahooTokenCredential>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth1Client"/> class.
        /// </summary>
        /// <param name="appKey">The application key.</param>
        /// <param name="appSecret">The application secret.</param>
        /// <author>Anwar</author>
        /// <datetime>3/24/2011 2:21 PM</datetime>
        public YahooClient(string appKey, string appSecret)
            : base(appKey, appSecret)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the OAuth1Client class.
        /// </summary>
        ///
        /// <param name="appKey">
        ///     The application key.
        /// </param>
        /// <param name="appSecret">
        ///     The application secret.
        /// </param>
        /// <param name="credential">
        ///     The credential.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public YahooClient(string appKey, string appSecret, YahooTokenCredential credential)
            : base(appKey, appSecret, credential)
        {
        }

        public OAuth1TempCredential GetRequestToken(string callbackUrl = null)
        {
            return this.GetRequestToken<OAuth1TempCredential>(YahooConstants.RequestToken, callbackUrl);
        }

        public YahooTokenCredential GetAccessToken(OAuth1Token requestToken, string verifier)
        {
            return this.GetAccessToken(requestToken, YahooConstants.AccessToken, verifier);
        }

        public YahooTokenCredential GetRefreshToken()
        {
            var credential = this.GetRefreshToken(this.Credential, YahooConstants.AccessToken);

            if (credential != null)
            {
                this.Credential = credential;
            }

            return credential;
        }

        public RestResponse<YahooDocument> GetAllContacts(string yahooID, int count = 5000)
        {
            RestRequest request = new RestRequest(YahooConstants.AllContactsApi.FormatString(yahooID, count), RequestMode.Json, AcceptMode.Json);
            return this.Get<YahooDocument>(request);
        }
    }
}