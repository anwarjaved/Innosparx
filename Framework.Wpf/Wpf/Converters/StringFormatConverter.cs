namespace Framework.Wpf.Converters
{
    using System;
    using System.Windows.Data;

    [ValueConversion(typeof(object), typeof(string))]
    public class StringFormatConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string format = parameter as string;
            if (!string.IsNullOrEmpty(format))
            {
                return string.Format(culture, format, value);
            }
            else
            {
                return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                    System.Globalization.CultureInfo culture)
        {
            return null;
        }     
    }
}
