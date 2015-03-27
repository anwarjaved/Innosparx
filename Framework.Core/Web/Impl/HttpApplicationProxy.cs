namespace Framework.Web.Impl
{
    using System.Web;

    public class HttpApplicationProxy : IHttpApplication
    {
        private readonly HttpApplication application;

        public HttpApplicationProxy(HttpApplication application)
        {
            this.application = application;
        }

        public HttpContextBase Context
        {
            get
            {
                return new HttpContextWrapper(this.application.Context);
            }
        }

        public HttpServerUtilityBase Server
        {
            get
            {
                return new HttpServerUtilityWrapper(this.application.Server);
            }
        }
    }
}
