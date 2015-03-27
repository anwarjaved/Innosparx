namespace Framework.Templates
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Security;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TemplateExtentions
    {
        [SecurityCritical]
        public static void Render(this IEnumerable<ITemplatePart> parts, ITemplateContext context)
        {
            if (parts != null)
            {
                foreach (ITemplatePart templatePart in parts)
                {
                    templatePart.Render(context);
                }
            }
        }
    }
}