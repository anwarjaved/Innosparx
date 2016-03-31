namespace Framework.IDMembership
{
    /// <summary>
    /// Information used by the password strategies.
    /// </summary>
    public class AccountPasswordInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountPasswordInfo"/> class.
        /// </summary>
        /// <param name="uniqueID">The uniqueID.</param>
        /// <param name="password">The password.</param>
        public AccountPasswordInfo(string uniqueID, string password)
        {
            this.UniqueID = uniqueID;
            this.Password = password;
        }

        public AccountPasswordInfo(string uniqueID, string password, string passwordSalt)
        {
            this.UniqueID = uniqueID;
            this.Password = password;
            this.PasswordSalt = passwordSalt;
        }

        /// <summary>
        /// Gest or sets the salt which was used when hashing the password.
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// Gets the password
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Gets uniqueID for the account
        /// </summary>
        public string UniqueID { get; private set; }
    }
}
