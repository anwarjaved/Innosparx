namespace Framework.Services.Impl
{
    using System;
    using System.Linq;
    using System.Net.Mail;
    using System.Security;
    using System.Web.Configuration;

    using Framework.Ioc;
    using Framework.Services.Domain;

    [InjectBind(typeof(IEmailProvider), "SqlEmailProvider", LifetimeType.Singleton)]
    public class SqlEmailProvider : IEmailProvider
    {
        private readonly string nameOrConnectionString;

        public SqlEmailProvider()
        {
            var overriddenConnectionString = WebConfigurationManager.AppSettings["Framework.EmailServiceContext"];
            if (!string.IsNullOrWhiteSpace(overriddenConnectionString))
            {
                this.nameOrConnectionString = overriddenConnectionString;
            }

            if (string.IsNullOrWhiteSpace(this.nameOrConnectionString))
            {
                this.nameOrConnectionString = "AppContext";
            }

        }
   
        [SecuritySafeCritical]
        public string Save(EmailMessage message)
        {
            using (SqlEmailServiceContext configContext = new SqlEmailServiceContext(this.nameOrConnectionString))
            {
                EmailQueueItem item = new EmailQueueItem();
                item.ID = Guid.NewGuid().ToCombGuid();
                item.ErrorMessage = string.Empty;
                item.CreateDate = DateTime.UtcNow;
                item.Message = EmailMessage.Serialize(message);
                configContext.EmailQueueItems.Add(item);
                configContext.SaveChanges();
                return item.ID.ToStringValue();
            }
        }

        [SecuritySafeCritical]
        public EmailMessage Get(string id)
        {
            using (SqlEmailServiceContext configContext = new SqlEmailServiceContext(this.nameOrConnectionString))
            {
                Guid itemID = new Guid(id);
                EmailQueueItem item = configContext.EmailQueueItems.FirstOrDefault(x => x.ID == itemID && x.SendDate == null);

                if (item != null)
                {
                    EmailMessage message = EmailMessage.Deserialize(item.Message);
                    message.ErrorMessage = item.ErrorMessage;
                    message.SendDate = item.SendDate;
                    message.RetryAttempt = item.RetryAttempt;
                    return message;
                }
            }

            return null;
        }

        [SecuritySafeCritical]
        public void Update(string id, EmailMessage message)
        {
            using (SqlEmailServiceContext configContext = new SqlEmailServiceContext(this.nameOrConnectionString))
            {
                Guid itemID = new Guid(id);
                EmailQueueItem item = configContext.EmailQueueItems.FirstOrDefault(x => x.ID == itemID && x.SendDate == null);
                item.ErrorMessage = message.ErrorMessage;
                item.RetryAttempt = message.RetryAttempt;
                item.SendDate = message.SendDate;
                configContext.SaveChanges();
            }
        }

        [SecuritySafeCritical]
        public void RemoveAll(DateTime date)
        {
            using (SqlEmailServiceContext configContext = new SqlEmailServiceContext(this.nameOrConnectionString))
            {
                IQueryable<EmailQueueItem> items = configContext.EmailQueueItems.Where(x => x.CreateDate > date);

                configContext.EmailQueueItems.RemoveRange(items);

                configContext.SaveChanges();
            }
        }
    }
}
