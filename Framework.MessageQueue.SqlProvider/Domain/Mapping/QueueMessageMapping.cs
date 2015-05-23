namespace Framework.Domain.Mapping
{
    using System.Security;

    
    public class QueueMessageMapping : EntityMapping<QueueMessage>
    {
        public QueueMessageMapping()
        {
            this.Property(x => x.QueueName).IsRequired().HasMaxLength(80);
            this.Property(x => x.Body).IsRequired().HasMaxLength(256);
            this.Property(x => x.SentTimestamp).IsRequired();
            this.Property(x => x.ApproximateReceiveCount).IsRequired();
            this.Property(x => x.ApproximateFirstReceiveTimestamp).IsRequired();
            this.Property(x => x.LastAccessTimestamp).IsOptional();
            this.HasRequired(x => x.Queue).WithMany().HasForeignKey(x => x.QueueName).WillCascadeOnDelete(true);
            this.ToTable("QueueMessages");
        }
    }
}
