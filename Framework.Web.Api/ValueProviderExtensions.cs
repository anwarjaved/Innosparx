using System;

namespace Framework
{
    using System.Security;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Value provider extensions.
    /// </summary>
    ///
    /// <remarks>
    ///     LM ANWAR, 5/1/2013.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public static class ValueProviderExtensions
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Retrieves a value object using the specified key.
        /// </summary>
        ///
        /// <remarks>
        ///     LM ANWAR, 5/1/2013.
        /// </remarks>
        ///
        /// <typeparam name="T">
        ///     Generic type parameter.
        /// </typeparam>
        /// <param name="modelBindingContext">
        ///     Context for the model binding.
        /// </param>
        /// <param name="key">
        ///     The key of the value object to retrieve.
        /// </param>
        ///
        /// <returns>
        ///     The value object for the specified key.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        public static T GetValueOrDefault<T>(this System.Web.Http.ModelBinding.ModelBindingContext modelBindingContext, string key) where T : struct
        {
            var result = modelBindingContext.ValueProvider.GetValue(key);

            if (result != null)
            {
                return (T)Convert.ChangeType(result.RawValue, typeof(T));
            }

            return default(T);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A System.Web.Http.ModelBinding.ModelBindingContext extension method that gets value or
        ///     default.
        /// </summary>
        ///
        /// <remarks>
        ///     LM ANWAR, 5/1/2013.
        /// </remarks>
        ///
        /// <param name="modelBindingContext">
        ///     Context for the model binding.
        /// </param>
        /// <param name="key">
        ///     The key of the value object to retrieve.
        /// </param>
        ///
        /// <returns>
        ///     The value or default.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        
        public static string GetValueOrDefault(this System.Web.Http.ModelBinding.ModelBindingContext modelBindingContext, string key)
        {
            var result = modelBindingContext.ValueProvider.GetValue(key);

            if (result != null)
            {
                return Convert.ToString(result.RawValue);
            }

            return string.Empty;
        }
    }
}
