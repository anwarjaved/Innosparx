namespace PayPalSDK.WebsiteStandard
{
    using System.Collections.Specialized;

    /// <summary>
    /// Represents Payment Information returned.
    /// </summary>
    public sealed class PaymentInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentInfo"/> class.
        /// </summary>
        public PaymentInfo()
        {
            this.Payer = new PayerInfo();
        }

        /// <summary>
        /// Gets the payer info.
        /// </summary>
        /// <value>The payer info.</value>
        public PayerInfo Payer { get; private set; }

        /// <summary>
        /// Gets  the amount.
        /// </summary>
        /// <value>The amount.</value>
        public double Amount { get; private set; }

        /// <summary>
        /// Gets the authorization expiration date.
        /// </summary>
        /// <value>The authorization expiration date.</value>
        public string AuthorizationExpirationDate { get; private set; }

        /// <summary>
        /// Gets the authorization ID.
        /// </summary>
        /// <value>The authorization ID.</value>
        public string AuthorizationID { get; private set; }

        /// <summary>
        /// Gets the invoice.
        /// </summary>
        /// <value>The invoice.</value>
        public string Invoice { get; private set; }

        /// <summary>
        /// Gets  the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; private set; }

        public void Parse(NameValueCollection values)
        {
            double value;

            if (double.TryParse(values["auth_amount"], out value))
            {
                this.Amount = value;
            }

            this.Status = values["payment_status"];

            this.Invoice = values["invoice"];

            this.AuthorizationID = values["auth_id"];
            this.AuthorizationExpirationDate = values["auth_exp"];

            this.Payer.Parse(values);
        }
    }
}