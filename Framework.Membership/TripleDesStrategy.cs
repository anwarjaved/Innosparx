namespace Framework.Membership
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Encrypts the password by using triple des.
    /// </summary>
    public class TripleDesStrategy : IPasswordStrategy
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
                account.PasswordSalt = Cryptography.GenerateSalt();
            }

            return EncryptString(account.Password, account.PasswordSalt);
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
            return DecryptString(password, passwordSalt);
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
            var clear = DecryptString(account.Password, account.PasswordSalt);
            return clearTextPassword == clear;
        }

        /// <summary>
        /// Gets if passwords can be decrypted.
        /// </summary>
        public bool IsPasswordsDecryptable
        {
            get
            {
                return true;
            }
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

        /// <summary>
        /// Encrypts a string.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="passphrase">The passphrase.</param>
        /// <returns>Encrypted string</returns>
        public static string EncryptString(string password, string passphrase)
        {
            byte[] results;
            var encoding = Encoding.UTF8;

            var hashProvider = new MD5CryptoServiceProvider();
            var key = hashProvider.ComputeHash(encoding.GetBytes(passphrase));
            var cryptoServiceProvider = new TripleDESCryptoServiceProvider
                { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };

            var dataToEncrypt = encoding.GetBytes(password);
            try
            {
                var encryptor = cryptoServiceProvider.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                cryptoServiceProvider.Clear();
                hashProvider.Clear();
            }

            return Convert.ToBase64String(results);
        }

        /// <summary>
        /// Decrypts a string.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="passphrase">The passphrase.</param>
        /// <returns>Decrypted string</returns>
        public static string DecryptString(string password, string passphrase)
        {
            byte[] results;
            var encoding = Encoding.UTF8;
            var hashProvider = new MD5CryptoServiceProvider();
            var key = hashProvider.ComputeHash(encoding.GetBytes(passphrase));
            var cryptoServiceProvider = new TripleDESCryptoServiceProvider
                { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };

            var dataToDecrypt = Convert.FromBase64String(password);
            try
            {
                var decryptor = cryptoServiceProvider.CreateDecryptor();
                results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            finally
            {
                cryptoServiceProvider.Clear();
                hashProvider.Clear();
            }
            return encoding.GetString(results);
        }
    }
}
