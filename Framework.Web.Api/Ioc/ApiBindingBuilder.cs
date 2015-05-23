using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Ioc
{
    using System.Reflection;
    using System.Security;
    using System.Web.Http.Controllers;

    [InjectBind(typeof(IAssemblyBindingBuilder), "ApiBindingBuilder", LifetimeType.Singleton)]
    public class ApiBindingBuilder : IAssemblyBindingBuilder
    {
        
        public void Build(IBindingDependencyBuilder dependencyBuilder, IReadOnlyList<Assembly> assemblies)
        {
            BuildInternal(dependencyBuilder, assemblies);
        }

        
        private static void BuildInternal(IBindingDependencyBuilder dependencyBuilder, IReadOnlyList<Assembly> assemblies)
        {
            var definations = FindDependenciesType(assemblies);
            foreach (var defination in definations.OrderBy(x => x.TypePriority))
            {
                if (!Container.Contains(defination.Type))
                {
                    var func = dependencyBuilder.GetInstance<IHttpController>(defination.Type);
                    Container.Bind(defination.Type).InTransientScope().ToMethod(func);
                }
            }
        }

        private static bool IsWebController(Type type)
        {
            return ((typeof(IHttpController).IsAssignableFrom(type) && type.IsPublic) && !type.IsAbstract) && !type.IsInterface;
        }

        private static IEnumerable<ApiControllerDefination> FindDependenciesType(IReadOnlyList<Assembly> assemblies)
        {
            List<ApiControllerDefination> list = new List<ApiControllerDefination>();
            foreach (Assembly assembly in assemblies)
            {
                foreach (var type in assembly.GetExportableLoadableTypes())
                {
                    if (IsWebController(type))
                    {
                        ApiControllerDefination defination = new ApiControllerDefination(type);

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
