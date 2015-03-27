using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Ioc
{
    using System.Reflection;

    public interface IAssemblyBindingBuilder
    {
        void Build(IBindingDependencyBuilder dependencyBuilder, IReadOnlyList<Assembly> assemblies);
    }
}
