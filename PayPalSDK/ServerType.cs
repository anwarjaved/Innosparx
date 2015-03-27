namespace PayPalSDK
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Defines Type of the Server To Used.
    /// </summary>
    [Serializable]
    public enum ServerType
    {
        /// <summary>
        /// A server that allows you to process transactions in a real environment.
        /// </summary>
        [Description("https://www.paypal.com/cgi-bin/webscr")]
        Live,

        /// <summary>
        /// A server that allows you to process transactions in a test environment.
        /// </summary>
        [Description("https://www.sandbox.paypal.com/cgi-bin/webscr")]
        Sandbox,
    }
}