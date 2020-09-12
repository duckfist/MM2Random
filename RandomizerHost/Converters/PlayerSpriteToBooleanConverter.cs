using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using MM2Randomizer;

namespace RandomizerHost.Converters
{
    public class PlayerSpriteToBooleanConverter : IValueConverter
    {
        public Object Convert(Object in_Value, Type in_TargetType, Object in_Parameter, CultureInfo in_Culture)
        {
            if (typeof(Boolean) != in_TargetType)
            {
                return new BindingNotification(
                    new NotSupportedException(@"Only Boolean target type is allowed"),
                    BindingErrorType.Error);
            }

            if (null == in_Value)
            {
                return new BindingNotification(
                    new ArgumentNullException(nameof(in_Value)),
                    BindingErrorType.Error);
            }

            if (null == in_Parameter)
            {
                return new BindingNotification(
                    new ArgumentNullException(nameof(in_Parameter)),
                    BindingErrorType.Error);
            }

            if (in_Parameter is String stringParameter)
            {
                Object parameter = Enum.Parse(typeof(PlayerSprite), stringParameter);

                Boolean retval = parameter.Equals(in_Value);
                return retval;
            }
            else
            {
                return (in_Parameter == in_Value);
            }
        }

        public Object ConvertBack(Object in_Value, Type in_TargetType, Object in_Parameter, CultureInfo in_Culture)
        {
            throw new NotSupportedException();
        }
    }
}
