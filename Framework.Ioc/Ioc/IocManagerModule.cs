namespace Framework.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    internal class IocManagerModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.EndRequest += OnEndRequest;
            context.BeginRequest += OnBeginRequest;
        }

        public void Dispose()
        {
        }

        private static void OnBeginRequest(object sender, System.EventArgs e)
        {
        }

        private static void OnEndRequest(object sender, System.EventArgs e)
        {
            var application = (HttpApplication)sender;
            var context = application.Context;
            if (context != null)
            {
                Dictionary<string, object> requestLifetimeInstances = context.Items[IocConstants.LifetimeManagerKey] as Dictionary<string, object>;
                if (requestLifetimeInstances != null)
                {
                    foreach (var item in requestLifetimeInstances.Values)
                    {
                        if (item is IDisposable)
                        {
                            try
                            {
                                (item as IDisposable).Dispose();
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }
    }
}
