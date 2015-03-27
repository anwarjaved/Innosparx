namespace Framework.Services.Impl
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Threading;

    using Framework.Configuration;
    using Framework.Ioc;
    using Framework.Logging;
    using Framework.MessageQueue;

    [InjectBind(typeof(IEmailService), LifetimeType.Singleton)]
    [InjectBind(typeof(IEmailService), "SmtpService", LifetimeType.Singleton)]
    public class SmtpService : IEmailService
    {
        private readonly IEmailProvider provider;

        private readonly IQueue queue;

        public SmtpService(IEmailProvider provider, IQueue queue)
        {
            this.provider = provider;
            this.queue = queue;
            queue.CreateQueue(EmailServiceConstants.EmailQueueComponent);
        }

        public bool LogEnabled { get; set; }

        public IEmailProvider Provider
        {
            get
            {
                return this.provider;
            }
        }

        public IQueue Queue
        {
            get
            {
                return this.queue;
            }
        }

        public bool ProcessEmail(string id, bool logEnabled = false)
        {
            EmailMessage message = provider.Get(id);
            if (message != null && message.SendDate == null)
            {
                if (string.IsNullOrWhiteSpace(ConfigManager.Mail.AuthenticationEmail)
                    || string.IsNullOrWhiteSpace(ConfigManager.Mail.AuthenticationPassword)
                    || string.IsNullOrWhiteSpace(ConfigManager.Mail.FromEmail)
                    || string.IsNullOrWhiteSpace(ConfigManager.Mail.SmtpServer))
                {
                    if (logEnabled)
                    {
                        Logger.Error("Can't Send Mail, Credential Not Set", EmailServiceConstants.EmailComponent);
                    }

                    return false;
                }

                SmtpClient client = new SmtpClient(ConfigManager.Mail.SmtpServer, ConfigManager.Mail.SmtpPort)
                {
                    Credentials =
                        new NetworkCredential(ConfigManager.Mail.AuthenticationEmail,
                            ConfigManager.Mail.AuthenticationPassword),
                    EnableSsl = ConfigManager.Mail.EnableSsl
                };


                if (logEnabled)
                {
                    Logger.Info(
                    "Sending an e-mail message to using the SMTP host {0}.".FormatString(ConfigManager.Mail.SmtpServer),
                    EmailServiceConstants.EmailComponent);
                }

                try
                {
                    client.Send(message.ToMailMessage());
                    if (logEnabled)
                    {
                        Logger.Info(
                            "Successfully Send an e-mail message to using the SMTP host {0}.".FormatString(
                                ConfigManager.Mail.SmtpServer),
                            EmailServiceConstants.EmailComponent);
                    }

                    message.SendDate = DateTime.UtcNow;
                    this.TryToUpdateStatus(id, message);
                    
                    return true;
                }
                catch (SmtpFailedRecipientsException ex)
                {
                    int retryCount = 0;
                    for (int i = 0; i < ex.InnerExceptions.Length;)
                    {
                        SmtpStatusCode status = ex.InnerExceptions[i].StatusCode;
                        var errorMessage = "Failed to deliver message to {0}".FormatString(
                            ex.InnerExceptions[i].FailedRecipient);
                        if (status == SmtpStatusCode.MailboxBusy || status == SmtpStatusCode.MailboxUnavailable)
                        {
                            if (logEnabled)
                            {
                                Logger.Warn(
                                    "Delivery failed - retrying in 5 seconds.",
                                    EmailServiceConstants.EmailComponent);
                            }

                            System.Threading.Thread.Sleep(5000);

                            try
                            {
                                client.Send(message.ToMailMessage());
                                i++;
                                retryCount = 0;
                                if (logEnabled)
                                {
                                    Logger.Info(
                                        "Successfully Send an e-mail message to using the SMTP host {0}.".FormatString(
                                            ConfigManager.Mail.SmtpServer),
                                        EmailServiceConstants.EmailComponent);
                                }

                                message.SendDate = DateTime.UtcNow;
                                this.TryToUpdateStatus(id, message);
                                return true;
                            }
                            catch
                            {
                                if (retryCount >= 3)
                                {
                                    retryCount = 0;
                                    i++;
                                    if (logEnabled)
                                    {
                                        Logger.Error(
                                            errorMessage,
                                            EmailServiceConstants.EmailComponent);
                                        message.RetryAttempt++;
                                        message.ErrorMessage += (errorMessage + Environment.NewLine);
                                        this.TryToUpdateStatus(id, message);
                                    }
                                }
                                else
                                {
                                    retryCount++;
                                }
                            }
                        }
                        else
                        {
                            if (logEnabled)
                            {
                                Logger.Error(
                                    errorMessage,
                                    EmailServiceConstants.EmailComponent);
                                message.RetryAttempt++;
                                message.ErrorMessage += (errorMessage + Environment.NewLine);
                                this.TryToUpdateStatus(id, message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    message.RetryAttempt++;
                    message.ErrorMessage = (ex.GetExceptionMessage() + Environment.NewLine);
                    this.TryToUpdateStatus(id, message);
                }

                return message.SendDate.HasValue || message.RetryAttempt > 5;
            }

            return true;

        }

        private void TryToUpdateStatus(string id, EmailMessage message)
        {
            try
            {
                this.provider.Update(id, message);
            }
            catch (Exception)
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                try
                {
                    this.provider.Update(id, message);
                }
                catch (Exception)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    try
                    {
                        this.provider.Update(id, message);
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                        try
                        {
                            this.provider.Update(id, message);
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(2));
                        }
                    }
                }
            }
        }

        public void Send(EmailMessage message, bool logEnabled = false)
        {
            var id = this.Provider.Save(message);

            queue.SendMessage(EmailServiceConstants.EmailQueueComponent, id);
        }
    }
}
