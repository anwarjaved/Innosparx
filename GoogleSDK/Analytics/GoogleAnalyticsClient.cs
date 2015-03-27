using System.Collections.Generic;

namespace GoogleSDK.Analytics
{
    using System;
    using System.Linq;
    using System.Security.Policy;

    using Framework;
    using Framework.Rest;
    using Framework.Rest.OAuth;

    public class GoogleAnalyticsClient : GoogleClient
    {
        public GoogleAnalyticsClient(string appID, string appSecret)
            : base(appID, appSecret)
        {
        }

        public GoogleAnalyticsClient(string appID, string appSecret, OAuth2TokenCredential credential)
            : base(appID, appSecret, credential)
        {
        }

        public GoogleAnalyticsClient(string appID, string appSecret, string token)
            : base(appID, appSecret, token)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds authorization URL.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 12/12/2012.
        /// </remarks>
        ///
        /// <param name="redirectUrl">
        ///     URL of the redirect.
        /// </param>
        /// <param name="scope">
        ///     (optional) the scope.
        /// </param>
        /// <param name="state">
        ///     (optional) the state.
        /// </param>
        /// <param name="parameters">
        ///     (optional) options for controlling the operation.
        /// </param>
        ///
        /// <returns>
        ///     The URL used when authenticating a user.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public override string BuildAuthorizationUrl(string redirectUrl, IEnumerable<string> scope = null, string state = "", IDictionary<string, string> parameters = null)
        {
            if (scope == null)
            {
                scope = new List<string>();
            }

            List<string> list = new List<string>(scope);

            if (!list.Contains(Scopes.GoogleAnalytics))
            {
                list.Add(Scopes.GoogleAnalytics);
            }

            return base.BuildAuthorizationUrl(redirectUrl, list, state, parameters);
        }

        public RestResponse<PageTrackingResponse> GetPageTracking(string profileID, string relativeUrl, DateTime? startDate = null, DateTime? endDate = null)
        {
            RestRequest request = new RestRequest(GoogleConstants.GoogleAnalyticsUrl, RequestMode.UrlEncoded, AcceptMode.Json);


            request.Parameters.Add("ids", "ga:" + profileID);
            request.Parameters.Add("start-date", startDate.HasValue ? startDate.Value.ToString("yyyy-MM-dd") : "30daysAgo");
            request.Parameters.Add("end-date", endDate.HasValue ? endDate.Value.ToString("yyyy-MM-dd") : "today");
            request.Parameters.Add("metrics", new[] { MetricesType.UniquePageViews, MetricesType.PageValue, 
                MetricesType.Entrances,MetricesType.PageViews, MetricesType.TimeOnPage, 
            MetricesType.Exits, MetricesType.EntranceRate, MetricesType.PageViewsPerVisit,
            MetricesType.AverageTimeOnPage, MetricesType.ExitRate}.Select(x => x.ToDescription()).ToConcatenatedString(","));
            request.Parameters.Add("filters", "ga:pagePath=~" + UrlPath.AppendLeadingSlash(relativeUrl));

            return this.Get<PageTrackingResponse>(request);
        }
    }
}
