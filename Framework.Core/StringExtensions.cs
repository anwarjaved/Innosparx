namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Utility Extension Function for <see cref="string"/>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class StringExtensions
    {
        private static readonly Regex FormatRegex = new Regex(
            @"{\d+}",
            RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        private static readonly Regex EmailRegex =
            new Regex(
                @"^(?:[a-zA-Z0-9_'^&/+-])+(?:\.(?:[a-zA-Z0-9_'^&/+-])+)*@(?:(?:\[?(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\.){3}(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\]?)|(?:[a-zA-Z0-9-]+\.)+(?:[a-zA-Z]){2,}\.?)$",
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex MessageTokenRegex = new Regex(
            @"(([\[]{2})?(\$([\w.#\+-:,;_ ]+)\$(?(2)[\]]{2})))",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        private static readonly Regex HtmlRegex = new Regex(
            @"</?(?<TagName>[a-z][a-z0-9]*)[^<>]*>|<!--.*?-->",
            RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        private static readonly Regex LinkifyRegex =
            new Regex(
                @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])",
                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// A string extension method that convert given value to link.
        /// </summary>
        /// <param name="value">A<see cref="String"></see>containing zero or more format items.</param>
        /// <returns>Converted String.</returns>
        public static string Linkify(this string value)
        {
            return LinkifyRegex.Replace(value, "<a href=\"$1\" title=\"Click to open in a new window or tab\" target=\"&#95;blank\">$1</a>").Replace("href=\"www", "href=\"http://www");
        }

        /// <summary>
        /// A string extension method that strip HTML.
        /// </summary>
        /// <param name="value">A<see cref="String"></see>containing zero or more format items.</param>
        /// <param name="skipTags">A variable-length parameters list containing skip tags.</param>
        /// <returns>Stripped Html.</returns>
        public static string StripHtml(this string value, params string[] skipTags)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if (skipTags != null && skipTags.Length > 0)
            {
                List<string> tags = skipTags.Select(x => x.ToLowerInvariant()).ToList();
                return HtmlRegex.Replace(
                    value,
                    delegate(Match m)
                    {
                        string tagName = m.Groups["TagName"].Value;

                        if (!string.IsNullOrWhiteSpace(tagName))
                        {
                            if (tags.Contains(tagName))
                            {
                                return m.Value;
                            }
                        }

                        return string.Empty;
                    });
            }

            return HtmlRegex.Replace(value, string.Empty);
        }

        /// <summary>
        /// Indicates whether the specified.<see cref="T:System.String" />Object is null or an <see cref="System.String.Empty" />.
        /// </summary>
        /// <returns>
        /// True if the value parameter is null or an empty string (""); otherwise, false.
        /// </returns>
        /// <param name="value">A. <see cref="System.String" /> Reference. </param>
        public static bool IsEmpty(this string value)
        {
            return value == null || string.IsNullOrEmpty(value.Trim());
        }

        /// <summary>
        /// Replaces the format item in a specified <see cref="T:System.String"></see> with the text equivalent of the value of a corresponding <see cref="T:System.Object"></see> instance in a specified array.
        /// </summary>
        /// <param name="value">A <see cref="String"></see> containing zero or more format items.</param>
        /// <param name="args">An <see cref="Object"></see> array containing zero or more objects to format.</param>
        /// <returns>
        /// A copy of format in which the format items have been replaced by the <see cref="T:System.String"></see> equivalent of the corresponding instances of <see cref="T:System.Object"></see> in args.
        /// </returns>
        public static string FormatString(this string value, params object[] args)
        {
            if (!value.IsEmpty())
            {
                if (args == null || args.Length == 0)
                {
                    return value;
                }

                try
                {
                    return FormatRegex.Replace(
                        value,
                        delegate(Match m)
                        {
                            string indexValue = m.Value.Substring(1, m.Value.Length - 2);
                            object returnValue = args[int.Parse(indexValue, CultureInfo.InvariantCulture)];
                            return returnValue != null ? returnValue.ToString() : string.Empty;
                        });
                }
                catch (Exception exception)
                {
                    throw new FormatException("Invalid Format", exception);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Return Whether specified strings are equal with ignore case.
        /// </summary>
        /// <param name="value">The string to compare.</param>
        /// <param name="compareValue">The second string to compare.</param>
        /// <returns>
        /// True if string are equal else false.
        /// </returns>
        public static bool EqualsIgnoreCase(this string value, string compareValue)
        {
            return string.Compare(value, compareValue, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Return Whether specified strings are equal without ignore case.
        /// </summary>
        /// <param name="value">The string to compare.</param>
        /// <param name="compareValue">The second string to compare.</param>
        /// <returns>
        /// True if string are equal else false.
        /// </returns>
        public static bool Equals(this string value, string compareValue)
        {
            return string.Compare(value, compareValue, StringComparison.Ordinal) == 0;
        }

        /// <summary>
        /// Replaces a character in string with specified character.
        /// </summary>
        /// <param name="value">The string in which character is replaced.</param>
        /// <param name="find">The char to search for.</param>
        /// <param name="rep">The char to replace.</param>
        /// <returns>String with replaced character.</returns>
        public static string Replace(this string value, char find, char rep)
        {
            return !IsEmpty(value) ? (value.IndexOf(find) < 0 ? value : value.Replace(find, rep)) : string.Empty;
        }

        /// <summary>
        /// This returns a new string with all surrounding whitespace
        /// removed and internal whitespace normalized to a single space.
        /// </summary>
        /// <param name="text">String to be normalized.</param>
        /// <returns>Normalized string or empty string.</returns>
        public static string Normalize(this string text)
        {
            if (text == null)
            {
                return string.Empty;
            }

            char[] c = text.ToCharArray();
            char[] n = new char[c.Length];
            bool white = true;
            int pos = 0;

            foreach (char t in c)
            {
                if (" \t\n\r".IndexOf(t) != -1)
                {
                    if (!white)
                    {
                        n[pos++] = ' ';
                        white = true;
                    }
                }
                else
                {
                    n[pos++] = t;
                    white = false;
                }
            }

            if (white && pos > 0)
            {
                pos--;
            }

            return new string(n, 0, pos);
        }

        /// <summary>
        /// Replaces a given character with another character in a string. 
        /// The replacement is case insensitive.
        /// </summary>
        /// <param name="value">A. <see cref="System.String" /> Reference. </param>
        /// <param name="charToReplace">The character to replace.</param>
        /// <param name="replacement">The character by which to be replaced.</param>
        /// <returns>Copy of string with the characters replaced.</returns>
        public static string CaseInsenstiveReplace(this string value, char charToReplace, char replacement)
        {
            Regex regEx = new Regex(charToReplace.ToString(CultureInfo.CurrentCulture), RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regEx.Replace(value, replacement.ToString(CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Replaces a given string with another string in a given string. 
        /// The replacement is case insensitive.
        /// </summary>
        /// <param name="value">A. <see cref="System.String" /> Reference. </param>
        /// <param name="stringToReplace">The string to replace.</param>
        /// <param name="replacement">The string by which to be replaced.</param>
        /// <returns>Copy of string with the string replaced.</returns>
        public static string CaseInsenstiveReplace(this string value, string stringToReplace, string replacement)
        {
            Regex regEx = new Regex(stringToReplace, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return regEx.Replace(value, replacement);
        }

        /// <summary>
        /// Replaces the first occurrence of a string with another string in a given string
        /// The replacement is case insensitive.
        /// </summary>
        /// <param name="value">A. <see cref="System.String" /> Reference. </param>
        /// <param name="stringToReplace">The string to replace.</param>
        /// <param name="replacement">The string by which to be replaced.</param>
        /// <returns>Copy of string with the string replaced.</returns>
        public static string ReplaceFirst(this string value, string stringToReplace, string replacement)
        {
            Regex regEx = new Regex(stringToReplace, RegexOptions.Multiline);
            return regEx.Replace(value, replacement, 1);
        }

        /// <summary>
        /// Replaces the first occurrence of a character with another character in a given string.
        /// The replacement is case insensitive.
        /// </summary>
        /// <param name="value">A. <see cref="System.String" /> Reference. </param>
        /// <param name="charToReplace">The character to replace.</param>
        /// <param name="replacement">The character by which to replace.</param>
        /// <returns>Copy of string with the character replaced.</returns>
        public static string ReplaceFirst(this string value, char charToReplace, char replacement)
        {
            Regex regEx = new Regex(charToReplace.ToString(CultureInfo.CurrentCulture), RegexOptions.Multiline);
            return regEx.Replace(value, replacement.ToString(CultureInfo.CurrentCulture), 1);
        }

        /// <summary>
        /// Replaces the last occurrence of a character with another character in a given string.
        /// The replacement is case insensitive.
        /// </summary>
        /// <param name="value">A. <see cref="System.String" /> Reference. </param>
        /// <param name="charToReplace">The character to replace.</param>
        /// <param name="replacement">The character by which to replace.</param>
        /// <returns>Copy of string with the character replaced.</returns>
        public static string ReplaceLast(this string value, char charToReplace, char replacement)
        {
            int index = value.LastIndexOf(charToReplace);
            if (index < 0)
            {
                return value;
            }

            StringBuilder sb = new StringBuilder(value.Length - 2);
            sb.Append(value.Substring(0, index));
            sb.Append(replacement);
            sb.Append(value.Substring(index + 1, value.Length - index - 1));
            return sb.ToString();
        }

        /// <summary>
        /// Replaces the last occurrence of a string with another string in a given string.
        /// The replacement is case insensitive.
        /// </summary>
        /// <param name="value">A. <see cref="System.String" /> Reference. </param>
        /// <param name="stringToReplace">The string to replace.</param>
        /// <param name="replacement">The string by which to be replaced.</param>
        /// <returns>Copy of string with the string replaced.</returns>
        public static string ReplaceLast(this string value, string stringToReplace, string replacement)
        {
            int index = value.LastIndexOf(stringToReplace, StringComparison.CurrentCulture);

            if (index < 0)
            {
                return value;
            }

            StringBuilder sb = new StringBuilder(value.Length - stringToReplace.Length + replacement.Length);
            sb.Append(value.Substring(0, index));
            sb.Append(replacement);
            sb.Append(value.Substring(index + stringToReplace.Length, value.Length - index - stringToReplace.Length));

            return sb.ToString();
        }

        /// <summary>
        /// Replaces Token String with the specified values.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="list">The list.</param>
        /// <param name="usePlaceHolder">Use Placeholder [[ ]].</param>
        /// <returns>Replaced Token.</returns>
        /// <exception cref="System.FormatException">Invalid Format</exception>
        public static string ReplaceToken(this string value, IDictionary<string, string> list, bool usePlaceHolder = false)
        {
            if (!value.IsEmpty())
            {
                Dictionary<string, string> tokenDictionary = new Dictionary<string, string>(list, StringComparer.OrdinalIgnoreCase);
                if (tokenDictionary.Count == 0)
                {
                    return value;
                }

                try
                {
                    return MessageTokenRegex.Replace(
                      value,
                        m =>
                        {
                            if (tokenDictionary.ContainsKey(m.Groups[4].Value))
                            {
                                return tokenDictionary[m.Groups[4].Value];
                            }

                            return usePlaceHolder ? "[[$" + m.Groups[4].Value + "$]]" : "$" + m.Groups[4].Value + "$";
                        });
                }
                catch (Exception exception)
                {
                    throw new FormatException("Invalid Format", exception);
                }
            }

            return string.Empty;
        }
        
        /// <summary>
        /// Reverses a string.
        /// </summary>
        /// <param name="value">A. <see cref="System.String" /> Reference. </param>
        /// <returns>Copy of the reversed string.</returns>
        public static string Reverse(this string value)
        {
            char[] reverse = new char[value.Length];
            for (int i = 0, k = value.Length - 1; i < value.Length; i++, k--)
            {
                if (char.IsSurrogate(value[k]))
                {
                    reverse[i + 1] = value[k--];
                    reverse[i++] = value[k];
                }
                else
                {
                    reverse[i] = value[k];
                }
            }

            return new string(reverse);
        }

        /// <summary>
        /// Convert specified string to the byte array.
        /// </summary>
        /// <param name="value">The input.</param>
        /// <returns>Converted Byte Array.</returns>
        public static byte[] ToByteArray(this string value)
        {
            byte[] byteArray = new byte[0];

            if (!string.IsNullOrEmpty(value))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    StreamWriter writer = new StreamWriter(stream);
                    writer.Write(value);
                    writer.Flush();

                    byteArray = stream.ToArray();
                }
            }

            return byteArray;
        }

        /// <summary>
        /// Convert byte array to string.
        /// </summary>
        /// <param name="value">The bytes array to convert.</param>
        /// <returns>Convert Byte Array to string.</returns>
        public static string StringConvert(this byte[] value)
        {
            string s = string.Empty;
            if (value != null && value.Length > 0)
            {
                using (MemoryStream stream = new MemoryStream(value))
                {
                    stream.Position = 0;
                    StreamReader reader = new StreamReader(stream);
                    s = reader.ReadToEnd();
                }
            }

            return s;
        }

        /// <summary>
        /// Determines whether the specified string is valid email.
        /// </summary>
        /// <param name="email">The string to check for valid email.</param>
        /// <returns>
        /// <see langword="true" />If the specified string is email; otherwise,.<see langword="false" />.
        /// </returns>
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrEmpty(email) || email.Length > 255)
            {
                return false;
            }

            return EmailRegex.IsMatch(email);
        }

        /// <summary>
        /// Ensures the end with semi colon.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Formatted string value.</returns>
        public static string EnsureEndWithSemiColon(this string value)
        {
            if (!value.IsEmpty())
            {
                int length = value.Length;

                if ((length > 0) && (value[length - 1] != ';'))
                {
                    return value + ";";
                }
            }

            return value;
        }

        /// <summary>
        /// Formats the string value for Javascript use.
        /// </summary>
        /// <param name="value">Value to be formatted.</param>
        /// <returns>Formatted string value.</returns>
        public static string ToJavascriptStringValue(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                switch (c)
                {
                    case '\'':
                        sb.Append("\\\'");
                        break;
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }

                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts a byte array to a string, using its byte order mark to convert it to the right encoding.
        /// </summary>
        /// <param name="buffer">An array of bytes to convert.</param>
        /// <returns>The byte as a string.</returns>
        public static string GetString(this byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
            {
                return string.Empty;
            }

            // Ansi as default
            Encoding encoding = Encoding.UTF8;

            /*
              EF BB BF      UTF-8 
              FF FE UTF-16  little endian 
              FE FF UTF-16  big endian 
              FF FE 00 00   UTF-32, little endian 
              00 00 FE FF   UTF-32, big-endian 
              */

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
            {
                encoding = Encoding.UTF8;
            }
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
            {
                encoding = Encoding.Unicode;
            }
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
            {
                encoding = Encoding.BigEndianUnicode; // utf-16be
            }
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
            {
                encoding = Encoding.UTF32;
            }
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
            {
                encoding = Encoding.UTF7;
            }

            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(buffer, 0, buffer.Length);
                stream.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts a byte array to a string, using its byte order mark to convert it to the right
        /// encoding.
        /// </summary>
        /// <param name="stream">
        /// An array of bytes to convert.
        /// </param>
        /// <returns>
        /// The byte as a string.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static string GetString(this Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Creates article slug from value
        /// </summary>
        /// <param name="value">String Value to create slug</param>
        /// <returns>Returns slug string with keywords separated by - character</returns>
        public static string ToSlug(this string value)
        {
            string slug = value.ToLower();

            // Replace characters specific fo croatian language
            // You don't need this part for english language
            // Also, you can replace other characters specific for other languages
            // e.g. ÃƒÂ© to e for French language etc.
            slug = slug.Replace("Ã„Â", "c");
            slug = slug.Replace("Ã„â€¡", "c");
            slug = slug.Replace("Ã…Â¡", "s");
            slug = slug.Replace("Ã…Â¾", "z");
            slug = slug.Replace("Ã„â€˜", "dj");

            // Replace - with empty space
            slug = slug.Replace("-", " ");

            // Replace unwanted characters with space
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", " ");

            // Replace multple white spaces with single space
            slug = Regex.Replace(slug, @"\s+", " ").Trim();

            // Replace white space with -
            slug = slug.Replace(" ", "-");

            return slug;
        }

        /// <summary>
        /// A string extension method that replace new line.
        /// </summary>
        /// <param name="val">The val to act on.</param>
        /// <param name="replaceString">(optional) the replace string.</param>
        /// <returns>Replaced String.</returns>
        public static string ReplaceNewLine(this string val, string replaceString = "<br />")
        {
            if (string.IsNullOrEmpty(val))
            {
                return string.Empty;
            }

            string text = val.Replace("\r\n", replaceString);
            text = text.Replace("\r", replaceString);
            text = text.Replace("\n", replaceString);

            return text;
        }

        /// <summary>
        /// A string extension method that with version.
        /// </summary>
        /// <param name="value">A<see cref="String"></see>containing zero or more format items.</param>
        /// <returns>Url String With Version Info added.</returns>
        public static string WithVersion(this string value)
        {
            if (value.IndexOf('?') == -1)
            {
                return value + "?v=" + FrameworkConstants.TimeStamp;
            }

            return value + "&v=" + FrameworkConstants.TimeStamp;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        /// A string extension method that converts a value to a md5 hash.
        /// </summary>
        /// <remarks>
        /// LM ANWAR, 6/2/2013.
        /// </remarks>
        /// <param name="value">
        /// A<see cref="String"></see>containing zero or more format items.
        /// </param>
        /// <returns>
        /// value as a string.
        /// </returns>
        /// -------------------------------------------------------------------------------------------------
        public static string ToMD5Hash(this string value)
        {
            return Cryptography.CreateHash(HashMode.MD5, value);
        }
    }
}