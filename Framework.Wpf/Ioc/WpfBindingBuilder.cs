using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Ioc
{
    using System.Reflection;
    using System.Security;

    using Framework.Wpf.MVVM;

    [InjectBind(typeof(IAssemblyBindingBuilder), "WpfBindingBuilder", LifetimeType.Singleton)]
    public class WpfBindingBuilder : IAssemblyBindingBuilder
    {
        
        public void Build(IBindingDependencyBuilder dependencyBuilder, IReadOnlyList<Assembly> assemblies)
        {
            BuildInternal(dependencyBuilder, assemblies);
        }

        
        private static void BuildInternal(IBindingDependencyBuilder dependencyBuilder, IEnumerable<Assembly> assemblies)
        {
            foreach (var type in from assembly in assemblies from type in assembly.GetExportableLoadableTypes() where IsViewModel(type) where !Container.Contains(type) select type)
            {
                dependencyBuilder.Bind(typeof(ViewModelBase), type, type.Name, LifetimeType.Singleton);
            }
        }

        private static bool IsViewModel(Type type)
        {
            return ((typeof(ViewModelBase).IsAssignableFrom(type) && type.IsPublic) && !type.IsAbstract) && !type.IsInterface;

        }
    }
}
