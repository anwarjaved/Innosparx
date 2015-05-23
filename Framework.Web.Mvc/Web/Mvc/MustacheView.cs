using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Infrastructure;

namespace Framework.Web.Mvc
{
    using System.IO;
    using System.Security;
    using System.Web.Mvc;

    using Framework.Caching;
    using Framework.Dynamic;
    using Framework.Ioc;
    using Framework.Templates;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Mustache view.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/16/2013 7:55 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    
    public class MustacheView : BuildManagerCompiledView
    {
        private readonly string masterPath;
        private const string CacheKey = "MustacheViewFileContents";

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the MustacheView class.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 7:55 PM.
        /// </remarks>
        ///
        /// <param name="controllerContext">
        ///     Context for the controller.
        /// </param>
        /// <param name="viewPath">
        ///     Full pathname of the view file.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public MustacheView(ControllerContext controllerContext, string viewPath)
            : base(controllerContext, viewPath)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the MustacheView class.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 7:55 PM.
        /// </remarks>
        ///
        /// <param name="controllerContext">
        ///     Context for the controller.
        /// </param>
        /// <param name="viewPath">
        ///     Full pathname of the view file.
        /// </param>
        /// <param name="masterPath">
        ///     Full pathname of the master file.
        /// </param>
        /// <param name="viewPageActivator">
        ///     The view page activator.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public MustacheView(ControllerContext controllerContext, string viewPath, string masterPath, IViewPageActivator viewPageActivator)
            : base(controllerContext, viewPath, viewPageActivator)
        {
            this.masterPath = masterPath;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     When overridden in a derived class, renders the specified view context by using the
        ///     specified writer object and object instance.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 7:55 PM.
        /// </remarks>
        ///
        /// <param name="viewContext">
        ///     Information related to rendering a view, such as view data, temporary data, and form
        ///     context.
        /// </param>
        /// <param name="writer">
        ///     The writer object.
        /// </param>
        /// <param name="instance">
        ///     An object that contains additional information that can be used in the view.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        
        protected override void RenderView(ViewContext viewContext, TextWriter writer, object instance)
        {
            dynamic model = viewContext.ViewData.Model.ToExpando();
            model.IsAuthenticated = viewContext.RequestContext.HttpContext.Request.IsAuthenticated;
            string filePath = viewContext.HttpContext.Server.MapPath(this.ViewPath);
            var compiledTemplate = GetFromCache(filePath);
            compiledTemplate.Render(writer, model);
        }

        
        private static ICompiledTemplate GetFromCache(string filePath)
        {
            string cacheKey = "{0}::{1}".FormatString(CacheKey, filePath);

            ICache cache = Container.Get<ICache>();
            ITemplateEngine engine = Container.Get<ITemplateEngine>();

            ICompiledTemplate compiledTemplate = cache.Get<ICompiledTemplate>(cacheKey);

            if (compiledTemplate == null)
            {
                compiledTemplate = engine.CompileFile(filePath);

                cache.Set(cacheKey, compiledTemplate, filePath);
            }

            return compiledTemplate;
        }
    }
}
