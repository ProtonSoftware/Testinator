using Testinator.Server.Core;
using System;
using System.Globalization;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// A converter that takes in a boolean and reurns
    /// the FontAwesome string for that icon
    /// </summary>
    public class BooleanToAwesomeConverter : BaseValueConverter<BooleanToAwesomeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return IconType.UnCheck.ToFontAwesome();
            return IconType.Check.ToFontAwesome();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}