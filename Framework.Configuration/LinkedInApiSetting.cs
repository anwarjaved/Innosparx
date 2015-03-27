namespace Framework.Configuration
{
    using System.Web;

    public class LinkedInApiSetting
    {
        public string AppID { get; set; }

        public string AppSecret { get; set; }

        public string Domain { get; set; }

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
            var builder = new UrlBuilder(SocialApiSetting.BuildUrl(this.Domain, "social/linkedin/authenticate"));
            builder.QueryString.Add("success", successUrl);
            builder.QueryString.Add("failure", failureUrl);
            builder.QueryString.Add("permissions", permissions.ToConcatenatedString(x => x, " "));

            if (!string.IsNullOrWhiteSpace(state))
            {
                builder.QueryString.Add("state", state);
            }

            return new HtmlString(builder.ToString(false));
        }
    }
}