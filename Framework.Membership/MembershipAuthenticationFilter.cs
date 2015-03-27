using System;

namespace Framework.Membership
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IdentityModel.Services;
    using System.IdentityModel.Services.Tokens;
    using System.IdentityModel.Tokens;
    using System.Security;
    using System.Web;
    using System.Web.Security;

    using Framework.Configuration;
    using Framework.Ioc;
    using Framework.Web;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Membership authentication filter.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/12/2013 1:30 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    [InjectBind(typeof(IHttpFilter), "MembershipAuthenticationFilter", LifetimeType.Singleton)]
    [SecurityCritical]
    public class MembershipAuthenticationFilter : BaseHttpFilter
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes this IHttpFilter.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/12/2013 1:31 PM.
        /// </remarks>
        ///
        /// <param name="application">
        ///     The application.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public override void Initialize(IHttpApplication application)
        {
            if (!HostingEnvironment.IsSharedHost)
            {
                this.ConfigureSession();
                ConfigureDefaultSessionDuration(FormsAuthentication.Timeout);
                ConfigureMackineKeyProtectionForSessionTokens();
                if (FormsAuthentication.SlidingExpiration)
                {
                    EnableSlidingSessionExpirations();
                }

                SuppressLoginRedirectsForApiCalls();
            }
        }

        public static void SuppressAuthenticationRedirect(HttpContext context)
        {
            context.Items[MembershipConstants.SuppressAuthenticationKey] = true;
        }

        public static void SuppressAuthenticationRedirect(HttpContextBase context)
        {
            context.Items[MembershipConstants.SuppressAuthenticationKey] = true;
        }

        [SecurityCritical]
        public override void OnEndRequest(IHttpApplication application)
        {
            var context = application.Context;
            if (context.Response.StatusCode == 401)
            {
                var config = ConfigManager.GetConfig(true);
                if (config != null && config.Application.EnableFormRedirection && !context.Items.Contains(MembershipConstants.SuppressAuthenticationKey))
                {
                    var loginUrl = FormsAuthentication.LoginUrl + "?returnUrl=" + HttpUtility.UrlEncode(context.Request.RawUrl, context.Request.ContentEncoding);
                    context.Response.Redirect(loginUrl);
                }
            }
        }

        private void ConfigureSession()
        {
            SessionAuthenticationModule sessionAuthenticationModule = FederatedAuthentication.SessionAuthenticationModule;
            if (sessionAuthenticationModule != null)
            {
                sessionAuthenticationModule.CookieHandler.Name = FormsAuthentication.FormsCookieName;
                sessionAuthenticationModule.CookieHandler.Domain = FormsAuthentication.CookieDomain;
                sessionAuthenticationModule.CookieHandler.RequireSsl = FormsAuthentication.RequireSSL;
                sessionAuthenticationModule.CookieHandler.Path = FormsAuthentication.FormsCookiePath;
            }
        }

        public static void SetCookie(string cookieName, string domain)
        {
            SessionAuthenticationModule sessionAuthenticationModule = FederatedAuthentication.SessionAuthenticationModule;
            if (sessionAuthenticationModule != null)
            {
                sessionAuthenticationModule.CookieHandler.Name = cookieName;
                sessionAuthenticationModule.CookieHandler.Domain = domain;
            }
        }

        private static bool IsAjaxRequest(HttpRequestBase request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            return ((request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest")));
        }

        private static void ConfigureDefaultSessionDuration(TimeSpan sessionDuration)
        {
            if (!FederatedAuthentication.FederationConfiguration.WsFederationConfiguration.PersistentCookiesOnPassiveRedirects)
            {
                var handler = (SessionSecurityTokenHandler)FederatedAuthentication.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers[typeof(SessionSecurityToken)];
                if (handler != null)
                {
                    handler.TokenLifetime = sessionDuration;
                }
            }
        }

        [SecurityCritical]
        public override void OnPostMapRequest(IHttpApplication application)
        {
            var ctx = application.Context;
            if (IsAjaxRequest(ctx.Request))
            {
                ctx.Response.SuppressFormsAuthenticationRedirect = true;
            }
        }

        private static void SuppressLoginRedirectsForApiCalls()
        {
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;
            if (fam != null)
            {
                fam.AuthorizationFailed +=
                    delegate(object sender, AuthorizationFailedEventArgs e)
                    {
                        var ctx = HttpContext.Current;
                        if (!ctx.User.Identity.IsAuthenticated)
                        {
                            e.RedirectToIdentityProvider = !ctx.Response.SuppressFormsAuthenticationRedirect;
                        }
                    };
            }
        }

        private static void EnableSlidingSessionExpirations()
        {
            if (FederatedAuthentication.SessionAuthenticationModule != null)
            {
                FederatedAuthentication.SessionAuthenticationModule.SessionSecurityTokenReceived += delegate(object sender, SessionSecurityTokenReceivedEventArgs e)
                {
                    var sam = (SessionAuthenticationModule)sender;
                    var token = e.SessionToken;

                    var duration = token.ValidTo.Subtract(token.ValidFrom);
                    if (duration > TimeSpan.Zero)
                    {
                        var diff = token.ValidTo.Add(sam.FederationConfiguration.IdentityConfiguration.MaxClockSkew).Subtract(DateTime.UtcNow);
                        if (diff > TimeSpan.Zero)
                        {
                            var halfWay = duration.Add(sam.FederationConfiguration.IdentityConfiguration.MaxClockSkew).TotalMinutes / 2;
                            var timeLeft = diff.TotalMinutes;
                            if (timeLeft <= halfWay)
                            {
                                //set duration not from original token, but from current app configuration
                                var handler = sam.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers[typeof(SessionSecurityToken)] as SessionSecurityTokenHandler;
                                if (handler != null)
                                {
                                    duration = handler.TokenLifetime;
                                }

                                e.ReissueCookie = true;
                                e.SessionToken = new SessionSecurityToken(token.ClaimsPrincipal, token.Context, DateTime.UtcNow, DateTime.UtcNow.Add(duration))
                                                 {
                                                     IsPersistent =
                                                         token
                                                         .IsPersistent,
                                                     IsReferenceMode
                                                         =
                                                         token
                                                         .IsReferenceMode
                                                 };
                            }
                        }
                    }
                };
            }
        }

        private static void ConfigureMackineKeyProtectionForSessionTokens()
        {
            var handler = (SessionSecurityTokenHandler)FederatedAuthentication.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers[typeof(SessionSecurityToken)];
            if (!(handler is MachineKeySessionSecurityTokenHandler))
            {
                var mkssth = new MachineKeySessionSecurityTokenHandler();
                if (handler != null) mkssth.TokenLifetime = handler.TokenLifetime;
                FederatedAuthentication.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers.AddOrReplace(mkssth);
            }
        }
    }
}