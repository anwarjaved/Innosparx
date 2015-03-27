namespace Framework.Configuration
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Web;

    using Framework.Web;

    public sealed class ApplicationSetting
    {
        public string CopyrightText { get; set; }

        public string Domain { get; set; }

        public string AdminDomain { get; set; }

        public string ExtraDomain { get; set; }

        public bool EnableTurboLinks { get; set; }

        public bool EnableFormRedirection { get; set; }

        public string Name { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets URL of the API.
        /// </summary>
        /// <value>
        ///     The API URL.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public string ApiDomain { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether the localization is enabled.
        /// </summary>
        /// <value>
        ///     true if localization enabled, false if not.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public bool LocalizationEnabled { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds API URL.
        /// </summary>
        /// <param name="resource">
        ///     The resource.
        /// </param>
        /// <returns>
        ///     .
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public IHtmlString BuildApiUrl(string resource)
        {
            if (!string.IsNullOrWhiteSpace(this.ApiDomain) && !string.IsNullOrWhiteSpace(resource))
            {
                return new HtmlString(UrlPath.Combine(this.ApiDomain, resource));
            }
            return new NullHtmlString();
        }
    }
}