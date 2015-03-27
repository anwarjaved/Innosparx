using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Caching.SqlCache.Infrastructure
{
    using System.ComponentModel;
    using System.Security;

    using Container = Framework.Ioc.Container;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SqlCacheInitializer
    {
        [SecurityCritical]
        public static void Init()
        {
            Container.OverrideDefaultService<ICache>("SqlCache");
        }
    }
}
