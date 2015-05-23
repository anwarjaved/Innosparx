namespace Framework.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using System.Web;

    using Framework.Web;
    using Framework.Web.Impl;

    
    internal class FilterHttpModule : IHttpModule
    {
        
        void IHttpModule.Dispose()
        {
            IReadOnlyList<IHttpFilter> filters = Container.TryGetAll<IHttpFilter>();

            foreach (var filter in filters)
            {
                filter.TryDispose();
            }
        }

        
        void IHttpModule.Init(HttpApplication context)
        {
            Initialize(context);
        }

        
        private static void Initialize(HttpApplication context)
        {
            IHttpApplication application = new HttpApplicationProxy(context);
            OrderedFilters.ForEach(x => x.Initialize(application));
            BindEvents(context);
        }

        
        private static void BindEvents(HttpApplication context)
        {
            context.BeginRequest += (sender, e) => OrderedFilters.ForEach(x => x.OnBeginRequest(new HttpApplicationProxy((HttpApplication)sender)));
            context.Error += (sender, e) => OrderedFilters.ForEach(x => x.OnError(new HttpApplicationProxy((HttpApplication)sender)));
            context.EndRequest += (sender, e) => OrderedFilters.ForEach(x => x.OnEndRequest(new HttpApplicationProxy((HttpApplication)sender)));
            context.PostMapRequestHandler += (sender, e) => OrderedFilters.ForEach(x => x.OnPostMapRequest(new HttpApplicationProxy((HttpApplication)sender)));
            context.PostAuthenticateRequest += (sender, e) => OrderedFilters.ForEach(x => x.OnPostAuthenticate(new HttpApplicationProxy((HttpApplication)sender)));
        }

        private static IEnumerable<IHttpFilter> OrderedFilters
        {
            
            get
            {
                Type priorityType = typeof(OrderAttribute);
                return Container.TryGetAll<IHttpFilter>().OrderBy(
                    t =>
                    {
                        Type taskType = t.GetType();

                        OrderAttribute priority = taskType.GetCustomAttributes(priorityType, false).SingleOrDefault() as OrderAttribute;

                        return priority != null ? priority.Value : int.MaxValue;
                    }).ToList();
            }
        }
    }
}
