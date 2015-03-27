namespace Framework.Localization
{
    using System;
    using System.Globalization;
    using System.Security;
    using System.Threading;
    using System.Web;

    using Framework.Configuration;
    using Framework.Ioc;
    using Framework.Web;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Language HTTP filter.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/12/2013 1:02 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    [InjectBind(typeof(IHttpFilter), "LanguageHttpFilter", LifetimeType.Singleton)]
    [SecurityCritical]
    public class LanguageHttpFilter : BaseHttpFilter
    {
        [SecurityCritical]
        public override void OnBeginRequest(IHttpApplication application)
        {
            if (ConfigManager.Application.LocalizationEnabled)
            {
                HttpContextBase context = application.Context;

                HttpCookie cookie = context.Request.Cookies[LocalizationConstants.CookieName]
                                    ?? new HttpCookie(LocalizationConstants.CookieName, LocalizationManager.GetLanguageLCID(LanguageCode.English).ToString(CultureInfo.InvariantCulture));
                cookie.Expires = DateTime.UtcNow.AddDays(7);

                context.Response.Cookies.Add(cookie);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Convert.ToInt32(cookie.Value));
            }
        }
    }
}
