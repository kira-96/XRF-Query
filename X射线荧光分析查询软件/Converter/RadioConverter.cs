namespace X射线荧光分析查询软件.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class RadioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString().Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToBoolean(value) ? parameter : null;
        }
    }
}
