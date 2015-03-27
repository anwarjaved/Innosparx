namespace Framework.Templates.Impl
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Security;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Framework.Infrastructure;
    using Framework.Ioc;
    using Framework.Services;

    using Container = Framework.Ioc.Container;

    [InjectBind(typeof(ITemplateExpression), "site:route", LifetimeType.Singleton)]
    public class SiteRouteExpression : ITemplateExpression
    {
        private static readonly Regex MarkerRegex = new Regex(@"\{(\w+)\}", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        [SecurityCritical]
        public void Render(string expression, bool inverted, dynamic properties, IEnumerable<ITemplatePart> parts, ITemplateContext context)
        {
            IWebContext webContext = Container.Get<IWebContext>();

            if (!webContext.InPreviewMode())
            {
                string action = properties.action ?? "index";
                string controller = properties.controller;

                RouteValueDictionary routeValueDict = properties.data != null ? new RouteValueDictionary(properties.data) : new RouteValueDictionary();
                if (!string.IsNullOrWhiteSpace(action) || !string.IsNullOrWhiteSpace(controller))
                {
                    UrlHelper url = new UrlHelper(webContext.RequestContext);
                    string route = url.Action(action, controller, routeValueDict);
                    context.Write(route);
                }
            }
            else
            {
                context.Write("#");
            }
        }
    }
}
