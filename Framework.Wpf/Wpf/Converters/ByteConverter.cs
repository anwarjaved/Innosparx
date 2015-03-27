namespace Framework.Wpf.Converters
{
    using System;
    using System.Windows.Data;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Convert Bytes To String Values.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 03/30/2014 10:08 PM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class ByteConverter : IValueConverter
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
            string size = "0 KB";

            if (value != null)
            {
                long byteCount = (long)value;

                if (byteCount >= 1073741824)
                    size = String.Format("{0:##.##}", byteCount / 1073741824) + " GB";
                else if (byteCount >= 1048576)
                    size = String.Format("{0:##.##}", byteCount / 1048576) + " MB";
                else if (byteCount >= 1024)
                    size = String.Format("{0:##.##}", byteCount / 1024) + " KB";
                else if (byteCount > 0 && byteCount < 1024)
                    size = "1 KB";    //Bytes are unimportant ;)            

            }

            return size;
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
