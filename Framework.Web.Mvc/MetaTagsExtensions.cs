namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Meta Tags Extensions.
    /// </summary>
    public static class MetaTagsExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A HtmlHelper extension method that adds a generator meta tag to 'generator'.
        /// </summary>
        ///
        /// <param name="htmlHelper">
        ///     The htmlHelper to act on.
        /// </param>
        /// <param name="generator">
        ///     (optional) the generator.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        public static IHtmlString AddGeneratorMetaTag(this HtmlHelper htmlHelper, string generator = "")
        {
            if (string.IsNullOrWhiteSpace(generator))
            {
                generator = WebConstants.FrameworkVersion;
            }

            TagBuilder builder = new TagBuilder("meta");
            builder.Attributes.Add("name", "generator");
            builder.Attributes.Add("content", generator);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}
