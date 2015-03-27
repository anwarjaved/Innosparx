namespace Framework.Configuration
{
    using System.Web;

    public class YahooApiSetting
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the consumer key.
        /// </summary>
        /// <value>
        ///     The consumer key.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public string AppKey { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the consumer secret.
        /// </summary>
        /// <value>
        ///     The consumer secret.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public string AppSecret { get; set; }

        public string Domain { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets authentication URL.
        /// </summary>
        /// <param name="successUrl">
        ///     URL of the success.
        /// </param>
        /// <param name="failureUrl">
        ///     URL of the failure.
        /// </param>
        /// <returns>
        ///     The authentication URL.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public IHtmlString GetAuthenticationUrl(string successUrl, string failureUrl)
        {
            return this.GetAuthenticationUrl(successUrl, failureUrl, null);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets authentication URL.
        /// </summary>
        /// <remarks>
        ///     Anwar Javed, 09/14/2013 5:31 PM.
        /// </remarks>
        /// <param name="successUrl">
        ///     URL of the success.
        /// </param>
        /// <param name="failureUrl">
        ///     URL of the failure.
        /// </param>
        /// <param name="state">
        ///     The state.
        /// </param>
        /// <returns>
        ///     The authentication URL.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public IHtmlString GetAuthenticationUrl(string successUrl, string failureUrl, string state)
        {
            var builder = new UrlBuilder(SocialApiSetting.BuildUrl(this.Domain, "social/yahoo/authenticate"));
            builder.QueryString.Add("success", successUrl);
            builder.QueryString.Add("failure", failureUrl);

            if (!string.IsNullOrWhiteSpace(state))
            {
                builder.QueryString.Add("state", state);
            }

            return new HtmlString(builder.ToString(false));
        }
    }
}