namespace Framework.Wpf.Converters
{
    using System;
    using System.Windows.Data;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A percent converter.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 03/30/2014 10:08 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class PercentConverter : IValueConverter
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Converts a value.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/30/2014 10:08 PM.
        /// </remarks>
        ///
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="targetType">
        ///     Type of the target.
        /// </param>
        /// <param name="parameter">
        ///     The parameter.
        /// </param>
        /// <param name="culture">
        ///     The culture.
        /// </param>
        ///
        /// <returns>
        ///     A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string percent = "0%";

            if (value != null)
                percent = (int)value + "%";

            return percent;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Converts a value.
        /// </summary>
        ///
        /// <remarks>
        ///     Anwar Javed, 03/30/2014 10:08 PM.
        /// </remarks>
        ///
        /// <exception cref="NotImplementedException">
        ///     Thrown when the requested operation is unimplemented.
        /// </exception>
        ///
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <param name="targetType">
        ///     Type of the target.
        /// </param>
        /// <param name="parameter">
        ///     The parameter.
        /// </param>
        /// <param name="culture">
        ///     The culture.
        /// </param>
        ///
        /// <returns>
        ///     A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        ///-------------------------------------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
