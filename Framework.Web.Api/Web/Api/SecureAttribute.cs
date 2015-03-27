using System;
using System.Linq;

namespace Framework.Web.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Security;
    using System.Security.Claims;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    /// <summary>Specifies the authorization filter that verifies the request's <see cref="T:System.Security.Principal.IPrincipal" />.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    [SecurityCritical]
    public class SecureAttribute : AuthorizationFilterAttribute
    {
        public SecureAttribute(string role)
        {
            this.Roles = new[] { role };
        }

        public SecureAttribute(string role1, string role2)
        {
            this.Roles = new[] { role1, role2 };
        }

        public SecureAttribute(string role1, string role2, string role3)
        {
            this.Roles = new[] { role1, role2, role3 };
        }

        public SecureAttribute(string role1, string role2, string role3, string role4)
        {
            this.Roles = new[] { role1, role2, role3, role4 };
        }

        public SecureAttribute(string role1, string role2, string role3, string role4, string role5)
        {
            this.Roles = new[] { role1, role2, role3, role4, role5 };
        }

        public SecureAttribute(string role1, string role2, string role3, string role4, string role5, string role6)
        {
            this.Roles = new[] { role1, role2, role3, role4, role5, role6 };
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the SecureAttribute class.
        /// </summary>
        ///
        /// <param name="roles">
        ///     The authorized roles.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        [CLSCompliant(false)]
        public SecureAttribute(params string[] roles)
        {
            this.Roles = roles;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the SecureAttribute class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public SecureAttribute()
        {
        }

        /// <summary>Calls when an action is being authorized.</summary>
        /// <param name="actionContext">The context.</param>
        /// <exception cref="T:System.ArgumentNullException">The context parameter is null.</exception>
        [SecurityCritical]
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            if (!SkipAuthorization(actionContext) && !this.IsAuthorized(actionContext))
            {
                this.HandleUnauthorizedRequest(actionContext);
            }
        }

        /// <summary>Processes requests that fail authorization.</summary>
        /// <param name="actionContext">The context.</param>
        protected virtual void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        /// <summary>Indicates whether the specified control is authorized.</summary>
        /// <returns>true if the control is authorized; otherwise, false.</returns>
        /// <param name="actionContext">The context.</param>
        protected virtual bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException("actionContext");
            }

            ClaimsPrincipal claimsPrincipal = ClaimsPrincipal.Current;

            if ((claimsPrincipal == null) || !claimsPrincipal.Identity.IsAuthenticated)
            {
                return false;
            }
            if ((this.Users != null && this.Users.Length > 0) && !this.Users.Contains(claimsPrincipal.Identity.Name, StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            if ((this.Roles != null && this.Roles.Length > 0) && !this.Roles.Any(claimsPrincipal.IsInRole))
            {
                return false;
            }

            return true;
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            if (!actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any<AllowAnonymousAttribute>())
            {
                return actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any<AllowAnonymousAttribute>();
            }
            return true;
        }


        /// <summary>Gets or sets the authorized roles. </summary>
        /// <returns>The roles string. </returns>
        public string[] Roles
        {
            get;
            set;
        }

        /// <summary>Gets or sets the authorized users. </summary>
        /// <returns>The users. </returns>
        public string[] Users
        {
            get;
            set;
        }
    }
}
