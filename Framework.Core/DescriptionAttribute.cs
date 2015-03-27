namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Attribute for description.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public class DescriptionAttribute : System.ComponentModel.DescriptionAttribute
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the DescriptionAttribute class.
        /// </summary>
        /// -------------------------------------------------------------------------------------------------
        public DescriptionAttribute()
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the DescriptionAttribute class.
        /// </summary>
        ///
        /// <param name="description">
        ///     The description.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public DescriptionAttribute(string description)
            : base(description)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the DescriptionAttribute class.
        /// </summary>
        ///
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="description">
        ///     The description.
        /// </param>
        ///-------------------------------------------------------------------------------------------------
        public DescriptionAttribute(string key, string description)
            : base(description)
        {
            this.Key = key;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the key.
        /// </summary>
        ///
        /// <value>
        ///     The key.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Key { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets localized description.
        /// </summary>
        ///
        /// <returns>
        ///     The localized description.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public string GetLocalizedDescription()
        {

            return this.GetLocalizedDescription(CultureInfo.CurrentCulture);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets localized description.
        /// </summary>
        ///
        /// <param name="culture">
        ///     The culture.
        /// </param>
        ///
        /// <returns>
        ///     The localized description.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public string GetLocalizedDescription( CultureInfo culture)
        {
           /* if (!string.IsNullOrWhiteSpace(this.Key))
            {
                return LocalizationManager.GetText(culture, this.Key);
            }*/

            return this.Description;
        }
    }
}
