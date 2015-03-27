namespace GoogleSDK.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Security;
    using System.Web.Mvc;


    using Framework;
    using Framework.Collections;
    using Framework.Configuration;
    using Framework.Infrastructure;
    using Framework.Rest;
    using Framework.Rest.OAuth;
    using Framework.Services;

    [SecurityCritical]
    [RouteArea("social")]
    [RoutePrefix("google")]
    public class GoogleController : Controller
    {
        private readonly IWebContext context;

        private readonly IOAuthStateManager stateManager;

        public GoogleController(IWebContext context, IOAuthStateManager stateManager)
        {
            this.context = context;
            this.stateManager = stateManager;
        }

        [Route("authenticate")]
        [HttpGet]
        public ActionResult Authenticate(string success, string failure, string permissions, string state, bool offline)
        {

            string key = Guid.NewGuid().ToStringValue();

            OAuthState authState = new OAuthState();
            authState.FailureUrl = failure;
            authState.SuccessUrl = success;
            authState.State = state;

            this.stateManager.SaveState(key, authState);

            IDictionary<string, string> parameters = new Dictionary<string, string>();
            if (offline)
            {
                parameters.Add("access_type", "offline");
            }

            List<string> permissionList = new List<string>();

            if (!string.IsNullOrWhiteSpace(permissions))
            {
                permissionList.AddRange(permissions.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries));
            }

            GoogleClient client = new GoogleClient(this.context.Config.Social.Google.AppID,
                                                    this.context.Config.Social.Google.AppSecret);

            string authorizationUrl = client.BuildAuthorizationUrl(SocialApiSetting.BuildUrl(this.context.Config.Social.Google.Domain, "social/google/authorize"), permissionList, key, parameters);
            return new RedirectResult(authorizationUrl);
        }

        [Route("authorize")]
        [HttpGet]
        public ActionResult Authorize(string state, string code, string error)
        {
            OAuthState authState = this.stateManager.GetState(state);

            if (authState == null)
            {
                throw new InvalidOperationException("Invalid Authorization State");
            }

            UrlBuilder errorUrlBuilder = new UrlBuilder(authState.FailureUrl);
            if (string.IsNullOrWhiteSpace(error))
            {
                GoogleClient client = new GoogleClient(this.context.Config.Social.Google.AppID, this.context.Config.Social.Google.AppSecret);

                var credential = client.GetAccessToken(code, SocialApiSetting.BuildUrl(this.context.Config.Social.Google.Domain, "social/google/authorize"));
                if (credential != null && credential.Success)
                {
                    UrlBuilder redirectBuilder = new UrlBuilder(authState.SuccessUrl);
                    redirectBuilder.QueryString.Add("token", credential.Token);
                    redirectBuilder.QueryString.Add("refreshToken", credential.RefreshToken);
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
            }
            errorUrlBuilder.QueryString.Add("code", error);


            return new RedirectResult(errorUrlBuilder.ToString());
        }
    }
}
