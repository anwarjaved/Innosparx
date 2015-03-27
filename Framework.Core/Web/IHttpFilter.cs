using System;

namespace Framework.Web
{
    using System.Security;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Interface for HTTP filter.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/12/2013 12:38 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    [SecurityCritical]
    public interface IHttpFilter : IDisposable
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes this IHttpFilter.
        /// </summary>
        ///
        /// <param name="application">
        ///     The application.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void Initialize(IHttpApplication application);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the begin request action.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        void OnBeginRequest(IHttpApplication application);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the end request action.
        /// </summary>
        ///
        /// <param name="application">
        ///     The application.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void OnEndRequest(IHttpApplication application);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the error action.
        /// </summary>
        ///
        /// <param name="application">
        ///     The application.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void OnError(IHttpApplication application);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the post map request action.
        /// </summary>
        ///
        /// <param name="application">
        ///     The application.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        void OnPostMapRequest(IHttpApplication application);


        void OnPostAuthenticate(IHttpApplication application);

    }
}
