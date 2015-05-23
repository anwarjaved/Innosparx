using System;

namespace Framework
{
    using System.ComponentModel;
    using System.Security;
    using System.Web.Mvc;
    using System.Web.WebPages;

    [EditorBrowsable(EditorBrowsableState.Never)]
    
    public static class ConditionalExtensions
    {
        /// <summary>
        /// A helper for performing conditional IF,ELSE logic using Razor
        /// </summary>
        
        public static HelperResult IfElse(this HtmlHelper html, bool condition, Func<dynamic, HelperResult> trueString, Func<dynamic, HelperResult> falseString = null)
        {
            return new HelperResult(writer =>
            {
                if (condition)
                {
                    if (trueString != null)
                    {   
                        trueString(null).WriteTo(writer);
                    }
                }
                else
                {
                    if (falseString != null)
                    {
                        falseString(null).WriteTo(writer);
                    }
                }
            });
        }

        /// <summary>
        /// A helper for performing conditional IF,ELSE logic using Razor
        /// </summary>
        
        public static HelperResult LoginView(this HtmlHelper html, Func<dynamic, HelperResult> itemTemplate, Func<dynamic, HelperResult> anonymousTemplate = null)
        {
            bool isAuthenticated = html.ViewContext.HttpContext.Request.IsAuthenticated;
            return html.IfElse(isAuthenticated, itemTemplate, anonymousTemplate);
        }

        
        public static HelperResult LoginViewForRole(this HtmlHelper html, string roleName, Func<dynamic, HelperResult> itemTemplate, Func<dynamic, HelperResult> anonymousTemplate = null)
        {
            var user = html.ViewContext.HttpContext.User;
            return html.IfElse(user.IsInRole(roleName), itemTemplate, anonymousTemplate);
        }

        /// <summary>
        /// A helper for performing conditional IF logic using Razor
        /// </summary>
        
        public static HelperResult If(this HtmlHelper html, bool condition, Func<dynamic, HelperResult> action)
        {
            return html.IfElse(condition, action);
        }

    }
}
