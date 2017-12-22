using System;
using System.Globalization;
using System.Windows;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Inverts a boolean value
    /// </summary>
    public class BooleanInvertConverter : BaseValueConverter<BooleanInvertConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}