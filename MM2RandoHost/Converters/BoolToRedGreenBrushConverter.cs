using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MM2RandoHost.Converters
{
    public class BoolToRedGreenBrushConverter : IValueConverter
    {
        private static readonly Brush red = new SolidColorBrush(Colors.Red);
        private static readonly Brush green = new SolidColorBrush(Colors.Green);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool myBool))
            {
                return red;
            }

            return myBool ? green : red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
