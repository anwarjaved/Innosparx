using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    using System.Windows.Threading;

    internal class DispatcherTimerWithAction : System.Windows.Threading.DispatcherTimer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Threading.DispatcherTimer"/> class.
        /// </summary>
        public DispatcherTimerWithAction()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Threading.DispatcherTimer"/> class which processes timer events at the specified priority.
        /// </summary>
        /// <param name="priority">The priority at which to invoke the timer.</param>
        public DispatcherTimerWithAction(DispatcherPriority priority)
            : base(priority)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Threading.DispatcherTimer"/> class which runs on the specified <see cref="T:System.Windows.Threading.Dispatcher"/> at the specified priority.
        /// </summary>
        /// <param name="priority">The priority at which to invoke the timer.</param><param name="dispatcher">The dispatcher the timer is associated with.</param><exception cref="T:System.ArgumentNullException"><paramref name="dispatcher"/> is null.</exception>
        public DispatcherTimerWithAction(DispatcherPriority priority, Dispatcher dispatcher)
            : base(priority, dispatcher)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Windows.Threading.DispatcherTimer"/> class which uses the specified time interval, priority, event handler, and <see cref="T:System.Windows.Threading.Dispatcher"/>.
        /// </summary>
        /// <param name="interval">The period of time between ticks.</param><param name="priority">The priority at which to invoke the timer.</param><param name="callback">The event handler to call when the <see cref="E:System.Windows.Threading.DispatcherTimer.Tick"/> event occurs.</param><param name="dispatcher">The dispatcher the timer is associated with.</param><exception cref="T:System.ArgumentNullException"><paramref name="dispatcher"/> is null.</exception><exception cref="T:System.ArgumentNullException"><paramref name="callback"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="interval"/> is less than 0 or greater than <see cref="F:System.Int32.MaxValue"/>.</exception>
        public DispatcherTimerWithAction(TimeSpan interval, DispatcherPriority priority, Dispatcher dispatcher)
            : base(interval, priority, null, dispatcher)
        {
        }

        public Action Action { get; set; }
    }
}
