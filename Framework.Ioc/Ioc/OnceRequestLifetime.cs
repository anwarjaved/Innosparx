namespace Framework.Ioc
{
    using System;
    using System.Web;

    /// <summary>
    /// This lifetime manager’s behavior is to always return a attempt to retrieve the instance from Request.Items when the Resolve method is called.  
    /// If the instance does not exist in Request.Items, the a new instance is created only once by executing the factory method, and storing it in the Request.Items.
    /// </summary>
    public class OnceRequestLifetime : ILifetime
    {
        private static Func<HttpContextBase> contextFunc = () => new HttpContextWrapper(HttpContext.Current);

        /// <summary>
        /// Gets or sets the context function.
        /// </summary>
        /// <value>
        /// The context function.
        /// </value>
        /// <date>11/9/2011</date>
        /// <author>
        /// Anwar
        /// </author>
        internal static Func<HttpContextBase> ContextFunc
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
            object instance = null;
            HttpContextBase context = contextFunc();

            if (context != null)
            {
                instance = context.Items[dependencyInfo.UniqueID];

                if (instance == null)
                {
                    instance = dependencyInfo.Instance();

                    context.Items[dependencyInfo.UniqueID] = instance;
                }
                else
                {
                    instance = null;
                }
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
            HttpContextBase context = contextFunc();
            if (context != null)
            {
                object instance = context.Items[dependencyInfo.UniqueID];

                if (instance != null)
                {
                    IDisposable disposable = instance as IDisposable;

                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }

                    context.Items.Remove(dependencyInfo.UniqueID);
                }
            }
        }
    }
}
