using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Ioc
{
    using System.Reflection;
    using System.Security;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.UI;

    using Framework.Activator;

    
    internal class MvcDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        private MvcDependencyResolver()
        {
        }

        
        public object GetService(Type serviceType)
        {
            using (var benchmark = Benchmark.Start())
            {
                var service = Container.TryGet(serviceType);

                benchmark.Stop();

                return service;
            }
        }

        
        public IEnumerable<object> GetServices(Type serviceType)
        {
            using (var benchmark = Benchmark.Start())
            {
                IReadOnlyList<object> services = Container.TryGetAll(serviceType);

                benchmark.Stop();

                return services;
            }
        }

        /// <summary>
        /// Registers all controllers in specified assembly.
        /// </summary>
        
        public static void Register()
        {
            System.Web.Mvc.DependencyResolver.SetResolver(new MvcDependencyResolver());
        }

    }
}
