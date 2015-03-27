using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Ioc
{
    using System.Reflection;
    using System.Security;
    using System.Web.Mvc;

    [InjectBind(typeof(IAssemblyBindingBuilder), "MvcBindingBuilder", LifetimeType.Singleton)]
    public class MvcBindingBuilder : IAssemblyBindingBuilder
    {
        private static bool IsController(Type type)
        {
            return ((typeof(IController).IsAssignableFrom(type) && type.IsPublic) && !type.IsAbstract) && !type.IsInterface;
        }

        [SecuritySafeCritical]
        public void Build(IBindingDependencyBuilder dependencyBuilder, IReadOnlyList<Assembly> assemblies)
        {
            BuildInternal(dependencyBuilder, assemblies);
        }

        [SecurityCritical]
        private static void BuildInternal(IBindingDependencyBuilder dependencyBuilder, IReadOnlyList<Assembly> assemblies)
        {
            var definations = FindDependenciesType(assemblies);
            foreach (var defination in definations.OrderBy(x => x.TypePriority))
            {
                if (!Container.Contains(defination.Type))
                {
                    var func = dependencyBuilder.GetInstance<IController>(defination.Type);
                    Container.Bind(defination.Type).InTransientScope().ToMethod(func);
                }
            }
        }

        private static IEnumerable<MvcControllerDefination> FindDependenciesType(IEnumerable<Assembly> assemblies)
        {
            List<MvcControllerDefination> list = new List<MvcControllerDefination>();
            foreach (Assembly assembly in assemblies)
            {
                foreach (var type in assembly.GetExportableLoadableTypes())
                {
                    if (IsController(type))
                    {
                        MvcControllerDefination defination = new MvcControllerDefination(type);

                        OrderAttribute priorityAttribute = type.GetCustomAttribute<OrderAttribute>();
                        defination.TypePriority = priorityAttribute == null ? int.MaxValue : priorityAttribute.Value;

                        list.Add(defination);
                    }
                }
            }

            return list;
        }


    }
}
