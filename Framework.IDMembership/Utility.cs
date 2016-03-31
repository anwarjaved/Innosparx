namespace Framework.IDMembership
{
    using System;
    using System.Collections.Specialized;

    /// <summary>
    /// Utility Methods.
    /// </summary>
    internal static class Utility
    {
        internal static bool GetBooleanValue(NameValueCollection config, string valueName, bool defaultValue)
        {
            bool flag;
            string str = config[valueName];
            if (str == null)
            {
                return defaultValue;
            }

            bool.TryParse(str, out flag);
            return flag;
        }

        internal static int GetIntegerValue(
          NameValueCollection config, string valueName, int defaultValue, bool zeroAllowed, int maxValueAllowed)
        {
            int num;
            string s = config[valueName];
            if (s == null)
            {
                return defaultValue;
            }

            if (!int.TryParse(s, out num))
            {
                if (zeroAllowed)
                {
                    throw new ArgumentOutOfRangeException(valueName, @"Non-negative number required.");
                }
            }

            if (zeroAllowed && (num < 0))
            {
                throw new ArgumentOutOfRangeException(valueName, "'{0}' must be greater than zero.".FormatString(valueName));
            }

            if (!zeroAllowed && (num <= 0))
            {
                throw new ArgumentOutOfRangeException(valueName, @"Non-negative number required.");
            }

            if ((maxValueAllowed > 0) && (num > maxValueAllowed))
            {
                throw new ArgumentException("The value '{0}' can not be greater than '{1}'.".FormatString(maxValueAllowed), valueName);
            }

            return num;
        }

        internal static bool ValidateParameter(
          ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int minSize, int maxSize)
        {
            if (param == null)
            {
                return !checkForNull;
            }

            param = param.Trim();
            return (((!checkIfEmpty || param.Length >= 1) && (maxSize <= 0 || param.Length <= maxSize)) && (minSize <= 0 || param.Length >= minSize)) && (!checkForCommas || !param.Contains(","));
        }

        internal static void CheckParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int minSize, int maxSize, string paramName)
        {
            if (param == null)
            {
                if (checkForNull)
                {
                    throw new ArgumentNullException(paramName);
                }

                return;
            }

            param = param.Trim();
            if (checkIfEmpty && param.Length < 1)
            {
                throw new ArgumentException(paramName, "The parameter '{0}' must not be empty.".FormatString(paramName));
            }

            if (minSize > 0 && param.Length < minSize)
            {
                throw new ArgumentException(
                  paramName,
                  "The parameter '{0}' is too small: it must exceed {1} chars in length.".FormatString(paramName, minSize));
            }

            if (maxSize > 0 && param.Length > maxSize)
            {
                throw new ArgumentException(
                  paramName,
                  "The parameter '{0}' is too long: it must not exceed {1} chars in length.".FormatString(paramName, maxSize));
            }

            if (checkForCommas && param.Contains(","))
            {
                throw new ArgumentException(paramName, "The parameter '{0}' must not contain commas.".FormatString(paramName));
            }
        }
    }
}
