namespace Framework.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    /// <summary>
    /// This lifetime manager’s behavior is to always return a attempt to retrieve the instance from Session when the Resolve method is called.  If the instance does not exist in Session, the a new instance is created by executing the factory method, and storing it in the Session.
    /// </summary>
    public class SessionLifetime : ILifetime
    {
        private static Func<HttpContextBase> contextFunc = () => new HttpContextWrapper(HttpContext.Current);

        /// <summary>
        /// Gets or sets the context function.
        /// </summary>
        /// <value>
        /// The context function.
        /// </value>
        /// <author>Anwar</author>
        /// <date>11/10/2011</date>
        public static Func<HttpContextBase> ContextFunc
        {
            get
            {
                return contextFunc;
            }

            set
            {
                contextFunc = value;
            }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="dependencyInfo">The binding info.</param>
        /// <returns>
        /// Instance of Dependency Object.
        /// </returns>
        /// <author>Anwar</author>
        /// <date>11/10/2011</date>
        public object GetInstance(IBindingInfo dependencyInfo)
        {
            object instance = null;
            HttpContextBase context = contextFunc();

            if (context != null && context.Session != null)
            {
                Dictionary<string, object> instanceCache = context.Session[IocConstants.LifetimeManagerKey] as Dictionary<string, object>
                                                           ??
                                                           new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

                if (instanceCache.ContainsKey(dependencyInfo.UniqueID))
                {
                    instance = instanceCache[dependencyInfo.UniqueID];
                }
                else
                {
                    instance = dependencyInfo.Instance();

                    instanceCache.Add(dependencyInfo.UniqueID, instance);
                }

                context.Session[IocConstants.LifetimeManagerKey] = instanceCache;
            }

            return instance;
        }

        /// <summary>
        /// Dispose the instance if exists.
        /// </summary>
        /// <param name="dependencyInfo">The binding info.</param>
        /// <author>Anwar</author>
        /// <date>11/10/2011</date>
        public void ReleaseInstance(IBindingInfo dependencyInfo)
        {
            // Do Nothing
        }
    }
}
