namespace Framework.Ioc
{
    using System;

    /// <summary>
    /// This lifetime manager’s behavior is to always return a the same instance when the 
    /// Resolve method is called by executing the factory method.
    /// </summary>
    internal class SingletonLifetime : ILifetime
    {
        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The dependency object instance.</value>
        private object CachedInstance { get; set; }

        public object GetInstance(IBindingInfo dependencyInfo)
        {
            return this.CachedInstance ?? (this.CachedInstance = dependencyInfo.Instance());
        }

        public void ReleaseInstance(IBindingInfo dependencyInfo)
        {
            if (this.CachedInstance != null)
            {
                IDisposable disposable = this.CachedInstance as IDisposable;

                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
