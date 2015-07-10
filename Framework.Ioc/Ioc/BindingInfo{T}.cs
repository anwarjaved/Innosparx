namespace Framework.Ioc
{
    using System;

    internal class BindingInfo<T> : BindingInfo, IBindingInfo<T>
    {
        public BindingInfo(string name = null)
            : base(typeof(T), name)
        {
        }

        public BindingInfo(Type service, string name = null)
            : base(service, name)
        {
        }

        /// <summary>
        /// Indicates that the service should be bound to the specified implementation type.
        /// </summary>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        /// <returns>Binding Info as <see cref="BindingInfo{T}"/>.</returns>
        public IBindingInfo<T> To<TImplementation>() where TImplementation : T
        {
            Type service = typeof(TImplementation);

            this.Instance = BindingCache.GetOrAdd(service, DependencyBuilder.GetInstance);
            this.BindingType = BindingType.Type;
            this.Implementation = typeof(TImplementation);

            return this;
        }

        public IBindingInfo<T> ToSelf()
        {
            Type service = typeof(T);
            this.Instance = BindingCache.GetOrAdd(service, DependencyBuilder.GetInstance);
            this.BindingType = BindingType.Self;
            this.Implementation = typeof(T);

            return this;
        }

        /// <summary>
        /// Indicates that the service should be bound to the specified method.
        /// </summary>
        /// <param name="method">The method to bound.</param>
        /// <returns><see cref="IBindingInfo{T}"/> object.</returns>
        /// <author>Anwar</author>
        /// <date>11/10/2011</date>
        public IBindingInfo<T> ToMethod(Func<T> method)
        {
            this.Instance = () => method();
            this.BindingType = BindingType.Method;
            this.Implementation = null;

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Indicates that instances activated via the binding should not be re-used.
        /// </summary>
        /// <returns>
        /// Binding Info as<see cref="BindingInfo{T}"/>.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public IBindingInfo<T> InTransientScope()
        {
            this.LifetimeManager = new TransientLifetime();
            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Indicates that only a single instance of the binding should be created, and then should
        /// be re-used for all subsequent requests.
        /// </summary>
        /// <returns>
        /// Binding Info as<see cref="BindingInfo{T}"/>.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public IBindingInfo<T> InSingletonScope()
        {
            this.LifetimeManager = new SingletonLifetime();
            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Indicates that instances activated via the binding should be re-used within the same
        /// thread.
        /// </summary>
        /// <returns>
        /// Binding Info as<see cref="BindingInfo{T}"/>.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public IBindingInfo<T> InThreadScope()
        {
            this.LifetimeManager = new ThreadLifetime();
            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Indicates that instances activated via the binding should be re-used within the same HTTP
        /// request.
        /// </summary>
        /// <returns>
        /// Binding Info as<see cref="BindingInfo{T}"/>.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public IBindingInfo<T> InRequestScope()
        {
            this.LifetimeManager = new RequestLifetime();
            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Indicates that instances activated via the binding should be re-used within the same HTTP
        /// Session.
        /// </summary>
        /// <returns>
        /// Binding Info as<see cref="BindingInfo{T}"/>.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public IBindingInfo<T> InSessionScope()
        {
            this.LifetimeManager = new SessionLifetime();
            return this;
        }
    }
}
