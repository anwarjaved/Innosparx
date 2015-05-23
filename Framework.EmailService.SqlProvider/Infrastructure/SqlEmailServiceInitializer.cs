namespace Framework.Services.Infrastructure
{
    using System.ComponentModel;
    using System.Security;

    using Container = Framework.Ioc.Container;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class SqlEmailServiceInitializer
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        
        public static void Init()
        {
            Container.OverrideDefaultService<IEmailProvider>("SqlEmailProvider");
        }
    }
}
