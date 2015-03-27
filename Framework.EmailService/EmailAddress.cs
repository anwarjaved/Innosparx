namespace Framework.Services
{
    using System;

    /// <summary>
    /// Represent Mail Address.
    /// </summary>
    [Serializable]
    public class EmailAddress
    {
        public EmailAddress()
        {
        }

        public EmailAddress(string address)
        {
            this.Address = address;
        }

        public EmailAddress(string address, string displayName)
        {
            this.Address = address;
            this.DisplayName = displayName;
        }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName { get; set; }
    }
}
