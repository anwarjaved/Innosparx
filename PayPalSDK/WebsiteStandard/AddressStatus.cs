namespace PayPalSDK.WebsiteStandard
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Status of Address.
    /// </summary>
    [Serializable]
    public enum AddressStatus
    {
        /// <summary>
        /// No Address Specified.
        /// </summary>
        [Description("")]
        None,

        /// <summary>
        /// Address Confirmed.
        /// </summary>
        [Description("Confirmed")]
        Confirmed,

        /// <summary>
        /// Address Unconfirmed.
        /// </summary>
        [Description("Unconfirmed")]
        Unconfirmed,
    }
}