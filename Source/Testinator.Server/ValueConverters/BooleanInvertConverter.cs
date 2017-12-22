using System;
using System.Globalization;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Inverts a boolean value
    /// </summary>
    public class BooleanInvertConverter : BaseValueConverter<BooleanInvertConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}