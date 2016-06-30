using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Mapping
{
    public class UserGroupMapping : AggregateEntityMapping<UserGroup>
    {
        public UserGroupMapping()
        {
            this.Property(x => x.Name).IsRequired().HasMaxLength(250);
            this.ToTable("UserGroups");
        }
    }
}
