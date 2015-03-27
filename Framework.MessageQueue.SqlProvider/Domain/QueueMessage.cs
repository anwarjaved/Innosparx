namespace Framework.Domain
{
    using System;

    public class QueueMessage : Entity
    {
        public string QueueName { get; set; }

        public string Body { get; set; }

        public DateTime SentTimestamp { get; set; }
        public int ApproximateReceiveCount { get; set; }

        public DateTime ApproximateFirstReceiveTimestamp { get; set; }

        public DateTime? LastAccessTimestamp { get; set; }

        public virtual MessageQueue Queue { get; set; }


    }
}
