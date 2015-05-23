namespace Framework.Domain.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Threading.Tasks;

    
    internal class SiteSettingMapping : EntityTypeConfiguration<SiteSetting>
    {
        public SiteSettingMapping()
        {
            this.Property(c => c.RowVersion).IsRowVersion();
            this.HasKey(x => x.Name);
            this.Property(x => x.Name).IsRequired().HasMaxLength(250);
            this.Property(x => x.Value).IsOptional();
        }
    }
}
