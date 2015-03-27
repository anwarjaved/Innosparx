namespace Framework.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Validates that the property has the same value as the given 'otherProperty' 
    /// </summary>
    /// <remarks>
    /// From Mvc3 Futures
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EqualToAttribute : ValidationAttribute
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the EqualToAttribute class.
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        ///
        /// <param name="otherProperty">
        ///     The other property.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public EqualToAttribute(string otherProperty)
        {
            if (otherProperty == null)
            {
                throw new ArgumentNullException("otherProperty");
            }
            this.OtherProperty = otherProperty;
            this.OtherPropertyDisplayName = null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the other property.
        /// </summary>
        ///
        /// <value>
        ///     The other property.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string OtherProperty { get; private set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name of the other property display.
        /// </summary>
        ///
        /// <value>
        ///     The name of the other property display.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string OtherPropertyDisplayName { get; set; }

        public override string FormatErrorMessage(string name)
        {
            if (this.ErrorMessage == null && this.ErrorMessageResourceName == null)
            {
                this.ErrorMessage = "'{0}' and '{1}' do not match.";
            }

            var otherPropertyDisplayName = this.OtherPropertyDisplayName ?? this.OtherProperty;

            return String.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, otherPropertyDisplayName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var memberNames = new[] {validationContext.MemberName};

            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(this.OtherProperty);
            if (otherPropertyInfo == null)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "Could not find a property named {0}.", this.OtherProperty), memberNames);
            }

            var displayAttribute =
                otherPropertyInfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;

            if (displayAttribute != null && !string.IsNullOrWhiteSpace(displayAttribute.Name))
            {
                this.OtherPropertyDisplayName = displayAttribute.Name;
            }

            object otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
            if (!Equals(value, otherPropertyValue))
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName), memberNames);
            }
            return null;
        }
    }
}