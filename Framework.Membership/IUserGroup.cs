namespace Framework.Membership
{
    using System.Collections.Generic;

    public interface IUserGroup
    {
        /// <summary>
        /// To get  and set the Group Name
        /// </summary>
        string Name { get; set; }

       ICollection<IUserGroupRolePermission> Permissions { get; }
    }
}