namespace Framework.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute for email.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EmailAttribute : DataTypeAttribute
    {
        private static readonly Regex EmailRegex =
            new Regex(
                @"^(?:[a-zA-Z0-9_'^&/+-])+(?:\.(?:[a-zA-Z0-9_'^&/+-])+)*@(?:(?:\[?(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\.){3}(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\]?)|(?:[a-zA-Z0-9-]+\.)+(?:[a-zA-Z]){2,}\.?)$",
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase
                | RegexOptions.Singleline);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the EmailAttribute class.
        /// </summary>
        ///-------------------------------------------------------------------------------------------------
        public EmailAttribute()
            : base(DataType.EmailAddress)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            if (this.ErrorMessage == null && this.ErrorMessageResourceName == null)
            {
                this.ErrorMessage = "The {0} field is not a valid e-mail address.";
            }

            return base.FormatErrorMessage(name);
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string valueAsString = value as string;
            return valueAsString != null && EmailRegex.Match(valueAsString).Length > 0;
        }
    }
}
