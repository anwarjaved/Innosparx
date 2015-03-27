namespace PayPalSDK.WebsiteStandard
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using Framework;
    using Framework.Collections;

    /// <summary>
    /// Represent Payer Details.
    /// </summary>
    public sealed class PayerDetails
    {
        public PayerDetails()
        {
            this.Country = CountryCode.UnitedStates;
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Street.
        /// </summary>
        /// <value>The Street.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string Street1 { get; set; }

        /// <summary>
        /// Gets or sets the street2.
        /// </summary>
        /// <value>The street2.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string Street2 { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>The payer city.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        [NotifyParentProperty(true)]
        [DefaultValue(typeof(CountryCode), "UnitedStates")]
        [Bindable(true)]
        public CountryCode Country { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>Postal code.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets The four-digit phone number for U.S. phone numbers. This will prepopulate the payer’s home phone number.
        /// </summary>
        /// <value>The four-digit phone number for U.S. phone numbers. This will prepopulate the payer’s home phone number.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string PhoneC { get; set; }

        /// <summary>
        /// Gets or sets The area code for U.S. phone numbers, or the country code for phone numbers outside the U.S.
        /// </summary>
        /// <value>The area code for U.S. phone numbers, or the country code for phone numbers outside the U.S.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string PhoneA { get; set; }

        /// <summary>
        /// Gets or sets the The three-digit prefix for U.S. phone numbers, or 
        /// the entire phone number for phone numbers outside the U.S., excluding country code.
        /// </summary>
        /// <value>The three-digit prefix for U.S. phone numbers, or the entire phone number for phone numbers outside the U.S., excluding country code.</value>
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        [Bindable(true)]
        public string PhoneB { get; set; }

        internal IDictionary<string, string> GetValues()
        {
            OrderedDictionary<string, string> dictionary = new OrderedDictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(this.Street1))
            {
                dictionary.Add("address1", this.Street1);
            }

            if (!string.IsNullOrWhiteSpace(this.Street2))
            {
                dictionary.Add("address2", this.Street2);
            }

            if (!string.IsNullOrWhiteSpace(this.City))
            {
                dictionary.Add("city", this.City);
            }

            dictionary.Add("country", this.Country.ToDescription());

            if (!string.IsNullOrWhiteSpace(this.Email))
            {
                dictionary.Add("email", this.Email);
            }

            if (!string.IsNullOrWhiteSpace(this.FirstName))
            {
                dictionary.Add("first_name", this.FirstName);
            }

            if (!string.IsNullOrWhiteSpace(this.LastName))
            {
                dictionary.Add("last_name", this.LastName);
            }

            if (!string.IsNullOrWhiteSpace(this.State))
            {
                dictionary.Add("state", this.State);
            }

            if (!string.IsNullOrWhiteSpace(this.Zip))
            {
                dictionary.Add("zip", this.Zip);
            }

            if (this.Country == CountryCode.UnitedStates)
            {
                dictionary.Add("night_phone_a", this.PhoneA);
                dictionary.Add("night_phone_b", this.PhoneB);
                dictionary.Add("night_phone_c", this.PhoneC);
            }
            else
            {
                dictionary.Add("night_phone_a", this.PhoneA);
                dictionary.Add("night_phone_b", this.PhoneB);
            }

            return dictionary;
        }
    }
}