namespace Framework.Infrastructure
{
    using System.ComponentModel;
    using System.Security;
    using System.Web.Compilation;
    using System.Web.Mvc;
    using System.Web.Razor;
    using System.Web.WebPages;
    using System.Web.WebPages.Razor;

    using Framework.Activator;
    using Framework.Ioc;
    using Framework.Web;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Pre application start code.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [EditorBrowsable(EditorBrowsableState.Never)]
    
    public static class WebInitializer
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initialises this object.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        
        public static void Init()
        {
            BuildProvider.RegisterBuildProvider(".mustactml", typeof(RazorBuildProvider));

              // add the new extensions to the collection of languages supported by Razor
            RazorCodeLanguage.Languages.Add("mustactml", new CSharpRazorCodeLanguage());

             // register the extensions
            WebPageHttpHandler.RegisterExtension("mustactml");
        }
    }
}
