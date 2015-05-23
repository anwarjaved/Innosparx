using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Ioc
{
    using System.Reflection;
    using System.Security;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dependencies;
    using System.Web.Routing;
    using System.Web.UI;

    using Framework.Activator;

    
    internal class ApiDependencyResolver : ApiDependencyScope, System.Web.Http.Dependencies.IDependencyResolver
    {
        private ApiDependencyResolver()
        {
        }

        /// <summary>
        /// Registers all controllers in specified assembly.
        /// </summary>
        
        public static void Register()
        {
            GlobalConfiguration.Configure(SetApiResolver);
        }

        
        private static void SetApiResolver(HttpConfiguration config)
        {
            config.DependencyResolver = new ApiDependencyResolver();
        }

        
        public IDependencyScope BeginScope()
        {
            return new ApiDependencyScope();
        }

    }
}
