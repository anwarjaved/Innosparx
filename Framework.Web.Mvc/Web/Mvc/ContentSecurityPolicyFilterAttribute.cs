namespace Framework.Web.Mvc
{
    using System.Security;
    using System.Web.Mvc;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute for content security policy filter.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/25/2013 4:07 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    [SecurityCritical]
    public class ContentSecurityPolicyFilterAttribute : ActionFilterAttribute
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/25/2013 4:07 PM.
        /// </remarks>
        ///
        /// <param name="filterContext">
        ///     The filter context.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            response.AddHeader("Content-Security-Policy", "default-src 'self'; img-src *; script-src 'self'; style-src 'self' 'unsafe-inline';");
            response.AddHeader("X-WebKit-CSP", "default-src 'self'; img-src *; script-src 'self'; style-src 'self' 'unsafe-inline';");
            response.AddHeader("X-Content-Security-Policy", "default-src 'self'; img-src *; script-src 'self'; style-src 'self' 'unsafe-inline';");
            base.OnActionExecuting(filterContext);
        }
    }
}
