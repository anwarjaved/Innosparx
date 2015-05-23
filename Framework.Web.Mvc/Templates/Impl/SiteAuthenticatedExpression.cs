using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Templates.Impl
{
    using System.Security;

    using Framework.Infrastructure;
    using Framework.Ioc;
    using Framework.Services;

    [InjectBind(typeof(ITemplateExpression), "site:isAuthenticated", LifetimeType.Singleton)]
    public class SiteAuthenticatedExpression : ITemplateExpression
    {
        
        public void Render(string expression, bool inverted, dynamic properties, IEnumerable<ITemplatePart> parts, ITemplateContext context)
        {
            IWebContext webContext = Container.Get<IWebContext>();

            bool isAuthenticated = webContext.IsAuthenticated;

            if (webContext.InPreviewMode())
            {
                isAuthenticated = false;
            }

            if ((!inverted && isAuthenticated) || (inverted && !isAuthenticated))
            {
                parts.Render(context);

            }
        }
    }
}
