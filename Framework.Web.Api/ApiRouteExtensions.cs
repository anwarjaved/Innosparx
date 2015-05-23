using System.Collections.Generic;
using System.Linq;
using Framework.Models;

namespace Framework
{
    using System.Security;
    using System.Web.Http.Controllers;
    using System.Web.Routing;

    public static class ApiRouteExtensions
    {
        
        public static IEnumerable<RouteModel> ToRouteModel(this IEnumerable<RouteBase> routes)
        {
            return routes.Select(BuildModel)
                .GroupBy(r => r.Url).Select(x => 
                    new RouteModel() { 
                        Url = x.Key,
                        Methods = x.SelectMany(y => y.Methods).Distinct().ToList()
            });
        }


        
        private static RouteModel BuildModel(RouteBase routeBase)
        {
            RouteModel model = new RouteModel();
            Route route = routeBase as Route;

            if (route != null)
            {
                model.Url = route.Url;


                List<string> methods = new List<string>();
                if (route.DataTokens != null)
                {
                    object obj;
                    if (route.DataTokens.TryGetValue("actions", out obj))
                    {
                        ReflectedHttpActionDescriptor[] descriptors = obj as ReflectedHttpActionDescriptor[];

                        if (descriptors != null)
                        {
                            foreach (var descriptor in descriptors)
                            {
                                if (descriptor.SupportedHttpMethods != null)
                                {
                                    methods.AddRange(descriptor.SupportedHttpMethods.Select(x => x.Method));
                                }
                            }
                        }
                    }

                    if (route.DataTokens.TryGetValue("action", out obj))
                    {
                        ReflectedHttpActionDescriptor[] descriptors = obj as ReflectedHttpActionDescriptor[];

                        if (descriptors != null)
                        {
                            foreach (var descriptor in descriptors)
                            {
                                if (descriptor.SupportedHttpMethods != null)
                                {
                                    methods.AddRange(descriptor.SupportedHttpMethods.Select(x => x.Method));
                                }
                            }
                        }
                    }
                }

                model.Methods = methods;
            }

            return model;
        }
    }
}
