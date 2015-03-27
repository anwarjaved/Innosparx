namespace Framework.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;

    using Framework.Logging;
    using Framework.Logging.Impl;

    using Container = Framework.Ioc.Container;

    /// <summary>
    /// Class SystemLogInitializer.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SystemLogInitializer
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [SecurityCritical]
        public static void Init()
        {
            Container.OverrideDefaultService<ILogAdapter>("DbLog");  
        }
    }
}
