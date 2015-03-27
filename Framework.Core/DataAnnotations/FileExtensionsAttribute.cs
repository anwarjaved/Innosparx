namespace Framework.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute for file extensions.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FileExtensionsAttribute : DataTypeAttribute
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the extensions.
        /// </summary>
        ///
        /// <value>
        ///     The extensions.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Extensions { get; private set; }

        /// <summary>
        /// Provide the allowed file extensions, seperated via "|" (or a comma, ","), defaults to "png|jpe?g|gif" 
        /// </summary>
        public FileExtensionsAttribute(string allowedExtensions = "png,jpg,jpeg,gif")
            : base("fileextension")
        {
            this.Extensions = string.IsNullOrWhiteSpace(allowedExtensions) ? "png,jpg,jpeg,gif" : allowedExtensions.Replace("|", ",").Replace(" ", "");
        }
        
        public override string FormatErrorMessage(string name)
        {
            if (this.ErrorMessage == null && this.ErrorMessageResourceName == null)
            {
                this.ErrorMessage = "The {0} field only accepts files with the following extensions: {1}";
            }

            return String.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, this.Extensions);
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string valueAsString;

            if (value is HttpPostedFileBase)
            {
                valueAsString = (value as HttpPostedFileBase).FileName;
            }
            else
            {
                valueAsString = value as string;
            }

            if (valueAsString != null)
            {
                return this.ValidateExtension(valueAsString);
            }

            return false;
        }

        private bool ValidateExtension(string fileName)
        {
            try
            {
                return this.Extensions.Split(',').Contains(Path.GetExtension(fileName).Replace(".","").ToLowerInvariant());
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}