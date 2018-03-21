using System;
using System.Globalization;
using System.Windows.Media;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// A converter that takes in a rgb string (#rrggbb) and converts it to <see cref="SolidColorBrush"/>
    /// </summary>
    public class StringRGBToBrushConverter : BaseValueConverter<StringRGBToBrushConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (SolidColorBrush)(new BrushConverter().ConvertFrom($"#{value}"));
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}