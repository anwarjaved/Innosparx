namespace Framework
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Cryptography Helper Methods.
    /// </summary>
    /// <remarks>
    ///     LM ANWAR, 6/2/2013.
    /// </remarks>
    /// -------------------------------------------------------------------------------------------------
    public static class Cryptography
    {
        /// <summary>
        /// Generates the salt.
        /// </summary>
        /// <param name="len">The length.</param>
        /// <returns>Random Generated Salt.</returns>
        public static string GenerateSalt(int len = 16)
        {
            byte[] buf = new byte[len];
            (new RNGCryptoServiceProvider()).GetBytes(buf);
            return Convert.ToBase64String(buf);
        }

        /// <summary>
        /// Creates the hash.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="stringToHash">The string to hash.</param>
        /// <returns>
        /// String as Hash.
        /// </returns>
        public static string CreateHash(HashMode mode, string stringToHash)
        {
            var hashAlgorithm = CreateHashAlgorithm(mode);
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates the hash.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="value">The value.</param>
        /// <returns>An <see cref="byte"/> array.</returns>
        public static byte[] CreateHash(HashMode mode, byte[] value)
        {
            var hashAlgorithm = CreateHashAlgorithm(mode);
            return hashAlgorithm.ComputeHash(value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Creates the hash.
        /// </summary>
        /// <remarks>
        /// LM ANWAR, 6/2/2013.
        /// </remarks>
        /// <param name="mode">
        /// The mode.
        /// </param>
        /// <param name="stringToHash">
        /// The string to hash.
        /// </param>
        /// <param name="salt">
        /// The salt.
        /// </param>
        /// <returns>
        /// String as Hash.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static string CreateHash(HashMode mode, string stringToHash, string salt)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(stringToHash);
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] allBytes = new byte[saltBytes.Length + inputBytes.Length];

            Buffer.BlockCopy(saltBytes, 0, allBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(inputBytes, 0, allBytes, saltBytes.Length, inputBytes.Length);

            var hashAlgorithm = CreateHashAlgorithm(mode);
            byte[] data = hashAlgorithm.ComputeHash(allBytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Creates the hash.
        /// </summary>
        /// <remarks>
        /// LM ANWAR, 6/2/2013.
        /// </remarks>
        /// <param name="mode">
        /// The mode.
        /// </param>
        /// <param name="stringToHash">
        /// The string to hash.
        /// </param>
        /// <param name="salt">
        /// The salt.
        /// </param>
        /// <returns>
        /// String as Hash.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static string CreateHash(KeyedHashMode mode, string stringToHash, string salt)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(stringToHash);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            var hashAlgorithm = CreateKeyedHashAlgorithm(mode, saltBytes);
            byte[] data = hashAlgorithm.ComputeHash(inputBytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Creates the hash.
        /// </summary>
        /// <remarks>
        /// LM ANWAR, 6/2/2013.
        /// </remarks>
        /// <param name="mode">
        /// The mode.
        /// </param>
        /// <param name="inputBytes">
        /// The input in bytes.
        /// </param>
        /// <param name="saltBytes">
        /// The salt in bytes.
        /// </param>
        /// <returns>
        /// String as Hash.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static string CreateHash(KeyedHashMode mode, byte[] inputBytes, byte[] saltBytes)
        {
            var hashAlgorithm = CreateKeyedHashAlgorithm(mode, saltBytes);
            byte[] data = hashAlgorithm.ComputeHash(inputBytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates the hash.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="inputBytes">The input in bytes.</param>
        /// <param name="saltBytes">The salt in bytes.</param>
        /// <returns>String as Hash.</returns>
        /// <remarks>LM ANWAR, 6/2/2013.</remarks>
        public static string CreateHash(HashMode mode, byte[] inputBytes, byte[] saltBytes)
        {
            byte[] allBytes = new byte[saltBytes.Length + inputBytes.Length];

            Buffer.BlockCopy(saltBytes, 0, allBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(inputBytes, 0, allBytes, saltBytes.Length, inputBytes.Length);

            var hashAlgorithm = CreateHashAlgorithm(mode);
            byte[] data = hashAlgorithm.ComputeHash(allBytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                builder.Append(data[i].ToString("x2"));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Decrypts the specified cipher text.
        /// </summary>
        /// <param name="cipherText">The cipher text.</param>
        /// <param name="password">The password.</param>
        /// <returns>Decrypted text.</returns>
        public static string Decrypt(string cipherText, string password)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, new byte[] { 73, 118, 97, 110, 32, 77, 101, 100, 118, 101, 100, 101, 118, 8, 34, 60 });
            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(0x20), pdb.GetBytes(0x10));
            return Encoding.UTF8.GetString(decryptedData, 0, decryptedData.Length);
        }

        /// <summary>
        /// Decrypt the specified cipher data.
        /// </summary>
        /// <param name="cipherData">The cipher data.</param>
        /// <param name="password">The password.</param>
        /// <returns>Decrypted Bytes.</returns>
        public static byte[] Decrypt(byte[] cipherData, string password)
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, new byte[] { 73, 118, 97, 110, 32, 77, 101, 100, 118, 101, 100, 101, 118, 8, 34, 60 });
            return Decrypt(cipherData, pdb.GetBytes(0x20), pdb.GetBytes(0x10));
        }

        /// <summary>
        /// Decrypts the specified file in.
        /// </summary>
        /// <param name="fileIn">The file in.</param>
        /// <param name="fileOut">The file out.</param>
        /// <param name="password">The password.</param>
        public static void Decrypt(string fileIn, string fileOut, string password)
        {
            using (Rijndael aes = Rijndael.Create())
            {
                int bytesRead;
                FileStream inputStream = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
                FileStream outputStream = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, salt: new byte[] { 73, 118, 97, 110, 32, 77, 101, 100, 118, 101, 100, 101, 118, 8, 34, 60 });
                aes.Key = pdb.GetBytes(32);
                aes.IV = pdb.GetBytes(16);
                CryptoStream cs = new CryptoStream(outputStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
                byte[] buffer = new byte[FrameworkConstants.BufferSize];
                do
                {
                    bytesRead = inputStream.Read(buffer, 0, FrameworkConstants.BufferSize);
                    cs.Write(buffer, 0, bytesRead);
                }
                while (bytesRead != 0);
                cs.Close();
                inputStream.Close();
            }
        }

        /// <summary>
        /// Decrypts the specified cipher data.
        /// </summary>
        /// <param name="cipherData">The cipher data.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="iv">The IV to use.</param>
        /// <returns>Decrypted Bytes.</returns>
        public static byte[] Decrypt(byte[] cipherData, byte[] key, byte[] iv)
        {
            using (Rijndael rijndael = Rijndael.Create())
            {
                MemoryStream ms = new MemoryStream();
                rijndael.Key = key;
                rijndael.IV = iv;
                CryptoStream cs = new CryptoStream(ms, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(cipherData, 0, cipherData.Length);
                cs.Close();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Encrypts the specified clear data.
        /// </summary>
        /// <param name="clearData">The clear data.</param>
        /// <param name="password">The password.</param>
        /// <returns>Encrypted Bytes.</returns>
        public static byte[] Encrypt(byte[] clearData, string password)
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, new byte[] { 73, 118, 97, 110, 32, 77, 101, 100, 118, 101, 100, 101, 118, 8, 34, 60 });
            return Encrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));
        }

        /// <summary>
        /// Encrypts the specified clear text.
        /// </summary>
        /// <param name="clearText">The clear text.</param>
        /// <param name="password">The password.</param>
        /// <returns>Encrypted Text.</returns>
        public static string Encrypt(string clearText, string password)
        {
            byte[] clearBytes = Encoding.UTF8.GetBytes(clearText);
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, new byte[] { 73, 118, 97, 110, 32, 77, 101, 100, 118, 101, 100, 101, 118, 8, 34, 60 });
            return Convert.ToBase64String(Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16)));
        }

        /// <summary>
        /// Encrypts the specified clear data.
        /// </summary>
        /// <param name="clearData">The clear data.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="iv">The IV to use.</param>
        /// <returns>Encrypted Bytes.</returns>
        public static byte[] Encrypt(byte[] clearData, byte[] key, byte[] iv)
        {
            using (Rijndael rijndael = Rijndael.Create())
            {
                MemoryStream ms = new MemoryStream();
                rijndael.Key = key;
                rijndael.IV = iv;
                CryptoStream cs = new CryptoStream(ms, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(clearData, 0, clearData.Length);
                cs.Close();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Encrypts the specified file in.
        /// </summary>
        /// <param name="fileIn">The file in.</param>
        /// <param name="fileOut">The file out.</param>
        /// <param name="password">The password.</param>
        public static void Encrypt(string fileIn, string fileOut, string password)
        {
            using (Rijndael rijndael = Rijndael.Create())
            {
                int bytesRead;
                FileStream inputStream = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
                FileStream outputStream = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(password, new byte[] { 73, 118, 97, 110, 32, 77, 101, 100, 118, 101, 100, 101, 118, 8, 34, 60 });
                rijndael.Key = pdb.GetBytes(32);
                rijndael.IV = pdb.GetBytes(16);
                CryptoStream cs = new CryptoStream(outputStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
                byte[] buffer = new byte[FrameworkConstants.BufferSize];
                do
                {
                    bytesRead = inputStream.Read(buffer, 0, FrameworkConstants.BufferSize);
                    cs.Write(buffer, 0, bytesRead);
                }
                while (bytesRead != 0);
                cs.Close();
                inputStream.Close();
            }
        }

        /// <summary>
        /// Verifies the hash.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="input">The input to check.</param>
        /// <param name="hash">The hash to compare.</param>
        /// <returns>Compare result of Hash.</returns>
        public static bool VerifyHash(HashMode mode, string input, string hash)
        {
            string hashOfInput = CreateHash(mode, input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }

        /// <summary>
        /// Verifies the hash.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="input">The input to check.</param>
        /// <param name="salt">The salt.</param>
        /// <param name="hash">The hash to compare.</param>
        /// <returns>Compare result of Hash.</returns>
        public static bool VerifyHash(HashMode mode, string input, string salt, string hash)
        {
            string hashOfInput = CreateHash(mode, input, salt);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }

        private static HashAlgorithm CreateKeyedHashAlgorithm(KeyedHashMode mode, byte[] key)
        {
            switch (mode)
            {
                case KeyedHashMode.HMACMD5:
                    return new HMACMD5(key);

                case KeyedHashMode.HMACSHA1:
                    return new HMACSHA1(key);

                case KeyedHashMode.MACTripleDES:
                    return new MACTripleDES(key);
                default:
                    goto case KeyedHashMode.HMACMD5;
            }
        }

        private static HashAlgorithm CreateHashAlgorithm(HashMode mode)
        {
            switch (mode)
            {
                case HashMode.MD5:
                    return new MD5CryptoServiceProvider();
                case HashMode.SHA256:
                    return new SHA256Managed();
                default:
                    goto case HashMode.SHA256;
            }
        }
    }
}
