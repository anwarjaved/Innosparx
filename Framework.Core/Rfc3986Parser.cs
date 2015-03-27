namespace Framework
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///      RFC 3986 compliant URI encoding/decoding.
    /// </summary>
    ///-------------------------------------------------------------------------------------------------
    internal static class Rfc3986Parser
    {
        /// <summary>
        /// RFC 3986 percent encoding escape sequence
        /// </summary>
        private static readonly Regex Rfc3986EscapeSequence = new Regex("%([0-9A-Fa-f]{2})", RegexOptions.Compiled);

        /// <summary>
        /// Perform RFC 3986 Percent-encoding on a string.
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>The RFC 3986 Percent-encoded string</returns>
        public static string Encode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return Encoding.ASCII.GetString(EncodeToBytes(input, Encoding.UTF8));
        }

        /// <summary>
        /// Perform RFC 3986 Percent-encoding on a string.
        /// </summary>
        /// <param name="input">The input string</param>
        /// <returns>The RFC 3986 Percent-encoded string</returns>
        public static byte[] EncodeToBytes(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new byte[0];

            return EncodeToBytes(input, Encoding.UTF8);
        }

        /// <summary>
        /// Perform RFC 3986 Percent-decoding on a string.
        /// </summary>
        /// <param name="input">The input RFC 3986 Percent-encoded string</param>
        /// <returns>The decoded string</returns>
        public static string Decode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return Rfc3986EscapeSequence.Replace(
                input,
                match =>
                    {
                        if (match.Success)
                        {
                            Group hexgrp = match.Groups[1];

                            return string.Format(CultureInfo.InvariantCulture, "{0}", (char)int.Parse(hexgrp.Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture));
                        }

                        throw new FormatException("Could not RFC 3986 decode string");
                    });
        }

        private static byte[] EncodeToBytes(string input, Encoding enc)
        {
            if (string.IsNullOrEmpty(input))
                return new byte[0];

            byte[] inbytes = enc.GetBytes(input);

            // Count unsafe characters
            int unsafeChars = inbytes.Select(b => (char)b).Count(NeedsEscaping);

            // Check if we need to do any encoding
            if (unsafeChars == 0)
                return inbytes;

            byte[] outbytes = new byte[inbytes.Length + (unsafeChars * 2)];
            int pos = 0;

            foreach (byte b in inbytes)
            {
                if (NeedsEscaping((char)b))
                {
                    outbytes[pos++] = (byte)'%';
                    outbytes[pos++] = (byte)IntToHex((b >> 4) & 0xf);
                    outbytes[pos++] = (byte)IntToHex(b & 0x0f);
                }
                else
                {
                    outbytes[pos++] = b;
                }
            }

            return outbytes;
        }

        private static bool NeedsEscaping(char c)
        {
            return !((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9')
                     || c == '-' || c == '_' || c == '.' || c == '~');
        }

        private static char IntToHex(int n)
        {
            if (n < 0 || n >= 16)
                throw new ArgumentOutOfRangeException("n");

            if (n <= 9)
            {
                return (char)(n + '0');
            }

            return (char)(n - 10 + 'A');
        }
    }
}
