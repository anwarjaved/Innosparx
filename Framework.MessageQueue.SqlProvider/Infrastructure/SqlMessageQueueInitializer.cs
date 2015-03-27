namespace Framework.Infrastructure
{
    using System.ComponentModel;
    using System.Security;

    using Framework.MessageQueue;

    using Container = Framework.Ioc.Container;

    /// <summary>
    /// Class SqlMessageQueueInitializer.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SqlMessageQueueInitializer
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [SecurityCritical]
        public static void Init()
        {
            Container.OverrideDefaultService<IQueue>("SqlQueue");
        }
    }
}
