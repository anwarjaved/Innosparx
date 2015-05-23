namespace Framework.Web.Mvc
{
    using System.Security;
    using System.Web.Mvc;

    using Framework.Configuration;
    using Framework.Dynamic;

    
    class FrameworkFilter : FilterAttribute, IResultFilter
    {
        
        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            dynamic frameworkInfo = new ElasticObject("Framework");
            frameworkInfo.Version = WebConstants.FrameworkVersion;

            var config = ConfigManager.GetConfig(true);

            frameworkInfo.SiteName = config.Application.Name;
            frameworkInfo.CopyrightText = config.Application.CopyrightText;

            filterContext.Controller.ViewBag.Framework = frameworkInfo;


        }

        
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}
