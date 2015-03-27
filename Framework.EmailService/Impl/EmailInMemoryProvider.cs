namespace Framework.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;

    using Framework.Caching;
    using Framework.Ioc;

    [InjectBind(typeof(IEmailProvider), LifetimeType.Singleton)]
    [InjectBind(typeof(IEmailProvider), "EmailInMemory", LifetimeType.Singleton)]
    public class EmailInMemoryProvider : IEmailProvider
    {
        private readonly Dictionary<string, DateTime> emailIDs = new Dictionary<string, DateTime>();
        private readonly ICache cache;

        public EmailInMemoryProvider(ICache cache)
        {
            this.cache = cache;
        }

        public string Save(EmailMessage message)
        {
            string id = Guid.NewGuid().ToStringValue();
            this.cache.Set(id, message, DateTime.UtcNow.AddDays(7));
            emailIDs.Add(id, DateTime.UtcNow);
            return id;
        }

        public EmailMessage Get(string id)
        {
            return this.cache.Get<EmailMessage>(id);
        }

        public void Update(string id, EmailMessage message)
        {
            
        }

        public void RemoveAll(DateTime date)
        {
            var pairs = emailIDs.Where(x => x.Value< date).ToList();

            foreach (var keyValuePair in pairs)
            {
                if (cache.Exists(keyValuePair.Key))
                {
                    cache.Remove(keyValuePair.Key);
                }

                if (emailIDs.ContainsKey(keyValuePair.Key))
                {
                    emailIDs.Remove(keyValuePair.Key);
                }
            }
        }
    }
}
