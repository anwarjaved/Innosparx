﻿namespace LinkedInSDK.Controllers
{
    using System;
    using System.Security;
    using System.Web.Mvc;

    using Framework;
    using Framework.Configuration;
    using Framework.Rest.OAuth;
    using Framework.Services;

    using LinkedInSDK;

    [SecurityCritical]
    [RouteArea("social")]
    [RoutePrefix("linkedin")]
    public class LinkedInController : Controller
    {
        private readonly IWebContext context;

        private readonly IOAuthStateManager stateManager;

        public LinkedInController(IWebContext context, IOAuthStateManager stateManager)
        {
            this.context = context;
            this.stateManager = stateManager;
        }

        [Route("authenticate")]
        [HttpGet]
        public ActionResult Authenticate(string success, string failure, string permissions, string state)
        {
            string key = Guid.NewGuid().ToStringValue();

            OAuthState authState = new OAuthState();
            authState.FailureUrl = failure;
            authState.SuccessUrl = success;
            authState.State = state;

            this.stateManager.SaveState(key, authState);


            LinkedInClient client = new LinkedInClient(this.context.Config.Social.LinkedIn.AppID,
                                                    this.context.Config.Social.LinkedIn.AppSecret);

            var authorizationUrl = client.BuildAuthorizationUrl(SocialApiSetting.BuildUrl(this.context.Config.Social.LinkedIn.Domain, "social/linkedin/authorize"), key, permissions);
            return new RedirectResult(authorizationUrl);
        }

        [Route("authorize")]
        [HttpGet]
        public ActionResult Authorize(string state, string code, string error, string error_description)
        {
            OAuthState authState = this.stateManager.GetState(state);

            if (authState == null)
            {
                throw new InvalidOperationException("Invalid Authorization State");
            }

            UrlBuilder errorUrlBuilder = new UrlBuilder(authState.FailureUrl);
            if (!string.IsNullOrWhiteSpace(code))
            {
                LinkedInClient client = new LinkedInClient(this.context.Config.Social.LinkedIn.AppID,
                                                    this.context.Config.Social.LinkedIn.AppSecret);

                OAuth2TokenCredential credential = client.GetAccessToken(code, SocialApiSetting.BuildUrl(this.context.Config.Social.LinkedIn.Domain, "social/linkedin/authorize"));

                if (credential != null && credential.Success)
                {
                    UrlBuilder redirectBuilder = new UrlBuilder(authState.SuccessUrl);
                    redirectBuilder.QueryString.Add("token", credential.Token);
                    redirectBuilder.QueryString.Add("expiresIn", credential.ExpiresIn.ToStringValue());

                    if (!string.IsNullOrWhiteSpace(authState.State))
                    {
                        redirectBuilder.QueryString.Add("state", authState.State);
                    }

                    return new RedirectResult(redirectBuilder.ToString());
                }

                if (credential != null && !string.IsNullOrWhiteSpace(credential.ErrorCode))
                {
                    errorUrlBuilder.QueryString.Add("code", credential.ErrorCode);
                    errorUrlBuilder.QueryString.Add("message", credential.ErrorMessage);

                }

                return new RedirectResult(errorUrlBuilder.ToString());

            }

            errorUrlBuilder.QueryString.Add("code", error);
            errorUrlBuilder.QueryString.Add("message", error_description);

            if (error == "access_denied" && error_description == "the user denied your request")
            {
                errorUrlBuilder.QueryString.Add("denied", "true");
            }

            return new RedirectResult(errorUrlBuilder.ToString());
        }
    }
}