namespace Framework.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Fake extensions.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public static class FakeExtensions
    {
       /// -------------------------------------------------------------------------------------------------
       /// <summary>
       /// A HttpContextBase extension method that query if 'httpContext' is fake context.
       /// </summary>
       /// <exception cref="ArgumentNullException">
       /// Thrown when one or more required arguments are null.
       /// </exception>
       /// <param name="httpContext">
       /// The httpContext to act on.
       /// </param>
       /// <returns>
       /// True if fake context, false if not.
       /// </returns>
       /// --------------------------------------------------------------------------------------------------
        public static bool IsFakeContext(this HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            return httpContext is FakeHttpContext;
        }
    }
}
