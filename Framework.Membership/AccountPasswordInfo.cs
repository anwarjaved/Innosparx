namespace Framework.Membership
{
    /// <summary>
    /// Information used by the password strategies.
    /// </summary>
    public class AccountPasswordInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountPasswordInfo"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        public AccountPasswordInfo(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }

        public AccountPasswordInfo(string email, string password, string passwordSalt)
        {
            this.Email = email;
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
        /// Gets email for the accoount
        /// </summary>
        public string Email { get; private set; }
    }
}
