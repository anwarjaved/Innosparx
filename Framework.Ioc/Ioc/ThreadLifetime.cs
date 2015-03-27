namespace Framework.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// This lifetime manager behavior is to always return a the same instance when the 
    /// Resolve method is called on same Thread.
    /// </summary>
    public class ThreadLifetime : ILifetime
    {
        private static readonly ThreadLocal<Dictionary<Func<object>, object>> Instances = new ThreadLocal<Dictionary<Func<object>, object>>(() => new Dictionary<Func<object>, object>());

        private static readonly ThreadLocal<object> SyncLock = new ThreadLocal<object>(() => new object());

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="dependencyInfo">
        /// Information describing the dependency.
        /// </param>
        /// <returns>
        /// Instance of Dependency Object.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public object GetInstance(IBindingInfo dependencyInfo)
        {
            if (Instances.Value.ContainsKey(dependencyInfo.Instance))
            {
                return Instances.Value[dependencyInfo.Instance];
            }

            lock (SyncLock.Value)
            {
                if (Instances.Value.ContainsKey(dependencyInfo.Instance))
                {
                    return Instances.Value[dependencyInfo.Instance];
                }

                object instance = dependencyInfo.Instance();

                Instances.Value.Add(dependencyInfo.Instance, instance);

                return instance;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Dispose the instance if exists.
        /// </summary>
        /// <param name="dependencyInfo">
        /// Information describing the dependency.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public void ReleaseInstance(IBindingInfo dependencyInfo)
        {
            lock (SyncLock.Value)
            {
                if (Instances.Value.ContainsKey(dependencyInfo.Instance))
                {
                    object instance = Instances.Value[dependencyInfo.Instance];

                    if (instance != null)
                    {
                        IDisposable disposable = instance as IDisposable;

                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }

                    Instances.Value.Remove(dependencyInfo.Instance);
                }
            }
        }
    }
}
