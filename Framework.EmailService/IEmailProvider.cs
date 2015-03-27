namespace Framework.Services
{
    using System;
    using System.Net.Mail;

    /// <summary>
    /// Interface IEmailPersister
    /// </summary>
    public interface IEmailProvider
    {
        string Save(EmailMessage message);

        EmailMessage Get(string id);

        void Update(string id, EmailMessage message);

        void RemoveAll(DateTime date);
    }
}
