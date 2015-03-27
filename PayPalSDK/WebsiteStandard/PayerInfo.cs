namespace PayPalSDK.WebsiteStandard
{
    using System.Collections.Specialized;

    /// <summary>
    /// Represents Payer Information returned.
    /// </summary>
    public sealed class PayerInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PayerInfo"/> class.
        /// </summary>
        public PayerInfo()
        {
            this.Address = new AddressInfo();
        }

        /// <summary>
        /// Gets Email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; private set; }

        /// <summary>
        /// Gets FirstName.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets Payer ID.
        /// </summary>
        /// <value>The payer id.</value>
        public string ID { get; private set; }

        /// <summary>
        /// Gets LastName.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets the contact phone.
        /// </summary>
        /// <value>The contact phone.</value>
        public string ContactPhone { get; private set; }

        /// <summary>
        /// Gets Status.
        /// </summary>
        /// <value>The status.</value>
        public PayerStatus Status { get; private set; }

        /// <summary>
        /// Gets the address.
        /// </summary>
        /// <value>The address.</value>
        public AddressInfo Address { get; private set; }

        internal void Parse(NameValueCollection values)
        {
            this.Email = values["payer_email"];
            this.FirstName = values["first_name"];
            this.ID = values["payer_id"];
            this.LastName = values["last_name"];
            this.ContactPhone = values["contact_phone"];
            ////this.Status = (PayerStatus)Reflector.DescriptionToEnum(typeof(PayerStatus), values["payer_status"]);

            this.Address.Parse(values);
        }
    }
}