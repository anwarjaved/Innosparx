namespace Framework.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute for maximum.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaxAttribute : DataTypeAttribute
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the maximum.
        /// </summary>
        ///
        /// <value>
        ///     The maximum value.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public object Max { get { return this.max; } }

        private readonly double max;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the MaxAttribute class.
        /// </summary>
        ///
        /// <param name="max">
        ///     The maximum value.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public MaxAttribute(int max)
            : base("max")
        {
            this.max = max;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the MaxAttribute class.
        /// </summary>
        ///
        /// <param name="max">
        ///     The maximum value.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public MaxAttribute(double max)
            : base("max")
        {
            this.max = max;
        }

        public override string FormatErrorMessage(string name)
        {
            if (this.ErrorMessage == null && this.ErrorMessageResourceName == null)
            {
                this.ErrorMessage = "The field {0} must be less than or equal to {1}";
            }

            return String.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, this.max);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            double valueAsDouble;

            var isDouble = double.TryParse(Convert.ToString(value), out valueAsDouble);

            return isDouble && valueAsDouble <= this.max;
        }
    }
}