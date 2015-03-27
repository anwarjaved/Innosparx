namespace Framework.Services
{
    using System.Net.Mail;

    using Framework.Ioc;
    using Framework.MessageQueue;
/*
    public static class EmailService
    {
        private static readonly QueueProcessor Processor = new QueueProcessor(EmailServiceConstants.EmailQueueComponent, ProcessMail);

        private static bool ProcessMail(MessageInfo messageInfo)
        {
            IEmailProvider provider = Container.Get<IEmailProvider>();

            MailMessage message = provider.Get(messageInfo.Body);

            if (message != null)
            {
                IEmailService emailService = Container.Get<IEmailService>();
                if (emailService.Send(message))
                {
                    provider.Remove(messageInfo.Body);
                    return true;
                }

                if (messageInfo.ApproximateReceiveCount >= 5)
                {
                    provider.Remove(messageInfo.Body);
                }
            }

            // clear emails automatically if already tried 5 times.
            return messageInfo.ApproximateReceiveCount >= 5;
        }

        public static void Send(MailMessage message)
        {
            IEmailProvider provider = Container.Get<IEmailProvider>();

            IQueue queue = Container.Get<IQueue>();
            queue.CreateQueue(EmailServiceConstants.EmailQueueComponent);

            string id = provider.Save(message);

            queue.SendMessage(EmailServiceConstants.EmailQueueComponent, id);

            Processor.Start();
        }
    }*/
}
