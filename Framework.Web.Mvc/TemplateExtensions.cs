using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    using System.ComponentModel;
    using System.Security;

    using Framework.Infrastructure;
    using Framework.Services;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class TemplateExtensions
    {
        
        public static void SetPreviewMode(this IWebContext context, bool value)
        {
            if (value)
            {
                context.Items[WebConstants.TemplatePreviewMode] = true;
            }
            else
            {
                context.Items.Remove(WebConstants.TemplatePreviewMode);
            }
        }

        
        public static bool InPreviewMode(this IWebContext context)
        {
            return context.Items[WebConstants.TemplatePreviewMode] != null;
        }
    }
}
