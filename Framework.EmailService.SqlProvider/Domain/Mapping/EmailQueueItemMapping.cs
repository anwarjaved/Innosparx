using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Services.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using System.Security;

    [SecurityCritical]
    internal class EmailQueueItemMapping: EntityTypeConfiguration<EmailQueueItem>
    {
        public EmailQueueItemMapping()
        {
            this.Property(c => c.RowVersion).IsRowVersion();
            this.HasKey(x => x.ID);
            this.Property(x => x.Message).HasColumnType("ntext").IsRequired();
            this.Property(x => x.RetryAttempt).IsRequired();
            this.Property(x => x.CreateDate).IsRequired();
            this.Property(x => x.SendDate).IsOptional();
            this.Property(x => x.ErrorMessage).IsOptional().HasMaxLength(2000);


            ToTable("EmailQueue");
        }
    }
}