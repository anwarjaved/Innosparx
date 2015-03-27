namespace Framework.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute for numeric.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NumericAttribute : DataTypeAttribute
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the NumericAttribute class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public NumericAttribute(int precision = 2) : base("numeric")
        {
            this.Precision = precision;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the precision.
        /// </summary>
        ///
        /// <value>
        ///     The precision.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public int Precision { get; set; }

        public override string FormatErrorMessage(string name)
        {
            if (this.ErrorMessage == null && this.ErrorMessageResourceName == null)
            {
                this.ErrorMessage = "The {0} field is not a valid number.";
            }

            return base.FormatErrorMessage(name);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            double retNum;

            return double.TryParse(Convert.ToString(value), out retNum);
        }
    }
}
