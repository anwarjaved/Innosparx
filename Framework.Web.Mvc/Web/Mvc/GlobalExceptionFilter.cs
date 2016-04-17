namespace Framework.Web.Mvc
{
    using System;
    using System.Net;
    using System.Security;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Security;

    
    public class GlobalExceptionFilter : HandleErrorAttribute
    {
        
        public override void OnException(ExceptionContext filterContext)
        {
            bool isUserException = false;
            if (filterContext.HttpContext.Request.ContentType.StartsWith(
                "application/json", StringComparison.OrdinalIgnoreCase))
            {
                Exception exception = filterContext.Exception;
                if (exception != null)
                {
                    Type exceptionType = exception.GetType();

                    HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                    StringBuilder sb = new StringBuilder();
                    if (exceptionType == typeof(UnauthorizedAccessException))
                    {
                        statusCode = HttpStatusCode.Unauthorized;
                        sb.Append(exception.GetExceptionMessage());
                    }
                    else if (exceptionType == typeof(ArgumentException))
                    {
                        statusCode = HttpStatusCode.NotFound;
                        sb.Append(exception.GetExceptionMessage());
                    }
                    else if (exceptionType == typeof(ApiException))
                    {
                        ApiException failure = (ApiException)exception;
                        sb.Append(failure.GetExceptionMessage());
                        statusCode = failure.StatusCode;
                        isUserException = true;
                    }
                    else if (exceptionType == typeof(ApplicationException))
                    {
                        ApplicationException applicationException = (ApplicationException)exception;
                        sb.Append(applicationException.GetExceptionMessage());
                        statusCode = HttpStatusCode.Forbidden;
                        isUserException = true;
                    }
                    else
                    {
                        sb.Append(exception.GetExceptionMessage());
                    }

                    filterContext.HttpContext.Response.StatusCode = (int)statusCode;
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                    filterContext.Result = new Framework.Web.Mvc.JsonNetResult()
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new { ErrorMessage = sb.ToString(), StatusCode = statusCode }
                    };
                }
            }

            if (!isUserException)
            {
            }

            base.OnException(filterContext);
        }
    }
}
