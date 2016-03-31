namespace Framework.IDMembership
{
    using Framework.Ioc;

    /// <summary>
    /// Default policy object
    /// </summary>
    [InjectBind(typeof(IAccountPolicy), LifetimeType.Singleton)]
    public class AccountPolicy : IAccountPolicy
    {
        public AccountPolicy()
        {
            this.IsPasswordResetEnabled = true;
            this.IsPasswordRetrievalEnabled = false;
            this.MaxInvalidPasswordAttempts = 50;
            this.MinRequiredNonAlphanumericCharacters = 0;
            this.PasswordAttemptWindow = 5;
            this.PasswordMinimumLength = 5;
            this.PasswordStrengthRegularExpression = null;
        }


        /// <summary>
        /// Gets number of invalid password or password-answer attempts allowed before the membership user is locked out
        /// </summary>
        public int MaxInvalidPasswordAttempts { get; set; }

        /// <summary>
        /// Gets whether the membership provider is configured to allow users to reset their passwords
        /// </summary>
        public bool IsPasswordResetEnabled { get; set; }

        /// <summary>
        /// Gets whether the membership provider is configured to allow users to retrieve their passwords
        /// </summary>
        public bool IsPasswordRetrievalEnabled { get; set; }

        /// <summary>
        /// Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </summary>
        public int PasswordAttemptWindow { get; set; }

        /// <summary>
        /// Get minimum length required for a password
        /// </summary>
        public int PasswordMinimumLength { get; set; }

        /// <summary>
        /// Gets minimum number of special characters that must be present in a valid password
        /// </summary>
        public int MinRequiredNonAlphanumericCharacters { get; set; }

        /// <summary>
        /// Gets the regular expression used to evaluate a password
        /// </summary>
        public string PasswordStrengthRegularExpression { get; set; }
    }
}
