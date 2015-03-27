namespace Framework.Activator
{
    using System;
    using System.Reflection;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    /// Base class of all the activation attributes.
    /// </summary>
    /// <remarks>
    /// LM ANWAR, 6/2/2013.
    /// </remarks>
    /// -------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public abstract class BaseActivationMethodAttribute : Attribute
    {
        private readonly Type type;
        private readonly string methodName;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the BaseActivationMethodAttribute class.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="methodName">
        /// The name of the method.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        protected BaseActivationMethodAttribute(Type type, string methodName)
        {
            this.type = type;
            this.methodName = methodName;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public Type Type
        {
            get
            {
                return this.type;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        /// <value>
        /// The name of the method.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public string MethodName
        {
            get
            {
                return this.methodName;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether method run in designer mode (in addition to running at runtime)
        /// </summary>
        /// <value><c>true</c> if [run in designer]; otherwise, <c>false</c>.</value>
        public bool RunInDesigner { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Executes the method on a different thread, and waits for the result.
        /// </summary>
        /// <remarks>
        /// LM ANWAR, 6/2/2013.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// -------------------------------------------------------------------------------------------------
        public void InvokeMethod()
        {
            // Get the method
            MethodInfo method = Type.GetMethod(this.MethodName, BindingFlags.Static | BindingFlags.Public, null, new Type[0], null);

            if (method == null)
            {
                throw new ArgumentException(
                    string.Format("The type {0} doesn't have a static method named {1}", Type, this.MethodName));
            }

            // Invoke it
            method.Invoke(null, null);
        }
    }
}