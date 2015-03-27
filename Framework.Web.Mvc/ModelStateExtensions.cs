using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    using System.ComponentModel;
    using System.Security;
    using System.Web.Mvc;
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Browsable(false)]
    public static class ModelStateExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A ModelStateDictionary extension method that adds a model error to 'exception'.
        /// </summary>
        ///
        /// <param name="state">
        ///     The state to act on.
        /// </param>
        /// <param name="exception">
        ///     The exception.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        [SecurityCritical]
        public static void AddModelError(this ModelStateDictionary state, Exception exception)
        {
            if (exception != null)
            {
                Exception baseException = exception.GetBaseException();

                state.AddModelError("", baseException.Message);
            }
        }
    }
}
