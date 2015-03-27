using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Web
{
    using System.Web;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Null HTML string.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 10/09/2013 7:29 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class NullHtmlString : IHtmlString
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns an HTML-encoded string.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 10/09/2013 7:29 PM.
        /// </remarks>
        ///
        /// <returns>
        ///     An HTML-encoded string.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public string ToHtmlString()
        {
            return string.Empty;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 10/09/2013 7:29 PM.
        /// </remarks>
        ///
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            return this.ToHtmlString();
        }
    }
}
