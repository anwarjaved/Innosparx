namespace Framework.IDMembership
{
    using System;
    using System.Linq;

    /// <summary>
    /// Extension methods for password policies.
    /// </summary>
    public static class PasswordPolicyExtensions
    {
        private const string AllowedChars = "abcdefghjkmnopqrstuvxtzABCDEFGHJKLMNPQRSTUVXYZ23456789";
        private const string AllowedAlphas = "@!?&%/\\";

        private static readonly Random Random = new Random();

        /// <summary>
        /// Determines whether the password is valid by going through all defined policies.
        /// </summary>
        /// <param name="accountPolicy">The password policy.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        ///   <c>true</c> if the password is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPasswordValid(this IAccountPolicy accountPolicy, string password)
        {
            var alphaCount = password.Count(ch => !char.IsLetterOrDigit(ch));
            if (alphaCount < accountPolicy.MinRequiredNonAlphanumericCharacters) return false;
            return password.Length >= accountPolicy.PasswordMinimumLength;
        }

        /// <summary>
        /// Generate a new password
        /// </summary>
        /// <param name="policy">Policy that should be used when generating a new password.</param>
        /// <returns>A password which is not encrypted.</returns>
        /// <remarks>Uses characters which can't be mixed up along with <![CDATA["@!?&%/\"]]> if non alphas are required</remarks>
        public static string GeneratePassword(this IAccountPolicy policy)
        {
            var length = Random.Next(policy.PasswordMinimumLength, policy.PasswordMinimumLength + 3);
            var password = "";

            var allowedCharacters = AllowedChars;
            if (policy.MinRequiredNonAlphanumericCharacters > 0) allowedCharacters += AllowedAlphas;

            var nonAlphaLeft = policy.MinRequiredNonAlphanumericCharacters;
            for (var i = 0; i < length; i++)
            {
                var ch = allowedCharacters[Random.Next(0, allowedCharacters.Length)];
                if (AllowedAlphas.IndexOf(ch) != -1) nonAlphaLeft--;

                if (length - i <= nonAlphaLeft) ch = AllowedAlphas[Random.Next(0, AllowedAlphas.Length)];

                password += ch;
            }

            return password;
        }
    }
}
