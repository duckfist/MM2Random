using System;
using System.Globalization;
using System.Reflection;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace RandomizerHost.Converters
{
    public class BitmapAssetValueConverter : IValueConverter
    {
        public Object Convert(Object in_Value, Type in_TargetType, Object in_Parameter, CultureInfo in_Culture)
        {
            if (null == in_Value)
            {
                return null;
            }

            if (in_Value is String rawUri && in_TargetType == typeof(IBitmap))
            {
                Uri uri;

                // Allow for assembly overrides
                if (rawUri.StartsWith("avares://"))
                {
                    uri = new Uri(rawUri);
                }
                else
                {
                    string assemblyName = Assembly.GetEntryAssembly().GetName().Name;
                    uri = new Uri($"avares://{assemblyName}{rawUri}");
                }

                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                var asset = assets.Open(uri);

                return new Bitmap(asset);
            }

            throw new NotSupportedException();
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
