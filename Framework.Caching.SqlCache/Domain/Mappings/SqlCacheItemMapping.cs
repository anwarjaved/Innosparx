using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Caching.SqlCache.Domain.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using System.Security;

    using Framework.Domain;

    [SecurityCritical]
    internal class SqlCacheItemMapping : EntityTypeConfiguration<SqlCacheItem>
    {
        public SqlCacheItemMapping()
        {
            this.Property(c => c.RowVersion).IsRowVersion();
            this.HasKey(x => x.Name);
            this.Property(x => x.Name).IsRequired().HasMaxLength(250);
            this.Property(x => x.Value).HasColumnType("ntext").IsOptional();
            this.ToTable("SqlCache");
        }
    }
}