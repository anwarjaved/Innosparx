namespace Framework.Activator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Security;

    
    internal class AssemblyComparer : IComparer<Assembly>
    {
        
        int IComparer<Assembly>.Compare(Assembly assembly1, Assembly assembly2)
        {
            if (assembly1.IsDefined(typeof(OrderAttribute)) && assembly2.IsDefined(typeof(OrderAttribute)))
            {
                return assembly1.GetCustomAttribute<OrderAttribute>().Value.CompareTo(assembly2.GetCustomAttribute<OrderAttribute>().Value);
            }

            if (assembly1.IsDefined(typeof(OrderAttribute)))
            {
                return -1;
            }

            if (assembly2.IsDefined(typeof(OrderAttribute)))
            {
                return 1;
            }

            var assembly1List = assembly1.GetReferencedAssemblies().Where(x => ActivationManager.SkipList.All(skipValue => !x.FullName.StartsWith(skipValue, StringComparison.OrdinalIgnoreCase))).Select(x => x.FullName).ToList();

            var assembly2List = assembly2.GetReferencedAssemblies().Where(x => ActivationManager.SkipList.All(skipValue => !x.FullName.StartsWith(skipValue, StringComparison.OrdinalIgnoreCase))).Select(x => x.FullName).ToList();

            if (assembly1List.Contains(assembly2.FullName))
            {
                return 1;
            }

            if (assembly2List.Contains(assembly1.FullName))
            {
                return -1;
            }

            return 0;
        }
    }
}
