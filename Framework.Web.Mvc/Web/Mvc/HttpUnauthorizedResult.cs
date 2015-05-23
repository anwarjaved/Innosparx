using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Web.Mvc
{
    using System.Net;
    using System.Security;
    using System.Web.Mvc;
    using System.Web.Security;

    
    public class HttpUnauthorizedResult : System.Web.Mvc.HttpUnauthorizedResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Mvc.HttpUnauthorizedResult"/> class.
        /// </summary>
        
        public HttpUnauthorizedResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Web.Mvc.HttpUnauthorizedResult"/> class using the status description.
        /// </summary>
        /// <param name="statusDescription">The status description.</param>
        
        public HttpUnauthorizedResult(string statusDescription)
            : base(statusDescription)
        {
        }

        /// <summary>Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.</summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.HttpContext.Response.StatusCode = this.StatusCode;
            if (this.StatusDescription != null)
            {
                context.HttpContext.Response.StatusDescription = this.StatusDescription;
            }

            if (!context.HttpContext.Items.Contains(WebConstants.SuppressAuthenticationKey))
            {
                var loginUrl = FormsAuthentication.LoginUrl + "?returnUrl=" + HttpUtility.UrlEncode(context.HttpContext.Request.RawUrl);
                context.HttpContext.Response.Redirect(loginUrl, false);
            }
        }
    }
}
