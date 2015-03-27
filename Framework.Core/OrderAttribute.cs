namespace Framework
{
    using System;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute for order.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.All)]
    public class OrderAttribute : Attribute
    {
        private readonly int value;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderAttribute"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public OrderAttribute(int value = Int32.MaxValue)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        /// <value>The priority.</value>
        public int Value
        {
            get
            {
                return this.value;
            }
        }
    }
}