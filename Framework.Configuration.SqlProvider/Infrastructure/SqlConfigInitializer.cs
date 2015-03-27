namespace Framework.Infrastructure
{
    using System.ComponentModel;
    using System.Security;

    using Framework.Configuration;

    using Container = Framework.Ioc.Container;

    /// <summary>
    /// Class SqlMessageQueueInitializer.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SqlConfigInitializer
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [SecurityCritical]
        public static void Init()
        {
            Container.OverrideDefaultService<IConfigProvider>("SqlConfig");
        }
    }
}
