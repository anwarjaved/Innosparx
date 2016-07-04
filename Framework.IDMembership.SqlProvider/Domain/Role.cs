namespace Framework.Domain
{
    using System.Collections.Generic;

    using Framework.IDMembership;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Role.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    public class Role : AggregateEntity, IRole
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        ///
        /// <value>
        ///     The name.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        ///
        /// <value>
        ///     The description.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public string Description { get; set; }
        private EntityCollection<User> users;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the users.
        /// </summary>
        ///
        /// <value>
        ///     The users.
        /// </value>
        ///-------------------------------------------------------------------------------------------------
        public virtual EntityCollection<User> Users
        {
            get { return this.users ?? (this.users = this.CreateCollection<User>()); }
        }
        private EntityCollection<UserGroupRolePermission> permissions;
        public virtual EntityCollection<UserGroupRolePermission> Permissions
        {
            get { return this.permissions ?? (this.permissions = this.CreateCollection<UserGroupRolePermission>()); }
        }
        IEnumerable<IUserGroupRolePermission> IRole.Permissions
        {
            get
            {
                List<IUserGroupRolePermission> list = new List<IUserGroupRolePermission>();
                foreach (var userGroupRolePermission in this.Permissions)
                {
                    list.Add(userGroupRolePermission);
                }

                return list;
            }
        }
    }
}
