using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Web.Mvc
{
    using System.Web.Mvc;

    using Framework.Ioc;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Manager for controllers.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public static class ControllerManager
    {
/*        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Registers this object.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="method">
        ///     The method.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void Register<T>(Func<T> method) where T : IController
        {
            if (!Container.Contains<T>())
            {
                Container.Bind<T>().InTransientScope().ToMethod(method);
            }
        }*/
    }
}
