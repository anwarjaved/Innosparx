using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    using System.ComponentModel;
    using System.Security;
    using System.Web.Mvc;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ControllerExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a <see cref="T:System.Web.Mvc.ViewResult" /> object using the view name, master-
        ///     page name, and model that renders a view.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 10:07 PM.
        /// </remarks>
        ///
        /// <param name="controller">
        ///     The controller to act on.
        /// </param>
        /// <param name="viewName">
        ///     The name of the view that is rendered to the response.
        /// </param>
        /// <param name="masterName">
        ///     The name of the master page or template to use when the view is rendered.
        /// </param>
        /// <param name="model">
        ///     The model that is rendered by the view.
        /// </param>
        ///
        /// <returns>
        ///     The view result.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        public static ViewResult MustacheView(this Controller controller, string viewName, string masterName, object model)
        {
            if (model != null)
            {
                controller.ViewData.Model = model;
            }

            return new ViewResult { ViewName = viewName, MasterName = masterName, ViewData = controller.ViewData, TempData = controller.TempData, ViewEngineCollection = controller.ViewEngineCollection };

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a <see cref="T:System.Web.Mvc.ViewResult" /> object using the view name and
        ///     master-page name that renders a view to the response.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 10:08 PM.
        /// </remarks>
        ///
        /// <param name="controller">
        ///     The controller to act on.
        /// </param>
        /// <param name="viewName">
        ///     The name of the view that is rendered to the response.
        /// </param>
        /// <param name="masterName">
        ///     The name of the master page or template to use when the view is rendered.
        /// </param>
        ///
        /// <returns>
        ///     The view result.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        public static ViewResult MustacheView(this Controller controller, string viewName, string masterName)
        {
            return controller.MustacheView(viewName, masterName, null);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a <see cref="T:System.Web.Mvc.ViewResult" /> object by using the view name and
        ///     model that renders a view to the response.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 10:08 PM.
        /// </remarks>
        ///
        /// <param name="controller">
        ///     The controller to act on.
        /// </param>
        /// <param name="viewName">
        ///     The name of the view that is rendered to the response.
        /// </param>
        /// <param name="model">
        ///     The model that is rendered by the view.
        /// </param>
        ///
        /// <returns>
        ///     The view result.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        public static ViewResult MustacheView(this Controller controller, string viewName, object model)
        {
            return controller.MustacheView(viewName, null, model);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a <see cref="T:System.Web.Mvc.ViewResult" /> object by using the view name that
        ///     renders a view.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 10:09 PM.
        /// </remarks>
        ///
        /// <param name="controller">
        ///     The controller to act on.
        /// </param>
        /// <param name="viewName">
        ///     The name of the view that is rendered to the response.
        /// </param>
        ///
        /// <returns>
        ///     The view result.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        public static ViewResult MustacheView(this Controller controller, string viewName)
        {
            return controller.MustacheView(viewName, null, null);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a <see cref="T:System.Web.Mvc.ViewResult" /> object by using the model that
        ///     renders a view to the response.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 10:09 PM.
        /// </remarks>
        ///
        /// <param name="controller">
        ///     The controller to act on.
        /// </param>
        /// <param name="model">
        ///     The model that is rendered by the view.
        /// </param>
        ///
        /// <returns>
        ///     The view result.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        public static ViewResult MustacheView(this Controller controller, object model)
        {
            return controller.MustacheView(null, null, model);
        }

        /// <summary>Creates a <see cref="T:System.Web.Mvc.ViewResult" /> object that renders a view to the response.</summary>
        /// <returns>The view result that renders a view to the response.</returns>
        
        public static ViewResult MustacheView(this Controller controller)
        {
            return controller.MustacheView(null, null, null);
        }
    }
}
