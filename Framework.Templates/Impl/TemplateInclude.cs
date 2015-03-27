namespace Framework.Templates.Impl
{
    using System.Security;

    internal class TemplateInclude : ITemplatePart
    {
        private readonly string templateName;

        public TemplateInclude(string templateName)
        {
            this.templateName = templateName;
        }

        [SecurityCritical]
        public void Render(ITemplateContext context)
        {
            object value = context.GetValue(this.templateName);

            if (value != null)
            {
                ICompiledTemplate compiledTemplate = context.GetTemplate(this.templateName);

                if (compiledTemplate != null)
                {
                    context.Write(compiledTemplate.Render(value, context.TemplateLocator));
                }
            }
        }
    }
}