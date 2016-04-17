namespace Framework.Web.Api
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Security;
    using System.Text;
    using System.Web.Http.Filters;

    
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            bool isUserException = false;
            Exception exception = actionExecutedContext.Exception;
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
                statusCode = HttpStatusCode.BadRequest;
                isUserException = true;
            }
            else
            {
                sb.Append(exception.GetExceptionMessage());
            }

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(statusCode, new { ErrorMessage = sb.ToString(), StatusCode = statusCode });

            if (!isUserException)
            {
            }

            base.OnException(actionExecutedContext);
        }

     
    }
}
