using System;
using System.Collections.Generic;

namespace Framework.Ioc
{
    using System.Security;
    using System.Web.Http.Dependencies;

    using Framework.Infrastructure;
    using Framework.Logging;

    [SecurityCritical]
    internal class ApiDependencyScope : IDependencyScope
    {
        [SecurityCritical]
        public object GetService(Type serviceType)
        {
            using (var benchmark = Benchmark.Start())
            {
                var service = Container.TryGet(serviceType);

                benchmark.Stop();
                if (service == null)
                {
                    Logger.Warn(Logger.Completed(benchmark.TotalTime, false, serviceType.Name), ApiConstants.IoCComponent);
                }
                else
                {
                    Logger.Info(Logger.Completed(benchmark.TotalTime, true, serviceType.Name), ApiConstants.IoCComponent);
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
                    Logger.Warn(Logger.Completed(benchmark.TotalTime, false, serviceType.Name), ApiConstants.IoCComponent);
                }
                else
                {
                    Logger.Info(Logger.Completed(benchmark.TotalTime, true, serviceType.Name), ApiConstants.IoCComponent);
                }

                return services;
            }
        }

        [SecuritySafeCritical]
        void IDisposable.Dispose()
        {
        }
    }
}
