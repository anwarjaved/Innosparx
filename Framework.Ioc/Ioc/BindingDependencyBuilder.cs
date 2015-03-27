using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Ioc
{
    using System.Reflection;

    [InjectBind(typeof(IBindingDependencyBuilder), LifetimeType.Singleton)]
    public class BindingDependencyBuilder : IBindingDependencyBuilder
    {

        public Func<TType> GetInstance<TType>()
        {
            return DependencyBuilder.GetInstance<TType>();
        }

        public Func<TType> GetInstance<TType>(Type injectedType)
        {
            return DependencyBuilder.GetInstance<TType>(injectedType);
        }

        public void Scan(Assembly assembly)
        {
            bool postBuildNeeded;
            DependencyBuilder.Scan(assembly, out postBuildNeeded);

            if (postBuildNeeded)
            {
                DependencyBuilder.PostBuildScan(new List<Assembly>() { assembly });
            }
        }

        public void Bind(Type serviceType, Type implmentedType, string name, LifetimeType lifetime)
        {
            DependencyBuilder.BindInstance(serviceType, implmentedType, name, lifetime);
        }
    }
}
