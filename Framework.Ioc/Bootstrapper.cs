using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    using System.ComponentModel;
    using System.Reflection;
    using System.Security;

    using Container = Framework.Ioc.Container;

    [SecuritySafeCritical]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class Bootstrapper
    {
        public static void Run()
        {
            var tasks = Container.TryGetAll<IBootstrapTask>().OrderBy(
                                    t =>
                                    {
                                        Type taskType = t.GetType();

                                        OrderAttribute priority = taskType.GetCustomAttribute<OrderAttribute>();

                                        return priority != null ? priority.Value : int.MaxValue;
                                    }).ToList();

            foreach (var task in tasks)
            {
                task.Execute();
            }
        }
    }
}
