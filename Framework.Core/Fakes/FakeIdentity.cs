namespace Framework.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Fake identity.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public class FakeIdentity : IIdentity
    {
        private readonly string name;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the FakeIdentity class.
        /// </summary>
        /// <param name="userName">
        /// Name of the user.
        /// </param>
        /// -------------------------------------------------------------------------------------------------
        public FakeIdentity(string userName)
        {
            this.name = userName;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the type of authentication used.
        /// </summary>
        /// <value>
        /// The type of authentication used to identify the user.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public string AuthenticationType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets a value indicating whether the user has been authenticated.
        /// </summary>
        /// <value>
        /// true if the user was authenticated; otherwise, false.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(this.name);
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the name of the current user.
        /// </summary>
        /// <value>
        /// The name of the user on whose behalf the code is running.
        /// </value>
        /// -------------------------------------------------------------------------------------------------
        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}