namespace Framework.Web.Mvc
{
    using System.Security;
    using System.Web.Mvc;

    using Framework.Configuration;
    using Framework.Dynamic;

    [SecurityCritical]
    class FrameworkFilter : FilterAttribute, IResultFilter
    {
        [SecurityCritical]
        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            dynamic frameworkInfo = new ElasticObject("Framework");
            frameworkInfo.Version = WebConstants.FrameworkVersion;

            var config = ConfigManager.GetConfig(true);

            frameworkInfo.SiteName = config.Application.Name;
            frameworkInfo.CopyrightText = config.Application.CopyrightText;

            filterContext.Controller.ViewBag.Framework = frameworkInfo;


        }

        [SecurityCritical]
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}
