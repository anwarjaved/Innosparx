using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    /// <summary>
    /// Stores an Func without causing a hard reference
    /// to be created to the Func's owner. The owner can be garbage collected at any time.
    /// </summary>
    /// <typeparam name="T">The type of the Func's parameter.</typeparam>
    /// <typeparam name="TResult">The type of the Func's return value.</typeparam>
    public class WeakFunc<T, TResult> : WeakFunc<TResult>, IWeakFunc<T, TResult>
    {
        private Func<T, TResult> staticFunc;

        /// <summary>
        /// Initializes a new instance of the WeakFunc class.
        /// </summary>
        /// <param name="func">The Func that will be associated to this instance.</param>
        public WeakFunc(Func<T, TResult> func)
            : this((func == null) ? null : func.Target, func)
        {
        }

        /// <summary>
        /// Initializes a new instance of the WeakFunc class.
        /// </summary>
        /// <param name="target">The Func's owner.</param>
        /// <param name="func">The Func that will be associated to this instance.</param>
        public WeakFunc(object target, Func<T, TResult> func)
        {
            if (func.Method.IsStatic)
            {
                this.staticFunc = func;
                if (target != null)
                {
                    base.Reference = new WeakReference(target);
                }
            }
            else
            {
                base.Method = func.Method;
                base.FuncReference = new WeakReference(func.Target);
                base.Reference = new WeakReference(target);
            }
        }

        /// <summary>
        /// Executes the Func. This only happens if the Func's owner
        /// is still alive. The Func's parameter is set to default(T).
        /// </summary>
        /// <returns>The result of the Func stored as reference.</returns>
        public override TResult Execute()
        {
            return this.Execute(default(T));
        }

        /// <summary>
        /// Executes the Func. This only happens if the Func's owner
        /// is still alive.
        /// </summary>
        /// <param name="parameter">A parameter to be passed to the action.</param>
        /// <returns>The result of the Func stored as reference.</returns>
        public TResult Execute(T parameter)
        {
            if (this.staticFunc != null)
            {
                return this.staticFunc(parameter);
            }
            object funcTarget = base.FuncTarget;
            if ((this.IsAlive && (base.Method != null)) && ((base.FuncReference != null) && (funcTarget != null)))
            {
                return (TResult)base.Method.Invoke(funcTarget, new object[] { parameter });
            }
            return default(TResult);
        }

        /// <summary>
        /// Sets all the funcs that this WeakFunc contains to null,
        /// which is a signal for containing objects that this WeakFunc
        /// should be deleted.
        /// </summary>
        public override void MarkForDeletion()
        {
            this.staticFunc = null;
            base.MarkForDeletion();
        }

        /// <summary>
        /// Gets a value indicating whether the Func's owner is still alive, or if it was collected
        /// by the Garbage Collector already.
        /// </summary>
        public override bool IsAlive
        {
            get
            {
                if ((this.staticFunc == null) && (base.Reference == null))
                {
                    return false;
                }
                if (this.staticFunc == null)
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
        /// Gets or sets the name of the method that this WeakFunc represents.
        /// </summary>
        public override string MethodName
        {
            get
            {
                if (this.staticFunc != null)
                {
                    return this.staticFunc.Method.Name;
                }
                return base.Method.Name;
            }
        }
    }

 

}
