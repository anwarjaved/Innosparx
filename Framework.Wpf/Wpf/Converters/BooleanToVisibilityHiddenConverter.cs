namespace Framework.Wpf.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    ///-------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A boolean to visibility hidden converter.
    /// </summary>
    ///
    /// <remarks>
    ///     Anwar Javed, 04/03/2014 9:55 AM.
    /// </remarks>
    ///-------------------------------------------------------------------------------------------------
    public class BooleanToVisibilityHiddenConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolean = false;
            if (value is bool) boolean = (bool)value;
            return boolean ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }
}
