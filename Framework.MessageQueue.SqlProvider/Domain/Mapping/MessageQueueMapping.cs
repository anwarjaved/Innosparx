namespace Framework.Domain.Mapping
{
    using System.Security;

    [SecurityCritical]
    public class MessageQueueMapping : BaseEntityMapping<MessageQueue>
    {
        public MessageQueueMapping()
        {
            this.HasKey(x => x.Name);
            this.Property(x => x.Name).HasMaxLength(80).IsRequired();
            this.Property(x => x.CreatedTimestamp).IsRequired();
            this.Property(x => x.Delay).IsRequired();
            this.Property(x => x.VisibilityTimeout).IsRequired();
            this.Property(x => x.MessageRetentionPeriod).IsRequired();
            this.ToTable("MessageQueue");
        }
    }
}
