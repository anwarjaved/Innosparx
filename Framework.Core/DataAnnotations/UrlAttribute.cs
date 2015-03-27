namespace Framework.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    using DataAnnotationsExtensions;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute for url.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UrlAttribute : DataTypeAttribute
    {
        /// <summary>
        /// The base URL regular expression.
        /// </summary>
        /// <remarks>
        /// RFC-952 describes basic name standards: http://www.ietf.org/rfc/rfc952.txt
        /// KB 909264 describes Windows name standards: http://support.microsoft.com/kb/909264
        /// </remarks>
        private const string BaseUrlExpression = @"(((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|([a-zA-Z][\-a-zA-Z0-9]*)|((([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?";

        /// <summary>
        /// The base protocol regular expression.
        /// </summary>
        private const string BaseProtocolExpression = @"(https?|ftp):\/\/";

        private readonly UrlOptions urlOptions = UrlOptions.RequireProtocol; //Default to require protocol

        private readonly string regex;

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
                return this.regex;
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the UrlAttribute class.
        /// </summary>
        ///
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        ///
        /// <param name="urlOptions">
        ///     (optional) options for controlling the URL.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public UrlAttribute(UrlOptions urlOptions = UrlOptions.RequireProtocol) : base(DataType.Url)
        {
            this.urlOptions = urlOptions;

            switch (urlOptions)
            {
                case UrlOptions.RequireProtocol:
                    this.regex = @"^" + BaseProtocolExpression + BaseUrlExpression + @"$";
                    break;
                case UrlOptions.OptionalProtocol:
                    this.regex = @"^(" + BaseProtocolExpression + @")?" + BaseUrlExpression + @"$";
                    break;
                case UrlOptions.DisallowProtocol:
                    this.regex = @"^" + BaseUrlExpression + @"$";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("urlOptions");
            }
        }

        public override string FormatErrorMessage(string name)
        {
            if (this.ErrorMessage == null && this.ErrorMessageResourceName == null)
            {
                switch (this.urlOptions)
                {
                    case UrlOptions.RequireProtocol:
                        this.ErrorMessage = "The {0} field is not a valid fully-qualified http, https, or ftp URL.";
                        break;
                    case UrlOptions.OptionalProtocol:
                        this.ErrorMessage = "The {0} field is not a valid URL.";
                        break;
                    case UrlOptions.DisallowProtocol:
                        this.ErrorMessage = "The {0} field is not a valid protocol-less URL.";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return base.FormatErrorMessage(name);
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var valueAsString =  value is Uri ? value.ToString() : value as string;
            
            return valueAsString != null &&
                   new Regex(this.regex, RegexOptions.Compiled | RegexOptions.IgnoreCase).Match(valueAsString).Length > 0;
        }
    }
}