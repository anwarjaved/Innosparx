using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    using System.Windows.Threading;

    public static class UIDispatcher
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Invokes an action asynchronously on the UI thread.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 04/03/2014 9:28 AM.
        /// </remarks>
        ///
        /// <param name="action">
        ///     The action that must be executed.
        /// </param>
        /// <param name="priority">
        ///     (Optional) the priority.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void RunAsync(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.BeginInvoke(action, priority, new object[0]);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Invokes an action synchronously on the UI thread.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 04/03/2014 9:28 AM.
        /// </remarks>
        ///
        /// <param name="action">
        ///     The action that must be executed.
        /// </param>
        /// <param name="priority">
        ///     (Optional) the priority.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void Run(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Invoke(action, priority, new object[0]);
            }
        }

        public static void DelayRun(Action action, int delay, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            var timer = new DispatcherTimerWithAction(TimeSpan.FromSeconds(delay), priority,
                dispatcher)
            {
                Action = action,

            };
            timer.Tick += UIDispatcherExtensions.OnTimeout;
            timer.Start();
        }

        public static void DelayRun<T>(Action<T> action, T paramater, int delay, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            var timer = new DispatcherTimerWithAction(TimeSpan.FromSeconds(delay), priority,
                    dispatcher)
            {
                Action = () => action(paramater),

            };
            timer.Tick += UIDispatcherExtensions.OnTimeout;
            timer.Start();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Invokes an action asynchronously on the UI thread.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 04/01/2014 5:17 PM.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="action">
        ///     The action that must be executed.
        /// </param>
        /// <param name="paramater">
        ///     The paramater.
        /// </param>
        /// <param name="priority">
        ///     (Optional) the priority.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void RunAsync<T>(Action<T> action, T paramater, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            if (dispatcher.CheckAccess())
            {
                action(paramater);
            }
            else
            {
                dispatcher.BeginInvoke(action, priority, new object[] { paramater });
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Invokes an action synchronously on the UI thread.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 04/03/2014 9:28 AM.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="action">
        ///     The action that must be executed.
        /// </param>
        /// <param name="paramater">
        ///     The paramater.
        /// </param>
        /// <param name="priority">
        ///     (Optional) the priority.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void Run<T>(Action<T> action, T paramater, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            if (dispatcher.CheckAccess())
            {
                action(paramater);
            }
            else
            {
                dispatcher.Invoke(action, priority, new object[] { paramater });
            }
        }

    }
}
