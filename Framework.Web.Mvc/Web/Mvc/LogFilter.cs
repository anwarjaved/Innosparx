namespace Framework.Web.Mvc
{
    using System;
    using System.Security;
    using System.Web.Mvc;

    using Framework.Logging;

    [SecurityCritical]
    class LogFilter : FilterAttribute, IResultFilter
    {
        [SecurityCritical]
        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var controllerName = Convert.ToString(filterContext.Controller.ValueProvider.GetValue("controller").RawValue);
            var actionName = Convert.ToString(filterContext.Controller.ValueProvider.GetValue("action").RawValue);

            var message = Logger.Executing("{0}/{1}".FormatString(controllerName, actionName));
            Logger.Info(message, WebConstants.FilterComponent);
        }

        [SecurityCritical]
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var controllerName = Convert.ToString(filterContext.Controller.ValueProvider.GetValue("controller").RawValue);
            var actionName = Convert.ToString(filterContext.Controller.ValueProvider.GetValue("action").RawValue);


            if (filterContext.Exception != null)
            {
                Logger.Error(filterContext.Exception, WebConstants.FilterComponent);
            }
            else
            {
                var message = Logger.Completed(message:"{0}/{1}".FormatString(controllerName, actionName));
                Logger.Info(message, WebConstants.FilterComponent);
            }
        }
    }
}
