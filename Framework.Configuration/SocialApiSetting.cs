namespace Framework.Configuration
{
    using System;
    using System.Text;
    using System.Web;

    public class SocialApiSetting
    {
        public SocialApiSetting()
        {
            this.Facebook = new FacebookApiSetting();
            this.Twitter = new TwitterApiSetting();
            this.LinkedIn = new LinkedInApiSetting();
            this.Google = new GoogleApiSetting();
            this.Yahoo = new YahooApiSetting();
        }

        public FacebookApiSetting Facebook { get; private set; }

        public GoogleApiSetting Google { get; private set; }

        public LinkedInApiSetting LinkedIn { get; private set; }

        public TwitterApiSetting Twitter { get; private set; }

        public YahooApiSetting Yahoo { get; private set; }

        public static string BuildUrl(string domain, string socialRoute)
        {
            if (string.IsNullOrWhiteSpace(domain))
            {
                HttpContext context = HttpContext.Current;

                if (context != null)
                {
                    Uri url = context.Request.Url;
                    var sb = new StringBuilder();
                    sb.Append(url.Scheme + "://");
                    sb.Append(url.Host);

                    return UrlPath.Combine(sb.ToString(), socialRoute);
                }
            }
            else
            {
                Uri url = new Uri(domain);
                var sb = new StringBuilder();
                sb.Append(url.Scheme + "://");
                sb.Append(url.Host);

                return UrlPath.Combine(sb.ToString(), socialRoute);
            }
            

            return null;
        }
    }
}