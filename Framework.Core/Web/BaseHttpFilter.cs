namespace Framework.Web
{
    using System.Security;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Base HTTP filter.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 09/12/2013 1:19 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    
    public abstract class BaseHttpFilter : DisposableObject, IHttpFilter
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes this IHttpFilter.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/12/2013 1:19 PM.
        /// </remarks>
        ///
        /// <param name="application">
        ///     The application.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        
        public virtual void Initialize(IHttpApplication application)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the begin request action.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/12/2013 1:19 PM.
        /// </remarks>
        ///
        /// <param name="application">
        ///     The application.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        
        public virtual void OnBeginRequest(IHttpApplication application)
        {
        }

        
        public virtual void OnEndRequest(IHttpApplication application)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the error action.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/12/2013 1:24 PM.
        /// </remarks>
        ///
        /// <param name="application">
        ///     The application.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        
        public virtual void OnError(IHttpApplication application)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the post map request action.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/12/2013 1:50 PM.
        /// </remarks>
        ///
        /// <param name="application">
        ///     The application.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        
        public virtual void OnPostMapRequest(IHttpApplication application)
        {
        }

        
        public virtual void OnPostAuthenticate(IHttpApplication application)
        {
        }
    }
}
