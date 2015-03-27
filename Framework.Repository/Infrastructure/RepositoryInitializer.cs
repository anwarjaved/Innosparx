namespace Framework.Infrastructure
{
    using System.ComponentModel;
    using System.Configuration;
    using System.Security;
    using System.Web.Configuration;

    using Framework.DataAccess;
    using Framework.DataAccess.Impl;

    using Container = Framework.Ioc.Container;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Pre application start code.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class RepositoryInitializer
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initialises this object.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public static void Init()
        {
            EntityContextFactory.Initalize();
            Container.Bind<IUnitOfWork>().ToMethod(
                () =>
                {
                    string nameOrConnectionString = null;
                    var overriddenConnectionString = WebConfigurationManager.AppSettings["Framework.RepositoryContext"];
                    if (!string.IsNullOrWhiteSpace(overriddenConnectionString))
                    {
                        nameOrConnectionString = overriddenConnectionString;
                    }

                    if (string.IsNullOrWhiteSpace(nameOrConnectionString))
                    {
                        nameOrConnectionString = "AppContext";
                    }

                    return new UnitOfWork(nameOrConnectionString);
                }).InRequestScope();
        }
    }
}
