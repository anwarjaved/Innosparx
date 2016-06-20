using System;
using System.Collections.Generic;

namespace Framework.Ioc
{
    using System.Security;
    using System.Web.Http.Dependencies;

    using Framework.Infrastructure;

    
    internal class ApiDependencyScope : IDependencyScope
    {
        
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

        
        void IDisposable.Dispose()
        {
        }
    }
}
