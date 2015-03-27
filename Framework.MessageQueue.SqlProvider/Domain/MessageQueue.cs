namespace Framework.Domain
{
    using System;

    /// <summary>
    /// Class MessageQueue.
    /// </summary>
    public class MessageQueue : BaseEntity
    {
        public DateTime CreatedTimestamp { get; set; }

        public int Delay { get; set; }

        public int VisibilityTimeout { get; set; }

        public int MessageRetentionPeriod { get; set; }

        public string Name { get; set; }
    }
}
