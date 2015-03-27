namespace PayPalSDK.WebsiteStandard
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Indicates the payment type.
    /// </summary>
    [Serializable]
    public enum PaymentType
    {
        /// <summary>
        /// No Payment Type.
        /// </summary>
        [Description("")]
        None,

        /// <summary>
        /// This payment was funded with an eCheck.
        /// </summary>
        [Description("echeck")]
        ECheck,

        /// <summary>
        /// This payment was funded with PayPal balance, credit card, or Instant Transfer.
        /// </summary>
        [Description("instant")]
        Instant,
    }
}