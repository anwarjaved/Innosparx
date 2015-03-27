namespace Framework.Tasks
{
    using System.Linq;
    using System.Security;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Framework.Configuration;
    using Framework.Ioc;
    using Framework.Web.Mvc;

    [InjectBind(typeof(IBootstrapTask), "MvcConfigTask")]
    [Order(0)]
    public class MvcConfigTask : IBootstrapTask
    {
        [SecurityCritical]
        public void Execute()
        {
            if (HostingEnvironment.IsHosted)
            {
                MvcDependencyResolver.Register();
                AreaRegistration.RegisterAllAreas();
                ConfigureBinders();
                Config config = ConfigManager.GetConfig(true);
                ConfigureFilters(config);
                ConfigureViewEngine();

                ConfigureRoutes();
            }
        }

        [SecurityCritical]
        private static void ConfigureViewEngine()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            ViewEngines.Engines.Add(new MustacheViewEngine());
            ViewEngines.Engines.Add(new WebFormViewEngine());
        }

        [SecurityCritical]
        private static void ConfigureBinders()
        {
            ValueProviderFactories.Factories.Remove(
                ValueProviderFactories.Factories.OfType<JsonValueProviderFactory>().FirstOrDefault());
            ModelBinders.Binders.DefaultBinder = new Framework.Web.Mvc.DefaultModelBinder();
        }

        [SecurityCritical]
        private static void ConfigureFilters(Config config)
        {
            var filters = GlobalFilters.Filters;
            filters.Add(new LogFilter());

            if (config.Application.EnableTurboLinks)
            {
                filters.Add(new TurbolinksAttribute());
            }

            filters.Add(new GlobalExceptionFilter());
            filters.Add(new FrameworkFilter());
        }

        [SecurityCritical]
        private static void ConfigureRoutes()
        {
            var routes = RouteTable.Routes;
            routes.LowercaseUrls = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.Ignore("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapMvcAttributeRoutes();
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { action = "Index", id = UrlParameter.Optional });
        }
    }
}
