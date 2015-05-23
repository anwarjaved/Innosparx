namespace Framework.Templates.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security;
    using System.Text;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Mustache Compiled template.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    internal class CompiledTemplate : ICompiledTemplate
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the CompiledTemplate class.
        /// </summary>
        /// <param name="parts">
        ///     The parts.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public CompiledTemplate(IEnumerable<ITemplatePart> parts)
        {
            this.Parts = parts;
        }

        public IEnumerable<ITemplatePart> Parts { get; private set; }

        
        public string Render(object data, Func<string, string> templateLocator = null)
        {
            var sb = new StringBuilder();

            using (var context = new TemplateContext(new StringWriter(sb), data, false, templateLocator))
            {
                foreach (ITemplatePart templatePart in this.Parts)
                {
                    templatePart.Render(context);
                }
            }

            return sb.ToString();
        }

        
        public void Render(TextWriter writer, object data, Func<string, string> templateLocator = null)
        {
            using (var context = new TemplateContext(writer, data, false, templateLocator))
            {
                foreach (ITemplatePart templatePart in this.Parts)
                {
                    templatePart.Render(context);
                }
            }
        }
    }
}