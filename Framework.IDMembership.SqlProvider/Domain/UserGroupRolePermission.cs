using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain
{
    using Framework.Domain.Mapping;
    using Framework.IDMembership;
    public class UserGroupRolePermission : BaseEntity, IUserGroupRolePermission
    {
        public Guid UserGroupID { get; set; }

        public Guid RoleID { get; set; }

        public Role Role { get; set; }


        public string Permissions { get; set; }

        public UserGroup Group { get; set; }

        IRole IUserGroupRolePermission.Role
        {
            get
            {
                return this.Role;
            }
        }

        IUserGroup IUserGroupRolePermission.Group
        {
            get
            {
                return this.Group;
            }
        }
    }
}
