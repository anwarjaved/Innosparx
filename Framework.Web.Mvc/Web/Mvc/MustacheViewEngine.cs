namespace Framework.Web.Mvc
{
    using System.Security;
    using System.Web.Mvc;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Mustache view engine.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/16/2013 7:55 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    
    public class MustacheViewEngine : BuildManagerViewEngine
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the MustacheViewEngine class.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 7:55 PM.
        /// </remarks>
        ///-------------------------------------------------------------------------------------------------
        public MustacheViewEngine()
        {
            // Define the location of the View file. 
            this.ViewLocationFormats = new[] { "~/Views/{1}/{0}.mustactml", "~/Views/Shared/{0}.mustactml" };

            this.PartialViewLocationFormats = new[] { "~/Views/{1}/{0}.mustactml", "~/Views/Shared/{0}.mustactml" };

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates the specified partial view by using the specified controller context.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 7:55 PM.
        /// </remarks>
        ///
        /// <param name="controllerContext">
        ///     The controller context.
        /// </param>
        /// <param name="partialPath">
        ///     The partial path for the new partial view.
        /// </param>
        ///
        /// <returns>
        ///     A reference to the partial view.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return new MustacheView(controllerContext, partialPath);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates the specified view by using the controller context, path of the view, and path of
        ///     the master view.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 7:55 PM.
        /// </remarks>
        ///
        /// <param name="controllerContext">
        ///     The controller context.
        /// </param>
        /// <param name="viewPath">
        ///     The path of the view.
        /// </param>
        /// <param name="masterPath">
        ///     The path of the master view.
        /// </param>
        ///
        /// <returns>
        ///     A reference to the view.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return new MustacheView(controllerContext, viewPath, masterPath, this.ViewPageActivator);
        }
    }
}
