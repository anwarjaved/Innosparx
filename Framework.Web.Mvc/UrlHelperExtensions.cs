namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// UrlHelper Extensions.
    /// </summary>
    public static class UrlHelperExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generate Absolute Action Link.
        /// </summary>
        ///
        /// <param name="url">
        ///     The relative URL.
        /// </param>
        /// <param name="action">
        ///     The action.
        /// </param>
        /// <param name="controller">
        ///     The controller.
        /// </param>
        ///
        /// <returns>
        ///     Absolute Url.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public static string AbsoluteAction(this UrlHelper url, string action, string controller)
        {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;

            string absoluteAction = string.Format("{0}://{1}{2}", requestUrl.Scheme, requestUrl.Authority, url.Action(action, controller));

            return absoluteAction;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generate Absolute Action Link.
        /// </summary>
        ///
        /// <param name="url">
        ///     The relative URL.
        /// </param>
        /// <param name="action">
        ///     The action.
        /// </param>
        /// <param name="controller">
        ///     The controller.
        /// </param>
        /// <param name="routeValues">
        ///     The route values.
        /// </param>
        ///
        /// <returns>
        ///     Absolute Url.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public static string AbsoluteAction(this UrlHelper url, string action, string controller, RouteValueDictionary routeValues = null)
        {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;

            string absoluteAction = string.Format("{0}://{1}{2}", requestUrl.Scheme, requestUrl.Authority, url.Action(action, controller, routeValues));
            return absoluteAction;
        }

        /// <summary>
        /// Generate Absolute Action Link.
        /// </summary>
        /// <param name="url">The relative URL.</param>
        /// <param name="action">The action.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>Absolute Url.</returns>
        [SecurityCritical]
        public static string AbsoluteAction(this UrlHelper url, string action, string controller, object routeValues = null)
        {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;

            string absoluteAction = string.Format("{0}://{1}{2}", requestUrl.Scheme, requestUrl.Authority, url.Action(action, controller, routeValues));

            return absoluteAction;
        }

        /// <summary>
        /// Determines whether the specified controller is current controller.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="controller">The controller.</param>
        /// <returns>
        /// <see langword="true"/> If the specified controller is current controller; otherwise, <see langword="false"/>.
        /// </returns>
        [SecurityCritical]
        public static bool IsCurrentController(this UrlHelper urlHelper, string controller)
        {
            return urlHelper.RequestContext.RouteData.Values["controller"] != null && urlHelper.RequestContext.RouteData.Values["controller"].ToString().Equals(controller, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified action is current action.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="action">The action.</param>
        /// <returns>
        /// <see langword="true"/> If the specified action is current action.; otherwise, <see langword="false"/>.
        /// </returns>
        [SecurityCritical]
        public static bool IsCurrentAction(this UrlHelper urlHelper, string action)
        {
            return urlHelper.RequestContext.RouteData.Values["action"] != null && urlHelper.RequestContext.RouteData.Values["action"].ToString().Equals(action, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified controller action is current controller action..
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="action">The action.</param>
        /// <param name="controller">The controller.</param>
        /// <returns>
        /// <see langword="true"/> If the specified controller action is current controller action; otherwise, <see langword="false"/>.
        /// </returns>
        [SecurityCritical]
        public static bool IsCurrentControllerAction(this UrlHelper urlHelper, string action, string controller)
        {
            return urlHelper.IsCurrentController(controller) && urlHelper.IsCurrentAction(action);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ViewContext extension method that gets current controller name.
        /// </summary>
        ///
        /// <param name="viewContext">
        ///     The viewContext to act on.
        /// </param>
        ///
        /// <returns>
        ///     The current controller name.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public static string GetCurrentControllerName(this ViewContext viewContext)
        {
            return Convert.ToString(viewContext.Controller.ValueProvider.GetValue("controller").RawValue);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ViewContext extension method that gets current action name.
        /// </summary>
        ///
        /// <param name="viewContext">
        ///     The viewContext to act on.
        /// </param>
        ///
        /// <returns>
        ///     The current action name.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public static string GetCurrentActionName(this ViewContext viewContext)
        {
            return Convert.ToString(viewContext.Controller.ValueProvider.GetValue("action").RawValue);
        }
    }
}
