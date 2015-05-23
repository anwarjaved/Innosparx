namespace PayPalSDK.WebsiteStandard
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Security;

    using Framework;

    /// <summary>
    /// The base button class to store general properties.
    /// </summary>
    [DefaultProperty("PayPalButtonUrl")]
    public abstract class PayPalButton
    {
        protected PayPalButton()
        {
            this.Settings = new DisplayDetails();
            this.Server = ServerType.Sandbox;
        }

/*        /// <summary>
        /// Gets or sets The URL to which PayPal posts information about the
        /// transaction, in the form of Instant Payment Notification messages.
        /// </summary>
        /// <value>The URL to which PayPal posts information about the
        /// transaction, in the form of Instant Payment Notification messages.</value>
        [DefaultValue("")]
        [System.ComponentModel.Description("The URL of the image to be shown.")]
        [Bindable(true)]
        [Category("PayPal")]
        public string NotifyUrl { get; set; }*/

        /// <summary>
        /// Gets the display settings.
        /// </summary>
        /// <value>The display settings.</value>
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DisplayDetails Settings { get; private set; }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>The server.</value>
        [DefaultValue(typeof(ServerType), "Sandbox")]
        [Category("PayPal")]
        public ServerType Server { get; set; }

        /// <summary>
        /// Gets or sets the business email.
        /// </summary>
        /// <value>The business email.</value>
        [DefaultValue("")]
        [Category("PayPal")]
        public string BusinessEmail { get; set; }

        
        public abstract string GetRedirectUrl(string state, string successUrl, string failureUrl);
    }
}