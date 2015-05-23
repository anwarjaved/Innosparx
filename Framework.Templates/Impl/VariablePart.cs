namespace Framework.Templates.Impl
{
    using System.Security;
    using System.Text.RegularExpressions;
    using System.Web;

    internal class VariablePart : ITemplatePart
    {
        private static readonly Regex ParsedRegex = new Regex(
            @"^\{\s*(.+?)\s*\}$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private readonly bool escaped;

        private readonly string variableName;

        public VariablePart(string content)
        {
            Match match = ParsedRegex.Match(content);
            this.escaped = !match.Success;

            this.variableName = match.Success ? match.Groups[1].Value : content;
        }

        
        public void Render(ITemplateContext context)
        {
            object value = context.GetValue(this.variableName);

            if (value != null)
            {
                context.Write(this.escaped ? HttpUtility.HtmlEncode(value.ToString()) : value.ToString());
            }
        }
    }
}