using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPalSDK.WebsiteStandard
{
    /// <summary>
    /// Class IPNResponse. This class cannot be inherited.
    /// </summary>
    public sealed class IPNResponse
    {
        public IPNResponse()
        {
            this.Payment = new PaymentInfo();
        }

        /// <summary>
        /// Gets the business email.
        /// </summary>
        /// <value>
        /// The business email.
        /// </value>
        public string BusinessEmail { get; set; }
        
             /// <summary>
        /// Gets the Payment info.
        /// </summary>
        /// <value>The Payment info.</value>
        public PaymentInfo Payment { get; set; }

        /// <summary>
        /// Gets the reciever email.
        /// </summary>
        /// <value>The reciever email.</value>
        public string ReceiverEmail { get; set; }

        /// <summary>
        /// Gets the reciever ID.
        /// </summary>
        /// <value>The reciever ID.</value>
        public string ReceiverID { get; set; }

        /// <summary>
        /// Gets the reciever country.
        /// </summary>
        /// <value>The reciever country.</value>
        public CountryCode ReceiverCountry { get; set; }

        /// <summary>
        /// Gets the transaction ID.
        /// </summary>
        /// <value>The transaction ID.</value>
        public string TransactionID { get; set; }

        /// <summary>
        /// Gets the parent transaction ID.
        /// </summary>
        /// <value>The parent transaction ID.</value>
        public string ParentTransactionID { get; set; }

        /// <summary>
        /// Gets the Custom value as passed by you, the merchant.
        /// </summary>
        /// <value>Custom value as passed by you, the merchant.</value>
        public string Custom { get; set; }

        /// <summary>
        /// Gets the type of the transaction.
        /// </summary>
        /// <value>The type of the transaction.</value>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// Gets the transaction subject.
        /// </summary>
        /// <value>The transaction subject.</value>
        public string TransactionSubject { get; set; }
    }
}
