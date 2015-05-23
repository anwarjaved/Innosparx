namespace Framework
{
    using System.Security;
    using System.Web;
    using System.Web.Mvc;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Bootstrap alert extensions.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public static class BootstrapAlertExtensions
    {
        private const string DefaultCloseSymbol = "&times;";

        private static string ToCssClass(this AlertType type)
        {
            switch (type)
            {
                case AlertType.Warning:
                    return "alert-warning";
                case AlertType.Error:
                    return "alert-error";
                case AlertType.Success:
                    return "alert-success";
                case AlertType.Info:
                    return "alert-info";
                default:
                    return "alert";
            }
        }

        
        public static IHtmlString Alert(this HtmlHelper helper, AlertType type, string text)
        {
            return helper.Alert(type, text, string.Empty, false);
        }

        
        public static IHtmlString Alert(this HtmlHelper helper, AlertType type, string text, string header)
        {
            return helper.Alert(type, text, header, false);
        }

        
        public static IHtmlString Alert(this HtmlHelper helper, AlertType type, string text, string header, bool close)
        {
            return Alert(helper, type, text, header, close, false);
        }

        
        public static IHtmlString Alert(this HtmlHelper helper, AlertType type, string text, bool close)
        {
            return Alert(helper, type, text, null, close, false);
        }

        
        public static IHtmlString Alert(this HtmlHelper helper, AlertType type, string text, string header, bool close, bool block)
        {
            return close
                       ? Alert(helper, type, text, header, ButtonTag.Default, block)
                       : Alert(helper, type, text, header, null, block);
        }

        
        public static IHtmlString Alert(this HtmlHelper helper, AlertType type, string text, string header, ButtonTag? closeButton, bool block)
        {
            var builder = new TagBuilder("div");

            if (block) builder.AddCssClass("alert-block");
            builder.AddCssClass(type.ToCssClass());
            builder.AddCssClass("alert");

            if (closeButton.HasValue)
            {
                builder.InnerHtml += DismissButton(helper, closeButton.Value);
            }

            if (!string.IsNullOrEmpty(header))
            {
                var headerTag = new TagBuilder("h4");
                headerTag.SetInnerText(header);
                builder.InnerHtml = headerTag.ToString();
            }

            builder.InnerHtml += text;
            return new MvcHtmlString(builder.ToString());
        }

        
        public static IHtmlString DismissButton(this HtmlHelper html, ButtonTag tag)
        {
            return html.Button(DefaultCloseSymbol, null, null, tag, false, false, new { @class = "close", data_dismiss = "alert" });
        }

        
        public static IHtmlString DismissButton(this HtmlHelper html, ButtonTag tag, string text)
        {
            return html.Button(text, null, null, tag, false, false, new { @class = "close", data_dismiss = "alert" });
        }
    }
}
