namespace Framework.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;

    using Framework.IDMembership;

    using Container = Framework.Ioc.Container;

    /// <summary>
    /// Class SqlMembershipInitializer.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SqlMembershipInitializer
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        
        public static void Init()
        {
            Container.OverrideDefaultService<IMembershipProvider>("SqlMembership");
        }
    }
}
