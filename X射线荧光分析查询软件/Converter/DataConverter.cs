namespace X射线荧光分析查询软件.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class DataConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // throw new NotImplementedException();
            return ((double)value <= 0) ? "" : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
