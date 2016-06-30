using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.Mapping
{
    public class UserGroupRolePermissionMapping : BaseEntityMapping<UserGroupRolePermission>
    {
        public UserGroupRolePermissionMapping()
        {
            this.HasKey(x => new { x.RoleID, x.UserGroupID });
            this.HasRequired(x => x.Group).WithMany(y => y.Permissions).HasForeignKey(x => x.UserGroupID);
            this.HasRequired(x => x.Role).WithMany(y => y.Permissions).HasForeignKey(x => x.RoleID);
            this.Property(x => x.Permissions).IsOptional();
            this.ToTable("UserGroupRolePermissions");
        }
    }
}
