namespace Framework.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute for integer.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IntegerAttribute : DataTypeAttribute
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the IntegerAttribute class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public IntegerAttribute()
            : base("integer")
        {
        }

        public override string FormatErrorMessage(string name)
        {
            if (this.ErrorMessage == null && this.ErrorMessageResourceName == null)
            {
                this.ErrorMessage = "The field {0} should be a positive or negative non-decimal number.";
            }

            return base.FormatErrorMessage(name);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            int retNum;

            return int.TryParse(Convert.ToString(value), out retNum);
        }
    }
}