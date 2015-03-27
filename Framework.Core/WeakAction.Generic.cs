using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    /// <summary>
    /// Stores an Action without causing a hard reference
    /// to be created to the Action's owner. The owner can be garbage collected at any time.
    /// </summary>
    /// <typeparam name="T">The type of the Action's parameter.</typeparam>
    public class WeakAction<T> : WeakAction, IWeakAction<T>

    {
        private Action<T> staticAction;

        /// <summary>
        /// Initializes a new instance of the WeakAction class.
        /// </summary>
        /// <param name="action">The action that will be associated to this instance.</param>
        public WeakAction(Action<T> action)
            : this((action == null) ? null : action.Target, action)
        {
        }

        /// <summary>
        /// Initializes a new instance of the WeakAction class.
        /// </summary>
        /// <param name="target">The action's owner.</param>
        /// <param name="action">The action that will be associated to this instance.</param>
        public WeakAction(object target, Action<T> action)
        {
            if (action.Method.IsStatic)
            {
                this.staticAction = action;
                if (target != null)
                {
                    base.Reference = new WeakReference(target);
                }
            }
            else
            {
                base.Method = action.Method;
                base.ActionReference = new WeakReference(action.Target);
                base.Reference = new WeakReference(target);
            }
        }

        /// <summary>
        /// Executes the action. This only happens if the action's owner
        /// is still alive. The action's parameter is set to default(T).
        /// </summary>
        public override void Execute()
        {
            this.Execute(default(T));
        }

        /// <summary>
        /// Executes the action. This only happens if the action's owner
        /// is still alive.
        /// </summary>
        /// <param name="parameter">A parameter to be passed to the action.</param>
        public void Execute(T parameter)
        {
            if (this.staticAction != null)
            {
                this.staticAction(parameter);
            }
            else
            {
                object actionTarget = base.ActionTarget;
                if ((this.IsAlive && (base.Method != null)) && ((base.ActionReference != null) && (actionTarget != null)))
                {
                    base.Method.Invoke(actionTarget, new object[] { parameter });
                }
            }
        }

        /// <summary>
        /// Sets all the actions that this WeakAction contains to null,
        /// which is a signal for containing objects that this WeakAction
        /// should be deleted.
        /// </summary>
        public override void MarkForDeletion()
        {
            this.staticAction = null;
            base.MarkForDeletion();
        }

        /// <summary>
        /// Gets a value indicating whether the Action's owner is still alive, or if it was collected
        /// by the Garbage Collector already.
        /// </summary>
        public override bool IsAlive
        {
            get
            {
                if ((this.staticAction == null) && (base.Reference == null))
                {
                    return false;
                }
                if (this.staticAction == null)
                {
                    return base.Reference.IsAlive;
                }
                if (base.Reference != null)
                {
                    return base.Reference.IsAlive;
                }
                return true;
            }
        }

        /// <summary>
        /// Gets the name of the method that this WeakAction represents.
        /// </summary>
        public override string MethodName
        {
            get
            {
                if (this.staticAction != null)
                {
                    return this.staticAction.Method.Name;
                }
                return base.Method.Name;
            }
        }

    }
}
