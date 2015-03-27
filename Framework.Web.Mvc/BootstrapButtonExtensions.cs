namespace Framework
{
    using System.Security;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Bootstrap button extensions.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public static class BootstrapButtonExtensions
    {
        private static string ToCssClass(this ButtonType type)
        {
            switch (type)
            {
                case ButtonType.Default:
                    return "btn";
                case ButtonType.Primary:
                    return "btn-primary";
                case ButtonType.Info:
                    return "btn-success";
                case ButtonType.Success:
                    return "btn-success";
                case ButtonType.Warning:
                    return "btn-warning";
                case ButtonType.Danger:
                    return "btn-danger";
                case ButtonType.Inverse:
                    return "btn-inverse";
                case ButtonType.Link:
                    return "btn-link";
                default:
                    return string.Empty;
            }
        }

        private static string ToCssClass(this ButtonSize type)
        {
            switch (type)
            {
                case ButtonSize.Large:
                    return "btn-large";
                case ButtonSize.Small:
                    return "btn-small";
                case ButtonSize.Mini:
                    return "btn-mini";
                default:
                    return string.Empty;
            }
        }

        private static string ToHtmlTag(this ButtonTag type)
        {
            switch (type)
            {
                case ButtonTag.Anchor:
                    return "a";
                case ButtonTag.Input:
                    return "input";
                case ButtonTag.InputSubmit:
                    return "input";
                default:
                    return "button";
            }
        }

        [SecuritySafeCritical]
        public static IHtmlString Button(this HtmlHelper html, string text)
        {
            return Button(html, text, ButtonType.Default);
        }

        [SecuritySafeCritical]
        public static IHtmlString Button(this HtmlHelper html, string text, ButtonType type)
        {
            return Button(html, text, type, ButtonSize.Default);
        }

        [SecuritySafeCritical]
        public static IHtmlString Button(this HtmlHelper html, string text, ButtonTag tag, object htmlAttributes)
        {
            return Button(html, text, ButtonType.Default, ButtonSize.Default, tag, false, false, htmlAttributes);
        }

        [SecuritySafeCritical]
        public static IHtmlString Button(this HtmlHelper html, string text, ButtonType type, ButtonSize size)
        {
            return Button(html, text, type, size, ButtonTag.Default);
        }

        [SecuritySafeCritical]
        public static IHtmlString Button(this HtmlHelper html, string text, ButtonType? type, ButtonSize? size, ButtonTag? tag)
        {
            return Button(html, text, type, size, tag, false, false);
        }

        [SecuritySafeCritical]
        public static IHtmlString Button(this HtmlHelper html, string text, ButtonType? type, ButtonSize? size, ButtonTag? tag, bool? block)
        {
            return Button(html, text, type, size, tag, block, false);
        }

        [SecuritySafeCritical]
        public static IHtmlString Button(this HtmlHelper html, string text, ButtonType? type, ButtonSize? size, ButtonTag? tag, bool? block, bool? disabled)
        {
            return Button(html, text, type, size, tag, block, disabled, null);
        }

        [SecuritySafeCritical]
        public static IHtmlString Button(this HtmlHelper html, string text, ButtonType? type, ButtonSize? size, ButtonTag? tag, bool? block, bool? disabled, object htmlAttributes)
        {
            var tagName = (tag.HasValue ? tag.Value : ButtonTag.Default).ToHtmlTag();
            var builder = new TagBuilder(tagName);

            // Adding css styles
            if (disabled.HasValue && disabled.Value)
            {
                builder.AddCssClass("disabled");
                builder.MergeAttribute("disabled", "disabled");
            }
            if (block.HasValue && block.Value)
                builder.AddCssClass("btn-block");
            if (size.HasValue && size.Value != ButtonSize.Default)
                builder.AddCssClass(size.Value.ToCssClass());
            if (type.HasValue)
            {
                if (type.Value != ButtonType.Default)
                {
                    builder.AddCssClass(type.Value.ToCssClass());
                }

                builder.AddCssClass(ButtonType.Default.ToCssClass());
            }

            // Adding html tag type
            var renderMode = TagRenderMode.Normal;
            if (tag.HasValue)
            {
                switch (tag)
                {
                    case ButtonTag.Anchor:
                        builder.MergeAttribute("href", "javascript:void(0)");
                        builder.SetInnerText(text);
                        break;
                    case ButtonTag.InputSubmit:
                        builder.MergeAttribute("type", "submit");
                        builder.MergeAttribute("value", text);
                        renderMode = TagRenderMode.SelfClosing;
                        break;
                    default:
                        builder.MergeAttribute("type", "button");
                        builder.SetInnerText(text);
                        break;
                }
            }

            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return new MvcHtmlString(builder.ToString(renderMode));
        }

        [SecuritySafeCritical]
        public static IHtmlString SubmitButton(this HtmlHelper html, string text)
        {
            return SubmitButton(html, text, ButtonType.Primary, ButtonSize.Default);
        }

        [SecuritySafeCritical]
        public static IHtmlString SubmitButton(this HtmlHelper html, string text, ButtonType? type, ButtonSize? size)
        {
            return Button(html, text, type, size, ButtonTag.InputSubmit);
        }
    }
}
