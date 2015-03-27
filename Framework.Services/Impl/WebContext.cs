namespace Framework.Services.Impl
{
    using System;
    using System.Collections;
    using System.Security;
    using System.Security.Principal;
    using System.Text;
    using System.Web;
    using System.Web.Routing;

    using Framework.Caching;
    using Framework.Configuration;
    using Framework.Fakes;
    using Framework.Ioc;
    using Framework.Membership;

    [InjectBind(typeof(IWebContext), LifetimeType.Request)]
    [SecurityCritical]
    public class WebContext : IWebContext
    {
        private const string ContextItemPrefix = "X-ContextItem-";

        private HttpContextBase context;

        public WebContext()
        {
            var currentContext = HttpContext.Current;

            if (currentContext != null)
            {
                this.context = new HttpContextWrapper(currentContext);

            }
            else
            {
                this.context = FakeHttpContext.Root();
            }
        }

        public HttpRequestBase Request
        {
            [SecurityCritical]
            get
            {
                return this.context.Request;
            }
        }

        public HttpServerUtilityBase Server
        {
            [SecurityCritical]
            get
            {
                return this.context.Server;
            }
        }

        [SecurityCritical]
        public IWebContext WithContext(HttpContextBase newContext)
        {
            this.context = newContext;
            return this;
        }

        public HttpResponseBase Response
        {
            [SecurityCritical]
            get
            {
                return this.context.Response;
            }
        }

        public RequestContext RequestContext
        {
            [SecurityCritical]
            get
            {
                return new RequestContext(this.context, new RouteData());
            }
        }

        public HttpSessionStateBase Session
        {
            [SecurityCritical]
            get
            {
                return this.context.Session;
            }
        }

        public IDictionary Items
        {
            [SecurityCritical]
            get
            {
                return this.context.Items;
            }
        }

        public IPrincipal Principal
        {
            [SecurityCritical]
            get
            {
                return this.context.User;
            }
            [SecurityCritical]
            set
            {
                this.context.User = value;
            }
        }

        public IUser User
        {
            [SecurityCritical]
            get
            {
                if (this.IsAuthenticated)
                {
                    return MembershipManager.GetCurrentUser();
                }

                return null;
            }
        }

        public bool IsAuthenticated
        {
            [SecurityCritical]
            get
            {
                if (this.Principal != null && this.Principal.Identity.IsAuthenticated)
                {
                    return true;
                }



                return this.Request.IsAuthenticated;
            }
        }

        public ICache Cache
        {
            [SecurityCritical]
            get
            {
                return Container.Get<ICache>();
            }
        }

        public Config Config
        {
            [SecurityCritical]
            get
            {
                return ConfigManager.GetConfig(true);
            }
        }

        [SecurityCritical]
        public T GetFromContext<T>(string key)
        {
            if (this.Items.Contains(ContextItemPrefix + key))
            {
                return (T)this.Items[ContextItemPrefix + key];
            }

            return default(T);
        }

        [SecurityCritical]
        public void SetInContext<T>(string key, T value)
        {
            if (this.Items.Contains(ContextItemPrefix + key))
            {
                this.Items[ContextItemPrefix + key] = value;
            }
            else
            {
                this.Items.Add(ContextItemPrefix + key, value);
            }
        }

        [SecurityCritical]
        public string BuildUrl(string resource)
        {
            Uri url = this.Request.Url;
            StringBuilder sb = new StringBuilder();
            sb.Append(url.Scheme + "://");
            sb.Append(url.Host);

            return UrlPath.Combine(sb.ToString(), resource);
        }
    }
}
