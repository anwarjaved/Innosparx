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
            
            get
            {
                return this.context.Request;
            }
        }

        public HttpServerUtilityBase Server
        {
            
            get
            {
                return this.context.Server;
            }
        }

        
        public IWebContext WithContext(HttpContextBase newContext)
        {
            this.context = newContext;
            return this;
        }

        public HttpResponseBase Response
        {
            
            get
            {
                return this.context.Response;
            }
        }

        public RequestContext RequestContext
        {
            
            get
            {
                return new RequestContext(this.context, new RouteData());
            }
        }

        public HttpSessionStateBase Session
        {
            
            get
            {
                return this.context.Session;
            }
        }

        public IDictionary Items
        {
            
            get
            {
                return this.context.Items;
            }
        }

        public IPrincipal Principal
        {
            
            get
            {
                return this.context.User;
            }
            
            set
            {
                this.context.User = value;
            }
        }

        public IUser User
        {
            
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
            
            get
            {
                return Container.Get<ICache>();
            }
        }

        public Config Config
        {
            
            get
            {
                return ConfigManager.GetConfig(true);
            }
        }

        
        public T GetFromContext<T>(string key)
        {
            if (this.Items.Contains(ContextItemPrefix + key))
            {
                return (T)this.Items[ContextItemPrefix + key];
            }

            return default(T);
        }

        
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
