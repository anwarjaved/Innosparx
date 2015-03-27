namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Framework.Activator;

    internal static class IocAssemblyExtensions
    {
        internal static IEnumerable<T> GetActivationAttributes<T>(this Assembly assembly) where T : BaseActivationMethodAttribute
        {
            return assembly.GetAttributes<T>();
        }
    }
}
