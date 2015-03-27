namespace Framework
{
    using System;
    using System.Text;
    using System.Web;

    public static class HttpContextExtensions
    {
        public static string GetApplicationUrl(this HttpContext ctx)
        {
            if (ctx == null) throw new ArgumentNullException("ctx");

            return new HttpContextWrapper(ctx).GetApplicationUrl();
        }

        public static string GetApplicationUrl(this HttpContextBase ctx)
        {
            if (ctx == null) throw new ArgumentNullException("ctx");

            return ctx.Request.GetApplicationUrl();
        }
        
        public static string GetApplicationUrl(this HttpRequestBase request)
        {
            if (request == null) throw new ArgumentNullException("request");
            StringBuilder sb = new StringBuilder();
            sb.Append(request.Url.Scheme + "://" + request.Url.Host);

            if ((request.Url.Scheme == "http" && request.Url.Port != 80)
                || (request.Url.Scheme == "https" && request.Url.Port != 443))
            {
                sb.Append(request.Url.Port);
            }

            sb.Append(request.ApplicationPath);

            var baseUrl = sb.ToString();

            if (!baseUrl.EndsWith("/")) baseUrl += "/";

            return baseUrl;
        }
    }
}
