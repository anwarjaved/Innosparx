namespace Framework.Fakes
{
    using System.Linq;
    using System.Security.Principal;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Fake principal.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public class FakePrincipal : IPrincipal
    {
        private readonly IIdentity identity;
        private readonly string[] roles;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the FakePrincipal class.
        /// </summary>
        /// <param name="identity">
        /// The identity.
        /// </param>
        /// <param name="roles">
        /// The roles.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public FakePrincipal(IIdentity identity, string[] roles)
        {
            this.identity = identity;
            this.roles = roles;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the identity of the current principal.
        /// </summary>
        /// <value>
        /// The<see cref="T:System.Security.Principal.IIdentity"/>object associated with the
        /// current principal.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public IIdentity Identity
        {
            get
            {
                return this.identity;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Determines whether the current principal belongs to the specified role.
        /// </summary>
        /// <param name="role">
        /// The role.
        /// </param>
        /// <returns>
        /// true if the current principal is a member of the specified role; otherwise, false.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public bool IsInRole(string role)
        {
            return (this.roles != null) && this.roles.Contains<string>(role);
        }
    }
}
