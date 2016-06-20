namespace Framework.Domain
{
    using Framework.Membership;

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

        public int? Permissions { get; set; }
    }
}
