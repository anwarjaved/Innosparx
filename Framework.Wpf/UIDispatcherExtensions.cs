using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    using System.Windows.Threading;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A dispatcher.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 04/01/2014 5:11 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public static class UIDispatcherExtensions
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
        /// <param name="dispatcher">
        ///     The dispatcher to act on.
        /// </param>
        /// <param name="action">
        ///     The action that must be executed.
        /// </param>
        /// <param name="priority">
        ///     (Optional) the priority.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void RunAsync(this Dispatcher dispatcher, Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
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
        /// <param name="dispatcher">
        ///     The dispatcher to act on.
        /// </param>
        /// <param name="action">
        ///     The action that must be executed.
        /// </param>
        /// <param name="priority">
        ///     (Optional) the priority.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public static void Run(this Dispatcher dispatcher, Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                dispatcher.Invoke(action, priority, new object[0]);
            }
        }

        internal static void OnTimeout(object sender, EventArgs arg)
        {
            var t = sender as DispatcherTimerWithAction;
            if (t != null)
            {
                t.Stop();

                if (t.Dispatcher.CheckAccess())
                {
                    t.Action();
                }
                else
                {
                    t.Dispatcher.Invoke(t.Action, new object[0]);
                }
                t.Tick -= OnTimeout;
            }
            
        }

        public static void DelayRun(this Dispatcher dispatcher, Action action, int delay, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            var timer = new DispatcherTimerWithAction(TimeSpan.FromSeconds(delay), priority,
                dispatcher)
            {
                Action = action,
                
            };
            timer.Tick += OnTimeout;
            timer.Start();
        }

        public static void DelayRun<T>(this Dispatcher dispatcher, Action<T> action, T paramater, int delay, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            var timer = new DispatcherTimerWithAction(TimeSpan.FromSeconds(delay), priority,
                    dispatcher)
            {
                Action = () => action(paramater),

            };
            timer.Tick += OnTimeout;
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
        /// <param name="dispatcher">
        ///     The dispatcher to act on.
        /// </param>
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
        public static void RunAsync<T>(this Dispatcher dispatcher, Action<T> action, T paramater, DispatcherPriority priority = DispatcherPriority.Normal)
        {
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
        /// <param name="dispatcher">
        ///     The dispatcher to act on.
        /// </param>
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
        public static void Run<T>(this Dispatcher dispatcher, Action<T> action, T paramater, DispatcherPriority priority = DispatcherPriority.Normal)
        {
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
