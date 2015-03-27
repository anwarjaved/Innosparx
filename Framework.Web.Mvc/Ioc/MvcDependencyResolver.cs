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
    using Framework.Logging;

    [SecurityCritical]
    internal class MvcDependencyResolver : System.Web.Mvc.IDependencyResolver
    {
        private MvcDependencyResolver()
        {
        }

        [SecurityCritical]
        public object GetService(Type serviceType)
        {
            using (var benchmark = Benchmark.Start())
            {
                var service = Container.TryGet(serviceType);

                benchmark.Stop();
                if (service == null)
                {
                    Logger.Warn(Logger.Completed(benchmark.TotalTime, false, serviceType.Name), WebConstants.IoCComponent);
                }
                else
                {
                    Logger.Info(Logger.Completed(benchmark.TotalTime, true, serviceType.Name), WebConstants.IoCComponent);
                }

                return service;
            }
        }

        [SecurityCritical]
        public IEnumerable<object> GetServices(Type serviceType)
        {
            using (var benchmark = Benchmark.Start())
            {
                IReadOnlyList<object> services = Container.TryGetAll(serviceType);

                benchmark.Stop();
                if (services.Count == 0)
                {
                    Logger.Warn(Logger.Completed(benchmark.TotalTime, false, serviceType.Name), WebConstants.IoCComponent);
                }
                else
                {
                    Logger.Info(Logger.Completed(benchmark.TotalTime, true, serviceType.Name), WebConstants.IoCComponent);
                }

                return services;
            }
        }

        /// <summary>
        /// Registers all controllers in specified assembly.
        /// </summary>
        [SecurityCritical]
        public static void Register()
        {
            System.Web.Mvc.DependencyResolver.SetResolver(new MvcDependencyResolver());
        }

    }
}
