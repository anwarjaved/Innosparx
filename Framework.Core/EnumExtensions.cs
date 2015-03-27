using System.Globalization;

namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Enum Extensions.
    /// </summary>
    public static class EnumExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that enum to list.
        /// </summary>
        ///
        /// <param name="enumType">
        ///     The enumType to act on.
        /// </param>
        /// <param name="skipValues">
        ///     A variable-length parameters list containing skip values.
        /// </param>
        ///
        /// <returns>
        ///     A list of.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static ICollection<Pair<string, long>> EnumToList(this Type enumType, params long[] skipValues)
        {
            Array values = Enum.GetValues(enumType);
            List<Pair<string, long>> list = new List<Pair<string, long>>();
            List<long> skipList = new List<long>();

            if (skipValues != null)
            {
                skipList.AddRange(skipValues);
            }

            foreach (Enum @enum in values)
            {
                var value = Convert.ToInt32(@enum.ToString("D"));

                if (!skipList.Contains(value))
                {
                    list.Add(new Pair<string, long>(@enum.ToLocalizedDescription(), value));
                }
            }
            return list;
        }

        private static void CheckIsEnum<T>(bool withFlags)
        {
            var enumType = typeof(T);
            var isNullableType = enumType.IsNullableType();
            Type type = isNullableType ? Nullable.GetUnderlyingType(enumType) : enumType;

            if (!type.IsEnum)
                throw new ArgumentException(string.Format("Type '{0}' is not an enum", typeof(T).FullName));
            if (withFlags && !Attribute.IsDefined(type, typeof(FlagsAttribute)))
                throw new ArgumentException(string.Format("Type '{0}' doesn't have the 'Flags' attribute", type.FullName));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A T extension method that query if 'value' is flag set.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 1:47 PM.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="flag">
        ///     The flag.
        /// </param>
        ///
        /// <returns>
        ///     true if flag set&lt; t&gt;, false if not.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static bool IsFlagSet<T>(this T value, T flag) where T : struct
        {
            CheckIsEnum<T>(true);
            
            long lValue = Convert.ToInt64(value);
            long lFlag = Convert.ToInt64(flag);
            return (lValue & lFlag) == lFlag;
        }

        public static IEnumerable<T> GetFlags<T>(this T value) where T : struct
        {
            CheckIsEnum<T>(true);
            return Enum.GetValues(typeof(T)).Cast<T>().Where(flag => value.IsFlagSet(flag));
        }

        public static T ClearFlags<T>(this T value, T flags) where T : struct
        {
            return value.SetFlags(flags, false);
        }

        public static T SetFlags<T>(this T value, T flags) where T : struct
        {
            return value.SetFlags(flags, true);
        }

        public static T SetFlags<T>(this T value, T flags, bool on) where T : struct
        {
            CheckIsEnum<T>(true);
            long lValue = Convert.ToInt64(value);
            long lFlag = Convert.ToInt64(flags);
            if (on)
            {
                lValue |= lFlag;
            }
            else
            {
                lValue &= (~lFlag);
            }
            return (T)Enum.ToObject(typeof(T), lValue);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An IEnumerable&lt;T&gt; extension method that combine flags.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 09/16/2013 1:47 PM.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="flags">
        ///     The flags to act on.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static T CombineFlags<T>(this IEnumerable<T> flags) where T : struct
        {
            CheckIsEnum<T>(true);
            long lValue = flags.Select(flag => Convert.ToInt64(flag)).Aggregate<long, long>(0, (current, lFlag) => current | lFlag);
            return (T)Enum.ToObject(typeof(T), lValue);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that enum to dictionary.
        /// </summary>
        ///
        /// <param name="enumType">
        ///     The enumType to act on.
        /// </param>
        /// <param name="skipValues">
        ///     A variable-length parameters list containing skip values.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IDictionary<string, long> EnumToDictionary(this Type enumType, params long[] skipValues)
        {
            Array values = Enum.GetValues(enumType);
            IDictionary<string, long> dictionary = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            List<long> skipList = new List<long>();

            if (skipValues != null)
            {
                skipList.AddRange(skipValues);
            }

            foreach (Enum @enum in values)
            {
                var value = Convert.ToInt32(@enum.ToString("D"));

                if (!skipList.Contains(value))
                {
                    dictionary.Add(@enum.ToLocalizedDescription(), value);
                }
            }
            return dictionary;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that enum to dictionary.
        /// </summary>
        ///
        /// <param name="enumType">
        ///     The enumType to act on.
        /// </param>
        /// <param name="skipValues">
        ///     A variable-length parameters list containing skip values.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IDictionary<string, long> EnumToDictionaryValues(this Type enumType, params long[] skipValues)
        {
            Array values = Enum.GetValues(enumType);
            IDictionary<string, long> dictionary = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            List<long> skipList = new List<long>();

            if (skipValues != null)
            {
                skipList.AddRange(skipValues);
            }

            foreach (Enum @enum in values)
            {
                var value = Convert.ToInt32(@enum.ToString("D"));

                if (!skipList.Contains(value))
                {
                    dictionary.Add(@enum.ToString("G"), value);
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Toes the description.
        /// </summary>
        /// <param name="enumeration">The enumeration.</param>
        /// <returns>DEscription of Current Enum.</returns>
        public static string ToDescription(this Enum enumeration)
        {
            // Get the type
            Type type = enumeration.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(enumeration.ToString());

            // Get the stringvalue attributes. 
            if (fieldInfo != null)
            {
                System.ComponentModel.DescriptionAttribute attribs =
                    (from attr in fieldInfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false) select attr).Cast<System.ComponentModel.DescriptionAttribute>().FirstOrDefault();

                // Return the first if there was a match.
                if (attribs != null)
                {
                    if (!string.IsNullOrWhiteSpace(attribs.Description))
                    {
                        return attribs.Description;
                    }
                }

                return enumeration.ToString("G");
            }

            return null;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An Enum extension method that converts an enumeration to a localized description.
        /// </summary>
        ///
        /// <param name="enumeration">
        ///     The enumeration.
        /// </param>
        ///
        /// <returns>
        ///     enumeration as a string.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static string ToLocalizedDescription(this Enum enumeration)
        {
            // Get the type
            Type type = enumeration.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(enumeration.ToString());

            // Get the stringvalue attributes
            DescriptionAttribute attribs =
              (from attr in fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) select attr).Cast<DescriptionAttribute>().FirstOrDefault();

            // Return the first if there was a match.
            if (attribs != null)
            {
                string desc = attribs.GetLocalizedDescription(CultureInfo.CurrentUICulture);
                if (!string.IsNullOrWhiteSpace(desc))
                {
                    return desc;
                }
            }
            else
            {
                System.ComponentModel.DescriptionAttribute descriptionAttribute =
             (from attr in fieldInfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false) select attr).Cast<System.ComponentModel.DescriptionAttribute>().FirstOrDefault();

                // Return the first if there was a match.
                if (descriptionAttribute != null)
                {
                    string desc = descriptionAttribute.Description;
                    if (!string.IsNullOrWhiteSpace(desc))
                    {
                        return desc;
                    }
                }
            }

            return enumeration.ToString("G");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An Enum extension method that gets the attributes.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="enumeration">
        ///     The enumeration.
        /// </param>
        /// <param name="inherit">
        ///     (optional) the inherit.
        /// </param>
        ///
        /// <returns>
        ///     The attributes&lt; t&gt;
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static IReadOnlyList<T> GetAttributes<T>(this Enum enumeration, bool inherit = false) where T : Attribute
        {
            Type type = enumeration.GetType();
            FieldInfo fieldInfo = type.GetField(enumeration.ToString());
            return fieldInfo.GetCustomAttributes(typeof(T), inherit).OfType<T>().ToList();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An Enum extension method that gets an attribute.
        /// </summary>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="enumeration">
        ///     The enumeration.
        /// </param>
        /// <param name="inherit">
        ///     (optional) the inherit.
        /// </param>
        ///
        /// <returns>
        ///     The attribute&lt; t&gt;
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static T GetAttribute<T>(this Enum enumeration, bool inherit = false) where T : Attribute
        {
            return enumeration.GetAttributes<T>(inherit).FirstOrDefault();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that parse enum.
        /// </summary>
        ///
        /// <param name="enumType">
        ///     The enumType to act on.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        ///
        /// <returns>
        ///     .
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public static object ParseEnum(this Type enumType, string value)
        {
            var isNullableType = enumType.IsNullableType();
            Type type = isNullableType ? Nullable.GetUnderlyingType(enumType) : enumType;

            var pairs = type.EnumToDictionary();

            if (pairs.ContainsKey(value))
            {
                return Enum.Parse(type, pairs[value].ToString());
            }

            return Enum.Parse(type, value);
        }
    }
}
