using System;
using System.Linq;

namespace Framework.Web.Mvc
{
    using System.Security;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Mvc;

    using Framework.Membership;


    /// <summary>Represents an attribute that is used to restrict access by callers to an action method.</summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    
    public class SecureAttribute : FilterAttribute, IAuthorizationFilter
    {
        public SecureAttribute(string role)
        {
            this.Roles = new[] { role };
        }

        public SecureAttribute(string role1, string role2)
        {
            this.Roles = new []{ role1, role2 };
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

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets URL of the unauthorized request.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 04/03/2014 2:47 PM.
        /// </remarks>
        ///
        /// <value>
        ///     The unauthorized request URL.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string UnauthorizedRequestUrl { get; set; }

        /// <summary>Called when a process requests authorization.</summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="filterContext" /> parameter is null.</exception>
        
        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
            {
                throw new InvalidOperationException("SecureAttribute cannot be used within a child action caching block.");
            }
            if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                if (this.IsAuthorized(filterContext.HttpContext))
                {
                    HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                    cache.SetProxyMaxAge(new TimeSpan(0L));
                    cache.AddValidationCallback((HttpContext context, object o, ref HttpValidationStatus status) => this.CacheValidateHandler(context, o, ref status), null);
                }
                else
                {
                    this.HandleUnauthorizedRequest(filterContext);
                }
            }
        }

        /// <summary>Processes HTTP requests that fail authorization.</summary>
        /// <param name="filterContext">Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />. The <paramref name="filterContext" /> object contains the controller, HTTP context, request context, action result, and route data.</param>
        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (string.IsNullOrWhiteSpace(this.UnauthorizedRequestUrl))
            {
                filterContext.Result = new HttpUnauthorizedResult();

            }
            else
            {
                filterContext.Result = new RedirectResult(this.UnauthorizedRequestUrl);
            }
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = this.OnCacheAuthorization(new HttpContextWrapper(context));
        }

        /// <summary>Called when the caching module requests authorization.</summary>
        /// <returns>A reference to the validation status.</returns>
        /// <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="httpContext" /> parameter is null.</exception>
        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            if (!this.IsAuthorized(httpContext))
            {
                return HttpValidationStatus.IgnoreThisRequest;
            }
            return HttpValidationStatus.Valid;
        }


        /// <summary>Indicates whether the specified control is authorized.</summary>
        /// <returns>true if the control is authorized; otherwise, false.</returns>
        /// <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="httpContext" /> parameter is null.</exception>
        protected virtual bool IsAuthorized(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
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
    }
}
