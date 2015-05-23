using System.Text;

namespace Framework
{
    using System.ComponentModel;
    using System.Security;
    using System.Web;
    using System.Web.Mvc;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     HTML string extensions.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public static class HtmlStringExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Converts a tag to a HTML string.
        /// </summary>
        ///
        /// <param name="tag">
        ///     The tag.
        /// </param>
        ///
        /// <returns>
        ///     tag as an IHtmlString.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        internal static IHtmlString ToHtmlString(string tag)
        {
            return MvcHtmlString.Create(tag);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the empty.
        /// </summary>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        internal static IHtmlString Empty()
        {
            return MvcHtmlString.Empty;
        }

        
        internal static IHtmlString ToHtmlString(StringBuilder tag)
        {
            return MvcHtmlString.Create(tag.ToString());
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Converts a tag to a HTML string.
        /// </summary>
        ///
        /// <param name="builder">
        ///     The builder to act on.
        /// </param>
        /// <param name="mode">
        ///     (optional) the mode.
        /// </param>
        ///
        /// <returns>
        ///     tag as an IHtmlString.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        public static IHtmlString ToHtmlString(this TagBuilder builder, TagRenderMode mode = TagRenderMode.SelfClosing)
        {
            return MvcHtmlString.Create(builder.ToString(mode));
        }
    }
}
