namespace Framework
{
    using System.Security;
    using System.Web;
    using System.Web.Mvc;


    /// <summary>
    /// Script Extensions.
    /// </summary>
    public static class CSSExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An UrlHelper extension method that renders the CSS class if controller.
        /// </summary>
        ///
        /// <param name="url">
        ///     The URL to act on.
        /// </param>
        /// <param name="controller">
        ///     The controller.
        /// </param>
        /// <param name="cssClass">
        ///     The CSS class.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        public static IHtmlString RenderCssClassIfController(this UrlHelper url, string controller, string cssClass)
        {
            if (url.IsCurrentController(controller))
            {
                return HtmlStringExtensions.ToHtmlString(cssClass);
            }

            return HtmlStringExtensions.Empty();
        }
    }
}
