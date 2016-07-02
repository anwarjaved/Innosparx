using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain
{
    using Framework.IDMembership;

    public class UserGroup : AggregateEntity, IUserGroup
    {
        /// <summary>
        /// To get  and set the Group Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// To get  and set the User ID
        /// </summary>
        //public Guid UserID { get; set; }

        private EntityCollection<UserGroupRolePermission> permissions;
        public virtual EntityCollection<UserGroupRolePermission> Permissions
        {
            get { return this.permissions ?? (this.permissions = this.CreateCollection<UserGroupRolePermission>()); }
        }

        ICollection<IUserGroupRolePermission> IUserGroup.Permissions
        {
            get
            {
                return (ICollection<IUserGroupRolePermission>)this.Permissions;
            }
        }
    }
}
