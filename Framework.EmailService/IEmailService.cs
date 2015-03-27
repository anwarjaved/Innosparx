namespace Framework.Services
{
    using System.Net.Mail;

    using Framework.MessageQueue;

    public interface IEmailService
    {
        void Send(EmailMessage message, bool logEnabled = false);

        IEmailProvider Provider { get; }

        IQueue Queue { get; }

        bool ProcessEmail(string id, bool logEnabled = false);
    }
}
