using System.Collections.Generic;

namespace Framework.Templates.Impl
{
    using System.Security;

    using Framework.Infrastructure;
    using Framework.Ioc;
    using Framework.Membership;
    using Framework.Services;

    [InjectBind(typeof(ITemplateExpression), "site:user", LifetimeType.Singleton)]
    public class SiteUserExpression : ITemplateExpression
    {
        
        public void Render(string expression, bool inverted, dynamic properties, IEnumerable<ITemplatePart> parts, ITemplateContext context)
        {
            IWebContext webContext = Container.Get<IWebContext>();

            bool previewMode = webContext.InPreviewMode();

            string name = properties.name;

            if (!string.IsNullOrWhiteSpace(name))
            {
                if (previewMode)
                {
                    RenderPreviewMode(context, name);
                }
                else
                {
                    bool isAuthenticated = webContext.IsAuthenticated;
                    if (isAuthenticated)
                    {
                        IUser user = webContext.User;

                        if (user != null)
                        {
                            switch (name)
                            {
                                case "Email":
                                    context.Write(user.Email);
                                    break;
                                case "ID":
                                    context.Write(user.ID.ToStringValue());
                                    break;
                                case "Name":
                                    context.Write(user.Name);
                                    break;
                                case "FirstName":
                                    context.Write(user.FirstName);
                                    break;
                                case "LastName":
                                    context.Write(user.LastName);
                                    break;
                            }
                        }

                    }
                }
              
            }

        }

        
        private static void RenderPreviewMode(ITemplateContext context, string name)
        {
            switch (name)
            {
                case "Email":
                    context.Write("xyx@demo.com");
                    break;
                case "ID":
                    context.Write("xxxxxxxxx");
                    break;
                case "Name":
                    context.Write("User Name");
                    break;
                case "FirstName":
                    context.Write("FirstName");
                    break;
                case "LastName":
                    context.Write("LastName");
                    break;
            }
        }
    }
}
