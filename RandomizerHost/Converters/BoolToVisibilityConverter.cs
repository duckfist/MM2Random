using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace RandomizerHost.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public Object Convert(Object in_Value, Type in_TargetType, Object in_Parameter, CultureInfo in_Culture)
        {
            /*
            if (null == in_Value)
            {
                return null;
            }

            if (in_Value is Boolean value)
            {
                return value ? Visibility.Visible : Visibility.Hidden;
            }
            else
            {
                throw new NotSupportedException();
            }
            */
            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
