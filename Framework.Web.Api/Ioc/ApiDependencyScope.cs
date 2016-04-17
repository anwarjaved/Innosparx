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
            var service = Container.TryGet(serviceType);
            return service;
        }


        public IEnumerable<object> GetServices(Type serviceType)
        {
            IReadOnlyList<object> services = Container.TryGetAll(serviceType);
            return services;
        }

        
        void IDisposable.Dispose()
        {
        }
    }
}
