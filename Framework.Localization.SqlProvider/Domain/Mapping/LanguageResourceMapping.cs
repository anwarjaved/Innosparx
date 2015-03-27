using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using System.Security;

    [SecurityCritical]
    internal class LanguageResourceMapping: EntityTypeConfiguration<LanguageResource>
    {
        public LanguageResourceMapping()
        {
            this.Property(c => c.RowVersion).IsRowVersion();
            this.HasKey(x => new { x.Key, x.Code});
            this.Property(x => x.Key).IsRequired().HasMaxLength(250);
            this.Property(x => x.Category).IsOptional().HasMaxLength(250);
            this.Property(x => x.Code).IsRequired();
            this.Property(x => x.Text).IsRequired().HasMaxLength(1000);
            this.Property(x => x.TooltipText).IsOptional().HasMaxLength(2000);
            this.Property(x => x.CanShowTooltip).IsRequired();
            ToTable("LanguageResources");
        }
    }
}
