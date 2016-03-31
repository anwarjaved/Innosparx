namespace Framework.IDMembership
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using Framework.Ioc;

    /// <summary>
    /// Hash a password using a salt.
    /// </summary>
    [InjectBind(typeof(IPasswordStrategy), LifetimeType.Singleton)]
    public class HashPasswordStrategy : IPasswordStrategy
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
            if (account.PasswordSalt.IsEmpty())
            {
                account.PasswordSalt = Cryptography.GenerateSalt(32);
            }

            var saltAndPwd = String.Concat(account.Password, account.PasswordSalt);
            var bytes = Encoding.UTF8.GetBytes(saltAndPwd);

            string computedHash;

            if (account.PasswordSalt.Length == 24)
            {
                var sha1 = SHA1.Create();
                computedHash = Convert.ToBase64String(sha1.ComputeHash(bytes));
            }
            else
            {
                var sha1256 = SHA256.Create();
                computedHash = Convert.ToBase64String(sha1256.ComputeHash(bytes));
            }

            return computedHash;
        }


        /// <summary>
        /// Decrypt a password
        /// </summary>
        /// <param name="password">Encrypted password</param>
        /// <param name="passwordSalt">The password salt.</param>
        /// <returns>
        /// Decrypted password if decryption is possible; otherwise null.
        /// </returns>
        public string Decrypt(string password, string passwordSalt)
        {
            throw new InvalidOperationException("Password decryption is not allowed/possible.");
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
            var clearTextInfo = new AccountPasswordInfo(account.UniqueID, clearTextPassword) { PasswordSalt = account.PasswordSalt };
            var password = this.Encrypt(clearTextInfo);
            return account.Password == password;
        }

        /// <summary>
        /// Gets if passwords can be decrypted.
        /// </summary>
        public bool IsPasswordsDecryptable
        {
            get { return false; }
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
