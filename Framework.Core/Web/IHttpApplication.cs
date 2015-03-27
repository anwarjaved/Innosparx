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
    ///     Interface for HTTP application.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/12/2013 12:39 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public interface IHttpApplication
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the context.
        /// </summary>
        ///
        /// <value>
        ///     The context.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        HttpContextBase Context { get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the server.
        /// </summary>
        ///
        /// <value>
        ///     The server.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        HttpServerUtilityBase Server { get; }
    }
}
