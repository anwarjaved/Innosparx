namespace Framework.Web.Mvc.UI
{
    using System.Security;
    using System.Web;

    /// <summary>
    /// Basic Interface for Control.
    /// </summary>
    /// <author>Anwar</author>
    /// <datetime>1/25/2011 3:43 PM</datetime>
    
    public abstract class Control
    {
        /// <summary>
        /// Renders this control.
        /// </summary>
        /// <returns>Output as MvcHtmlString.</returns>
        /// <author>Anwar</author>
        /// <datetime>1/25/2011 3:44 PM</datetime>
        public abstract IHtmlString Render();
    }
}
