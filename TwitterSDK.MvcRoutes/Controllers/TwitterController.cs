namespace TwitterSDK.Controllers
{
    using System;
    using System.Security;
    using System.Web.Mvc;

    using Framework;
    using Framework.Configuration;
    using Framework.Infrastructure;
    using Framework.Ioc;
    using Framework.Rest.OAuth;
    using Framework.Services;

    using TwitterSDK;

    
    [RouteArea("social")]
    [RoutePrefix("twitter")]
    public class TwitterController : Controller
    {
        private readonly IWebContext context;

        private readonly IOAuthStateManager stateManager;

        public TwitterController(IWebContext context, IOAuthStateManager stateManager)
        {
            this.context = context;
            this.stateManager = stateManager;
        }

        [Route("authenticate")]
        [HttpGet]
        public ActionResult Authenticate(string success, string failure, string state)
        {
            string key = Guid.NewGuid().ToStringValue();

            OAuthState authState = new OAuthState();
            authState.FailureUrl = failure;
            authState.SuccessUrl = success;
            authState.State = state;

            this.stateManager.SaveState(key, authState);
            UrlBuilder urlBuilder = new UrlBuilder(SocialApiSetting.BuildUrl(this.context.Config.Social.Twitter.Domain, "social/twitter/authorize"));
            urlBuilder.QueryString.Add("state", key);

            TwitterClient client = new TwitterClient(this.context.Config.Social.Twitter.AppKey,
                                                    this.context.Config.Social.Twitter.AppSecret);

            var tempCredential = client.GetRequestToken(urlBuilder.ToString());

            if (tempCredential == null || !tempCredential.OAuthCallbackConfirmed)
            {
                return new RedirectResult(failure);
            }

            var authorizationUrl = client.BuildAuthorizationUrl(tempCredential);
            ITokenManager tokenManager = Container.Get<ITokenManager>();
            tokenManager.SaveRequestToken(key, tempCredential);
            return new RedirectResult(authorizationUrl);
        }

        [Route("authorize")]
        [HttpGet]
        public ActionResult Authorize(string state, string oauth_verifier, string denied)
        {
            OAuthState authState = this.stateManager.GetState(state);

            UrlBuilder errorUrlBuilder = new UrlBuilder(authState.FailureUrl);

            if (authState == null)
            {
                throw new InvalidOperationException("Invalid Authorization State");
            }

            if (!string.IsNullOrWhiteSpace(oauth_verifier))
            {
                TwitterClient client = new TwitterClient(this.context.Config.Social.Twitter.AppKey,
                                        this.context.Config.Social.Twitter.AppSecret);
                ITokenManager tokenManager = Container.Get<ITokenManager>();
                var token = tokenManager.GetRequestToken(state);

                if (token != null)
                {
                    OAuth1TokenCredential credential = client.GetAccessToken(token, oauth_verifier);

                    if (credential != null && credential.Success)
                    {
                        UrlBuilder redirectBuilder = new UrlBuilder(authState.SuccessUrl);
                        redirectBuilder.QueryString.Add("token", credential.Token);
                        redirectBuilder.QueryString.Add("secret", credential.Secret);

                        if (!string.IsNullOrWhiteSpace(authState.State))
                        {
                            redirectBuilder.QueryString.Add("state", authState.State);
                        }

                        return new RedirectResult(redirectBuilder.ToString());
                    }
                }
            }


            if (!string.IsNullOrWhiteSpace(denied))
            {
                errorUrlBuilder.QueryString.Add("denied", "true");
            }

            return new RedirectResult(errorUrlBuilder.ToString());
        }
    }
}
