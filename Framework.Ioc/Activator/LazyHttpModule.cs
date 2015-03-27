namespace Framework.Activator
{
    using System;
    using System.Web;

    using Framework.Ioc;

    /// <summary>
    /// Class LazyHttpModule.
    /// </summary>
    /// <typeparam name="TModule">The type of the t module.</typeparam>
    public class LazyHttpModule<TModule> : IHttpModule where TModule : IHttpModule
    {
        private readonly Lazy<IHttpModule> modules = new Lazy<IHttpModule>(RetrieveModule);

        /// <summary>
        /// Registers the specified module function.
        /// </summary>
        /// <param name="moduleFunc">The module function.</param>
        public static void Register(Func<TModule> moduleFunc)
        {
            Container.Bind<TModule>().ToMethod(moduleFunc);
            var moduleType = typeof(LazyHttpModule<TModule>);
            HttpApplication.RegisterModule(moduleType);
        }
        
        /// <summary>
        /// Initializes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Init(HttpApplication context)
        {
            this.modules.Value.Init(context);
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            this.modules.Value.Dispose();
        }

        private static IHttpModule RetrieveModule()
        {
            return Container.Get<TModule>();
        }
    }
}