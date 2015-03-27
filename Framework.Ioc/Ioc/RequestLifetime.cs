namespace Framework.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web;

    using Framework.Fakes;

    /// <summary>
    /// This lifetime manager’s behavior is to always return a attempt to retrieve the instance from Request.Items when the Resolve method is called.  If the instance does not exist in Request.Items, the a new instance is created by executing the factory method, and storing it in the Request.Items.
    /// </summary>
    public class RequestLifetime : ILifetime
    {
        private static Func<HttpContextBase> contextFunc = BuildContextFunc();

        /// <summary>
        /// Gets or sets the context function.
        /// </summary>
        /// <value>
        /// The context function.
        /// </value>
        /// <author>Anwar</author>
        /// <date>11/9/2011</date>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
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
        /// <date>11/9/2011</date>
        public object GetInstance(IBindingInfo dependencyInfo)
        {
            object instance;
            HttpContextBase context = contextFunc();
            if (context != null && !context.IsFakeContext())
            {
                Dictionary<string, object> instanceCache = context.Items[IocConstants.LifetimeManagerKey] as Dictionary<string, object>
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

                context.Items[IocConstants.LifetimeManagerKey] = instanceCache;
            }
            else
            {
                instance = dependencyInfo.Instance();
            }

            return instance;
        }

        /// <summary>
        /// Dispose the instance if exists.
        /// </summary>
        /// <param name="dependencyInfo">The binding info.</param>
        /// <author>Anwar</author>
        /// <date>11/9/2011</date>
        public void ReleaseInstance(IBindingInfo dependencyInfo)
        {
            // Dont Do Anything
        }

        private static Func<HttpContextBase> BuildContextFunc()
        {
            return () =>
            {
                var context = HttpContext.Current;

                if (context != null)
                {
                    return new HttpContextWrapper(context);
                }

                return FakeHttpContext.Root();
            };
        }
    }
}
