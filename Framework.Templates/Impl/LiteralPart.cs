namespace Framework.Templates.Impl
{
    using System.Security;

    internal class LiteralPart : ITemplatePart
    {
        private readonly string content;

        public LiteralPart(string content)
        {
            this.content = content;
        }

        
        public void Render(ITemplateContext context)
        {
            context.Write(this.content);
        }
    }
}