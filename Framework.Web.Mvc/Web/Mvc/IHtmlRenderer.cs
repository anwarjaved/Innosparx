namespace Framework.Web.Mvc
{
    using System.Security;
    using System.Web.Mvc;
    using System.Web.UI;

    /// <summary>
    /// Control Html Renderer Interface.
    /// </summary>
    
    public interface IHtmlRenderer
    {
        /// <summary>
        /// Renders html to the specified writer.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="writer">The writer.</param>
        void Render(HtmlHelper htmlHelper, HtmlTextWriter writer);
    }
}
