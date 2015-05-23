namespace Framework.Services
{
    using System.Collections;
    using System.Security;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Routing;

    using Framework.Caching;
    using Framework.Configuration;
    using Framework.Membership;

    
    public interface IWebContext
    {
        //HttpContextBase Context { get; }
        HttpRequestBase Request { get; }
        HttpResponseBase Response { get; }
        HttpSessionStateBase Session { get; }

        IDictionary Items { get; }
        IPrincipal Principal { get; set; }

        IUser User { get; }

        bool IsAuthenticated { get; }

        ICache Cache { get; }

        Config Config { get; }
        RequestContext RequestContext { get; }
        HttpServerUtilityBase Server {  get; }

        T GetFromContext<T>(string key);

        void SetInContext<T>(string key, T value);

        IWebContext WithContext(HttpContextBase context);

        string BuildUrl(string resource);
    }
}
