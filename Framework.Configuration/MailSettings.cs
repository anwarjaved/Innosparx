namespace Framework.Configuration
{
    public class MailSettings
    {
        public MailSettings()
        {
            this.SmtpPort = 25;
        }

        /// <summary>
        ///     Gets or sets the authentication email.
        /// </summary>
        /// <value>
        ///     The authentication email.
        /// </value>
        public string AuthenticationEmail { get; set; }

        /// <summary>
        ///     Gets or sets the authentication password.
        /// </summary>
        /// <value>
        ///     The authentication password.
        /// </value>
        public string AuthenticationPassword { get; set; }

        /// <summary>
        ///     Gets or sets the contact us email.
        /// </summary>
        /// <value>
        ///     The contact us email.
        /// </value>
        public string ContactUsEmail { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [enable SSL].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [enable SSL]; otherwise, <c>false</c>.
        /// </value>
        public bool EnableSsl { get; set; }

        /// <summary>
        ///     Gets or sets from email.
        /// </summary>
        /// <value>
        ///     From email.
        /// </value>
        public string FromEmail { get; set; }

        /// <summary>
        ///     Gets or sets the SMTP port.
        /// </summary>
        /// <value>
        ///     The SMTP port.
        /// </value>
        public int SmtpPort { get; set; }

        /// <summary>
        ///     Gets or sets the SMTP server.
        /// </summary>
        /// <value>
        ///     The SMTP server.
        /// </value>
        public string SmtpServer { get; set; }
    }
}