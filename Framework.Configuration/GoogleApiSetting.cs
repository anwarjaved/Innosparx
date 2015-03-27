namespace Framework.Configuration
{
    using System;
    using System.Web;

    public class GoogleApiSetting
    {
        public string AppID { get; set; }

        public string AppSecret { get; set; }

        public string Domain { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string RefreshToken { get; set; }

        public string Token { get; set; }

        public IHtmlString GetAuthenticationUrl(string successUrl, string failureUrl, params string[] permissions)
        {
            return GetAuthenticationUrl(successUrl, failureUrl, null, permissions);
        }

        public IHtmlString GetAuthenticationUrl(
            string successUrl,
            string failureUrl,
            string state = null,
            params string[] permissions)
        {
            return this.GetAuthenticationUrl(successUrl, failureUrl, false, null, permissions);
        }

        public IHtmlString GetAuthenticationUrl(
            string successUrl,
            string failureUrl,
            bool offline = false,
            string state = null,
            params string[] permissions)
        {
            var builder = new UrlBuilder(SocialApiSetting.BuildUrl(this.Domain, "social/google/authenticate"));
            builder.QueryString.Add("success", successUrl);
            builder.QueryString.Add("failure", failureUrl);
            builder.QueryString.Add("offline", offline.ToStringValue());
            builder.QueryString.Add("permissions", permissions.ToConcatenatedString(x => x));

            if (!string.IsNullOrWhiteSpace(state))
            {
                builder.QueryString.Add("state", state);
            }

            return new HtmlString(builder.ToString(false));
        }
    }
}