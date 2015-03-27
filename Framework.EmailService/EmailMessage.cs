namespace Framework.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.Mail;

    using Framework.Ioc;
    using Framework.Serialization.Json;

    /// <summary>
    /// Represents Email Message.
    /// </summary>
    [Serializable]
    public class EmailMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessage"/> class.
        /// </summary>
        public EmailMessage()
        {
            this.BCC = new Collection<EmailAddress>();
            this.To = new Collection<EmailAddress>();
            this.CC = new Collection<EmailAddress>();
            this.ReplyTo = new Collection<EmailAddress>();
        }

        private EmailMessage(MailMessage message) : this()
        {
            foreach (var address in message.Bcc)
            {
                this.BCC.Add(ConvertMailAddressToEmailAddress(address));
            }

            foreach (var address in message.To)
            {
                this.To.Add(ConvertMailAddressToEmailAddress(address));
            }

            foreach (var address in message.CC)
            {
                this.CC.Add(ConvertMailAddressToEmailAddress(address));
            }

            foreach (var address in message.ReplyToList)
            {
                this.ReplyTo.Add(ConvertMailAddressToEmailAddress(address));
            }

            this.Body = message.Body;
            this.From = ConvertMailAddressToEmailAddress(message.From);
            this.IsBodyHtml = message.IsBodyHtml;
            this.Subject = message.Subject;
            this.Sender = ConvertMailAddressToEmailAddress(message.Sender);
        }

        /// <summary>
        /// Gets the BCC.
        /// </summary>
        /// <value>The email BCC.</value>
        public Collection<EmailAddress> BCC { get; internal set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The email body.</value>
        public string Body { get; set; }

        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets the cc.
        /// </summary>
        /// <value>The email cc.</value>
        public Collection<EmailAddress> CC { get; internal set; }

        public DateTime? SendDate { get; set; }

        public int RetryAttempt { get; set; }

        public string ErrorMessage { get; set; }


        /// <summary>
        /// Gets or sets from email .
        /// </summary>
        /// <value>From email.</value>
        public EmailAddress From { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is body HTML.
        /// </summary>
        /// <value>
        /// <see langword="true"/> If this instance is body HTML; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// Gets the reply to.
        /// </summary>
        /// <value>The reply to.</value>
        public Collection<EmailAddress> ReplyTo { get; internal set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>The sender.</value>
        public EmailAddress Sender { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets Recivers email ids.
        /// </summary>
        /// <value>To Email address.</value>
        public Collection<EmailAddress> To { get; internal set; }

        /// <summary>
        /// Deserializes the specified serialized my mail message.
        /// </summary>
        /// <param name="serializedemailMessage">The serialized my mail message.</param>
        /// <returns>Serialized <see cref="MailMessage"/>.</returns>
        internal static MailMessage DeserializeInternal(string serializedemailMessage)
        {
            var jsonSerializer = Container.Get<IJsonSerializer>();
            EmailMessage mmm = jsonSerializer.Deserialize<EmailMessage>(serializedemailMessage);
            MailMessage mm = new MailMessage();
            foreach (EmailAddress a in mmm.To)
            {
                mm.To.Add(ConvertEmailAddressToMailAddress(a));
            }

            foreach (EmailAddress a in mmm.CC)
            {
                mm.CC.Add(ConvertEmailAddressToMailAddress(a));
            }

            foreach (EmailAddress a in mmm.BCC)
            {
                mm.Bcc.Add(ConvertEmailAddressToMailAddress(a));
            }

            foreach (EmailAddress a in mmm.ReplyTo)
            {
                mm.ReplyToList.Add(ConvertEmailAddressToMailAddress(a));
            }

            mm.Body = mmm.Body;
            mm.IsBodyHtml = mmm.IsBodyHtml;

            mm.Sender = ConvertEmailAddressToMailAddress(mmm.Sender);
            mm.Subject = mmm.Subject;
            mm.From = ConvertEmailAddressToMailAddress(mmm.From);
            return mm;
        }

        /// <summary>
        /// Deserializes the encrypted.
        /// </summary>
        /// <param name="serializedAndEncryptedemailMessage">The serialized and encrypted my mail message.</param>
        /// <returns>Deserialized <see cref="MailMessage"/>.</returns>
        internal static MailMessage DeserializeEncryptedInternal(string serializedAndEncryptedemailMessage)
        {
            return DeserializeInternal(Cryptography.Decrypt(serializedAndEncryptedemailMessage, "SomeSaltAndPepper"));
        }

        /// <summary>
        /// Serializes the specified mail message.
        /// </summary>
        /// <param name="mailMessage">The mail message.</param>
        /// <returns>Serialized <see cref="MailMessage"/>.</returns>
        internal static string SerializeInternal(MailMessage mailMessage)
        {
            var jsonSerializer = Container.Get<IJsonSerializer>();

            return jsonSerializer.Serialize(new EmailMessage
            {
                BCC = new Collection<EmailAddress>(ConvertMailAddressToEmailAddress(mailMessage.Bcc)),
                Body = mailMessage.Body,
                CC = new Collection<EmailAddress>(ConvertMailAddressToEmailAddress(mailMessage.CC)),
                From = ConvertMailAddressToEmailAddress(mailMessage.From),
                IsBodyHtml = mailMessage.IsBodyHtml,
                ReplyTo = new Collection<EmailAddress>(ConvertMailAddressToEmailAddress(mailMessage.ReplyToList)),
                Sender = ConvertMailAddressToEmailAddress(mailMessage.Sender),
                Subject = mailMessage.Subject,
                To = new Collection<EmailAddress>(ConvertMailAddressToEmailAddress(mailMessage.To))
            });
        }

        public static string Serialize(EmailMessage message)
        {
            return EmailMessage.SerializeInternal(message.ToMailMessage());
        }

        public static EmailMessage Deserialize(string serializedemailMessage)
        {
            return new EmailMessage(EmailMessage.DeserializeInternal(serializedemailMessage));
        }

        /// <summary>
        /// Serializes the encrypted mail message.
        /// </summary>
        /// <param name="mailMessage">The mail message.</param>
        /// <returns>Serialized Encrypted <see cref="MailMessage"/>.</returns>
        internal static string SerializeEncrypted(MailMessage mailMessage)
        {
            return Cryptography.Encrypt(SerializeInternal(mailMessage), "SomeSaltAndPepper");
        }

        internal MailMessage ToMailMessage()
        {
            MailMessage mm = new MailMessage();
            foreach (EmailAddress item in this.To)
            {
                mm.To.Add(ConvertEmailAddressToMailAddress(item));
            }

            foreach (EmailAddress item in this.CC)
            {
                mm.CC.Add(ConvertEmailAddressToMailAddress(item));
            }

            foreach (EmailAddress item in this.BCC)
            {
                mm.Bcc.Add(ConvertEmailAddressToMailAddress(item));
            }

            foreach (EmailAddress item in this.ReplyTo)
            {
                mm.ReplyToList.Add(ConvertEmailAddressToMailAddress(item));
            }

            mm.From = ConvertEmailAddressToMailAddress(this.From);

            mm.Sender = ConvertEmailAddressToMailAddress(this.Sender);

            mm.Subject = this.Subject;
            mm.Body = this.Body;
            mm.IsBodyHtml = this.IsBodyHtml;
            return mm;
        }

        /// <summary>
        /// Converts the mail address to my mail address.
        /// </summary>
        /// <param name="mailAddress">The mail address.</param>
        /// <returns>EmaiAddress object.</returns>
        private static EmailAddress ConvertMailAddressToEmailAddress(MailAddress mailAddress)
        {
            EmailAddress ma = new EmailAddress();
            if (mailAddress != null)
            {
                ma.Address = mailAddress.Address;
                ma.DisplayName = mailAddress.DisplayName;
            }

            return ma;
        }

        private static IList<EmailAddress> ConvertMailAddressToEmailAddress(IEnumerable<MailAddress> mailAddresses)
        {
            return mailAddresses.Select(ConvertMailAddressToEmailAddress).ToList();
        }

        private static MailAddress ConvertEmailAddressToMailAddress(EmailAddress emailAddress)
        {
            MailAddress ma = null;
            if (((emailAddress != null) && (emailAddress.Address != null)))
            {
                ma = new MailAddress(emailAddress.Address, emailAddress.DisplayName);
            }

            return ma;
        }
    }
}
