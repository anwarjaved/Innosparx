using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Ioc
{
    using System.Reflection;

    public interface IBindingDependencyBuilder
    {
        Func<TType> GetInstance<TType>();

        Func<TType> GetInstance<TType>(Type injectedType);

        void Scan(Assembly assembly);

        void Bind(Type serviceType, Type implmentedType, string name, LifetimeType lifetime);
    }
}
