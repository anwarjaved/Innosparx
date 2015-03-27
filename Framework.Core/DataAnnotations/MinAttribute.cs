namespace Framework.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute for minimum.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinAttribute : DataTypeAttribute
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the minimum.
        /// </summary>
        ///
        /// <value>
        ///     The minimum value.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public object Min { get { return this.min; } }

        private readonly double min;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the MinAttribute class.
        /// </summary>
        ///
        /// <param name="min">
        ///     The minimum value.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public MinAttribute(int min) : base("min")
        {
            this.min = min;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the MinAttribute class.
        /// </summary>
        ///
        /// <param name="min">
        ///     The minimum value.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public MinAttribute(double min) : base("min")
        {
            this.min = min;
        }

        public override string FormatErrorMessage(string name)
        {
            if (this.ErrorMessage == null && this.ErrorMessageResourceName == null)
            {
                this.ErrorMessage = "The field {0} must be greater than or equal to {1}";
            }

            return String.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, this.min);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            
            double valueAsDouble;

            var isDouble = double.TryParse(Convert.ToString(value), out valueAsDouble);

            return isDouble && valueAsDouble >= this.min;
        }
    }
}
