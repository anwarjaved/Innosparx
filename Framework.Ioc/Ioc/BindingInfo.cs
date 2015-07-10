namespace Framework.Ioc
{
    using System;
    using System.Collections.Concurrent;

    internal class BindingInfo : IBindingInfo, IBindingMatcher
    {
        protected static readonly ConcurrentDictionary<Type, Func<object>> BindingCache = new ConcurrentDictionary<Type, Func<object>>();

        private readonly bool hasBindingInitializer;
        private Func<object> creator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingInfo"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="name">The name.</param>
        /// <author>Anwar</author>
        /// <date>11/9/2011</date>
        protected BindingInfo(Type service, string name = null)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            this.Service = service;
            this.Implementation = service;
            this.BindingType = BindingType.Self;
            this.LifetimeManager = new TransientLifetime();
            this.Name = name;
            this.UniqueID = Guid.NewGuid().ToString();

            this.hasBindingInitializer = service != typeof(object) && service.IsAssignableFrom(typeof(IBindingInitializer));
        }

        public string UniqueID { get; private set; }

        /// <summary>
        /// Gets the service type that is controlled by the binding.
        /// </summary>
        public Type Service { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets the implementation type that is controlled by the binding. </summary>
        ///
        /// <value> The service. </value>
        ///-------------------------------------------------------------------------------------------------
        public Type Implementation { get; protected set; }

        public string Name { get; private set; }

        public int? Order { get; set; }

        /// <summary>
        /// Gets or sets the type of binding.
        /// </summary>
        public BindingType BindingType { get; set; }

        /// <summary>
        /// Gets or sets the lifetime manager.
        /// </summary>
        /// <value>
        /// The lifetime manager.
        /// </value>
        public ILifetime LifetimeManager { get; set; }
        
        /// <summary>
        /// Gets or sets the function used to create instance.
        /// </summary>
        /// <value>The factory.</value>
        public Func<object> Instance
        {
            get
            {
                if (this.creator == null)
                {
                    this.creator = this.BuildInitWrapperFunc(BindingCache.GetOrAdd(this.Service, DependencyBuilder.GetInstance));

                    if (!this.Order.HasValue)
                    {
                        this.Order = int.MaxValue;
                    }
                }

                return this.creator;
            }

            internal set
            {
                this.creator = this.BuildInitWrapperFunc(value);
                if (!this.Order.HasValue)
                {
                    this.Order = int.MaxValue;
                }
            }
        }

        /// <summary>
        /// Gets the instance of Dependency object with <see cref="ILifetime"/>.
        /// </summary>
        /// <returns>Instance of Dependency Object from <see cref="ILifetime"/>.</returns>
        public object GetInstance()
        {
            return this.LifetimeManager.GetInstance(this);
        }

        /// <summary>
        /// Dispose the instance if exists.
        /// </summary>
        public void ReleaseInstance()
        {
            this.LifetimeManager.ReleaseInstance(this);
        }

        bool IBindingMatcher.Matches(IBindingRequest request)
        {
            return object.Equals(request.Service, this.Service) && string.Equals(request.Name, this.Name, StringComparison.OrdinalIgnoreCase);
        }

        private Func<object> BuildInitWrapperFunc(Func<object> creatorFunc)
        {
            if (this.hasBindingInitializer)
            {
                return () =>
                {
                    object instance = creatorFunc();

                    ((IBindingInitializer)instance).Initialize();

                    return instance;
                };
            }

            return creatorFunc;
        }
    }
}
