namespace Framework.Tasks
{
    using System.Security;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using Framework.Ioc;
    using Framework.Serialization.Json.Converters;
    using Framework.Web.Api;

    using Newtonsoft.Json;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Web API configuration task.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    [InjectBind(typeof(IBootstrapTask), "WebApiConfigTask")]
    [Order(1)]
    public class WebApiConfigTask : IBootstrapTask
    {

        
        public void Execute()
        {
            if (HostingEnvironment.IsHosted)
            {
                GlobalConfiguration.Configure(this.Configure);
                ApiDependencyResolver.Register();
            }
        }

        
        private void Configure(HttpConfiguration configuration)
        {
            ConfigureRoutes(configuration);
            ConfigureFilters(configuration);
            configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            configuration.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new GuidConverter());
            configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            configuration.Formatters.Insert(0, new DataTableFormatter());

            configuration.EnableCors(new EnableCorsAttribute("*", "*", "*", "*") { SupportsCredentials = true });
        }

        
        private static void ConfigureFilters(HttpConfiguration configuration)
        {
            configuration.Filters.Add(new ApiExceptionFilter());
        }

        
        private static void ConfigureRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "{controller}/{id}", new { id = RouteParameter.Optional });
        }
    }
}




