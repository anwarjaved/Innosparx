using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Membership
{
    public interface IUserGroupRolePermission
    {
        Guid UserGroupID { get; set; }

        Guid RoleID { get; set; }

        IRole Role { get; }

        IUserGroup Group { get; }

        long? Permissions { get; set; }
    }
}
