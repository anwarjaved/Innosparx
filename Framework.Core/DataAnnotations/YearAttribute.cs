namespace Framework.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute for year.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class YearAttribute : DataTypeAttribute
    {
        private static readonly Regex YearRegex = new Regex(@"^[0-9]{4}$");

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the regular expression.
        /// </summary>
        ///
        /// <value>
        ///     The regular expression.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Regex
        {
            get
            {
                return YearRegex.ToString();
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the YearAttribute class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public YearAttribute()
            : base("year")
        {
        }

        public override string FormatErrorMessage(string name)
        {
            if (this.ErrorMessage == null && this.ErrorMessageResourceName == null)
            {
                this.ErrorMessage = "The {0} field is not a valid year.";
            }

            return base.FormatErrorMessage(name);
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            int retNum;
            var parseSuccess = int.TryParse(Convert.ToString(value), out retNum);

            return parseSuccess && retNum >= 1 && retNum <= 9999;
        }
    }
}
