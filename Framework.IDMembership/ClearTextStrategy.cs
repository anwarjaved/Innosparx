namespace Framework.IDMembership
{
    /// <summary>
    /// Strategy that do nothing with the passwords.
    /// </summary>
    public class ClearTextStrategy : IPasswordStrategy
    {
        /// <summary>
        /// Encrypt a password
        /// </summary>
        /// <param name="account">Account information used to encrypt password</param>
        /// <returns>
        /// encrypted password.
        /// </returns>
        public string Encrypt(AccountPasswordInfo account)
        {
            return account.Password;
        }

        /// <summary>
        /// Decrypt a password
        /// </summary>
        /// <param name="password">Encrpted password</param>
        /// <param name="passwordSalt">The password salt.</param>
        /// <returns>
        /// Decrypted password if decryption is possible; otherwise null.
        /// </returns>
        public string Decrypt(string password, string passwordSalt)
        {
            return password;
        }

        /// <summary>
        /// Generate a new password
        /// </summary>
        /// <param name="policy">Policy that should be used when generating a new password.</param>
        /// <returns>A password which is not encrypted.</returns>
        public string GeneratePassword(IAccountPolicy policy)
        {
            return policy.GeneratePassword();
        }

        /// <summary>
        /// Compare if the specified password matches the encrypted password
        /// </summary>
        /// <param name="account">Stored acount informagtion.</param>
        /// <param name="clearTextPassword">Password specified by user.</param>
        /// <returns>
        /// true if passwords match; otherwise null
        /// </returns>
        public bool Compare(AccountPasswordInfo account, string clearTextPassword)
        {
            return account.Password.Equals(clearTextPassword);
        }

        /// <summary>
        /// Gets if passwords can be decrypted.
        /// </summary>
        public bool IsPasswordsDecryptable
        {
            get { return true; }
        }

        /// <summary>
        /// Checks if the specified password is valid
        /// </summary>
        /// <param name="password">Password being checked</param>
        /// <param name="accountPolicy">Policy used to validate password.</param>
        /// <returns></returns>
        public bool IsValid(string password, IAccountPolicy accountPolicy)
        {
            return accountPolicy.IsPasswordValid(password);
        }
    }
}
