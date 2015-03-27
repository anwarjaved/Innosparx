using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSDK
{
    using Framework.Rest;

    public class GoogleApiClient : RestClient
    {
        private readonly string apikey;

        public GoogleApiClient(string apikey)
        {
            this.apikey = apikey;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes.
        /// </summary>
        ///
        /// <param name="request">
        ///     The request to get.
        /// </param>
        /// <param name="method">
        ///     The method.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        protected override RestResponse Execute(RestRequest request, MethodType method)
        {
            request.Parameters.Add("key", apikey);
            return base.Execute(request, method);
        }

        protected override Task<RestResponse> ExecuteAsync(RestRequest request)
        {
            request.Parameters.Add("key", apikey);
            return base.ExecuteAsync(request);
        }
    }
}
