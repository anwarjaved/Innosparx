namespace PayPalSDK.WebsiteStandard
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// PayPal status of a user Address.
    /// </summary>
    [Serializable]
    public enum PayerStatus
    {
        /// <summary>
        /// No Status specified.
        /// </summary>
        [Description("")]
        None,

        /// <summary>
        /// The address is verified.
        /// </summary>
        [Description("verified")]
        Verified,

        /// <summary>
        /// PayPal responds that the address is unverified.
        /// </summary>
        [Description("unverified")]
        Unverified,
    }
}