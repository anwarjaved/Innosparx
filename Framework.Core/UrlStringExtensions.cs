namespace Framework
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;

    /// <summary>
  /// Provides helper extensions for turning strings into fully-qualified and SSL-enabled Urls.
  /// </summary>
  public static class UrlStringExtensions
  {
    /// <summary>
    /// Convert Url to Fully Qualified Link.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>Return Url as Fully Qualified Link.</returns>
    public static string ToFullyQualifiedLink(this string text)
    {
      Regex regex = new Regex(
        "(?<Before><a.*href=\")(?!http)(?<Url>.*?)(?<After>\".+>)", RegexOptions.Multiline | RegexOptions.IgnoreCase);

        return regex.Replace(text, m => m.Groups["Before"].Value + ToFullyQualifiedUrl(m.Groups["Url"].Value) + m.Groups["After"].Value);
    }

    /// <summary>
    /// Convert Url to Fully Qualified URL.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>Return Url as Fully Qualified Url.</returns>
    public static string ToFullyQualifiedUrl(this string text)
    {
      // ### the VirtualPathUtility doesn"t handle querystrings, so we break the original url up
      string oldUrl = text;

      string[] oldUrlArray = oldUrl.Contains("?") ? oldUrl.Split('?') : new[] { oldUrl, string.Empty };

      // ### we"ll use the Request.Url object to recreate the current server request"s base url
      // ### requestUri.AbsoluteUri = "http://domain.com:1234/Home/Index"
      // ### requestUri.LocalPath = "/Home/Index"
      // ### subtract the two and you get "http://domain.com:1234", which is urlBase
      Uri requestUri = HttpContext.Current.Request.Url;

      // ### fix for Mike Hadlow's reported issue regarding extraneous link elements when a querystring is present
      // var urlBase = requestUri.AbsoluteUri.Substring( 0, requestUri.AbsoluteUri.Length - requestUri.LocalPath.Length );
      string localPathAndQuery = requestUri.LocalPath + requestUri.Query;
      string urlBase = requestUri.AbsoluteUri.Substring(0, requestUri.AbsoluteUri.Length - localPathAndQuery.Length);

      while (urlBase.EndsWith("/"))
      {
        urlBase = urlBase.Substring(0, urlBase.Length - 1);
      }

      // ### convert the request url into an absolute path, then reappend the querystring, if one was specified
      string newUrl = VirtualPathUtility.ToAbsolute(oldUrlArray[0]);

      while (newUrl.EndsWith("/"))
      {
        newUrl = newUrl.Substring(0, newUrl.Length - 1);
      }

      if (!string.IsNullOrEmpty(oldUrlArray[1]))
      {
        newUrl += "?" + oldUrlArray[1];
      }

      // ### combine the old url base (protocol + server + port) with the new local path
      return urlBase + newUrl;
    }

    /// <summary>
    /// Convert Url to the SSL Link.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>Return Url as SSl Link.</returns>
    public static string ToSslLink(this string text)
    {
      return ToFullyQualifiedLink(text).Replace("http:", "https:");
    }

    /// <summary>
    /// Convert Url to the SSL URL.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>Return Url as SSl Url.</returns>
    public static string ToSslUrl(this string text)
    {
      return ToFullyQualifiedUrl(text).Replace("http:", "https:");
    }
  }
}